using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Carousel
{
    [RequireComponent(typeof(ScrollRect))]
    public class LoopingScrollRectVariable : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform viewport;
        [SerializeField] private RectTransform itemPrefab;

        [Header("Data")]
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();

        [Header("Pooling")]
        [Tooltip("Покрытие вьюпорта элементами (во сколько раз ширина пулла больше ширины viewport).")]
        [SerializeField] private float coverage = 2.0f;
        [SerializeField] private int extraBuffer = 2;
        [SerializeField] private float offscreenBufferPx = 8f; // небольшой доп. буфер, чтобы не срабатывало на границе

        [Header("Perf")]
        [SerializeField] private float recenterThreshold = 40000f;  // реже рецентрить
        [SerializeField] private float minDeltaToProcess = 0.25f;   // игнорировать микросдвиги

        private readonly List<RectTransform> pool = new List<RectTransform>();
        private HorizontalLayoutGroup hlg;
        private Vector2 lastContentPos;
        private int headDataIndex;

        void Reset()
        {
            scrollRect = GetComponent<ScrollRect>();
            if (scrollRect) { content = scrollRect.content; viewport = scrollRect.viewport; }
        }

        void Awake()
        {
            if (!scrollRect) scrollRect = GetComponent<ScrollRect>();
            if (!content)    content    = scrollRect.content;
            if (!viewport)   viewport   = scrollRect.viewport;

            scrollRect.horizontal   = true;
            scrollRect.vertical     = false;
            scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

            hlg = content.GetComponent<HorizontalLayoutGroup>();

            // слушаем только изменения скролла
            scrollRect.onValueChanged.AddListener(_ => OnScrollChanged());
        }

        void OnEnable() => Rebuild();

        public void SetData(List<Sprite> data)
        {
            sprites = data ?? new List<Sprite>();
            Rebuild();
        }

        void Rebuild()
        {
            // очистить старый пул
            foreach (var rt in pool)
                if (rt) Destroy(rt.gameObject);
            pool.Clear();

            if (!itemPrefab || sprites.Count == 0 || !content || !viewport) return;

            // сброс позиции
            content.anchoredPosition = Vector2.zero;
            lastContentPos = content.anchoredPosition;
            headDataIndex = 0;

            // создать столько элементов, чтобы покрыть viewport * coverage + буфер
            float target = Mathf.Max(1f, coverage) * viewport.rect.width;
            float sum = 0f;
            int idx = headDataIndex;
            int guard = 2000;

            while (sum < target && guard-- > 0)
            {
                var rt = Instantiate(itemPrefab, content);
                rt.gameObject.SetActive(true);
                Bind(rt, sprites[idx]);
                ForceLayoutImmediate(); // только при добавлении
                pool.Add(rt);
                sum += rt.rect.width + (hlg ? hlg.spacing : 0f);
                idx = Next(idx);
            }

            for (int i = 0; i < extraBuffer; i++)
            {
                var rt = Instantiate(itemPrefab, content);
                rt.gameObject.SetActive(true);
                Bind(rt, sprites[idx]);
                ForceLayoutImmediate();
                pool.Add(rt);
                idx = Next(idx);
            }

            ForceLayoutImmediate(); // финальный прогон
        }

        void OnScrollChanged()
        {
            // реакция только при заметном сдвиге
            var cur = content.anchoredPosition;
            if (Mathf.Abs(cur.x - lastContentPos.x) < minDeltaToProcess) return;
            lastContentPos = cur;

            // Пробуем перекинуть несколько раз (для быстрой инерции)
            int safety = 32;
            bool movedAny = false;
            while (safety-- > 0 && (TryMoveLeftToRight() || TryMoveRightToLeft()))
            {
                movedAny = true;
                ForceLayoutImmediate(); // только после реальной перестановки
            }

            // рецентрирование — крайне редко
            if (Mathf.Abs(content.anchoredPosition.x) > recenterThreshold)
            {
                // близкий к нулю перенос: просто «сдвинем» всех детей на одинаковую величину,
                // изменяя anchoredPosition контента на кратное средней ширине (упрощенно — ширине viewport)
                float delta = Mathf.Round(content.anchoredPosition.x / viewport.rect.width) * viewport.rect.width;
                content.anchoredPosition -= new Vector2(delta, 0f);
            }

            if (movedAny) SyncHeadIndex();
        }

        // ===== Перекидывания =====

        bool TryMoveLeftToRight()
        {
            if (pool.Count == 0) return false;
            var first = pool[0];

            // границы первого относительно viewport
            if (!IsFullyLeftOfViewport(first, offscreenBufferPx)) return false;

            // перекидываем в конец
            pool.RemoveAt(0);
            first.SetAsLastSibling();

            int newData = TailNextIndex();
            Bind(first, sprites[newData]);
            pool.Add(first);
            return true;
        }

        bool TryMoveRightToLeft()
        {
            if (pool.Count == 0) return false;
            var last = pool[pool.Count - 1];

            if (!IsFullyRightOfViewport(last, offscreenBufferPx)) return false;

            // перекидываем в начало
            pool.RemoveAt(pool.Count - 1);
            last.SetAsFirstSibling();

            int newData = HeadPrevIndex();
            Bind(last, sprites[newData]);
            pool.Insert(0, last);
            return true;
        }

        // Проверка «полностью слева/справа» через реальные границы
        bool IsFullyLeftOfViewport(RectTransform child, float buffer)
        {
            var childBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, child);
            // childBounds.max.x — правый край относительно viewport (0 в центре viewport’а)
            return (childBounds.max.x + buffer) < -viewport.rect.width * 0.5f;
        }

        bool IsFullyRightOfViewport(RectTransform child, float buffer)
        {
            var childBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, child);
            // childBounds.min.x — левый край относительно viewport
            return (childBounds.min.x - buffer) > viewport.rect.width * 0.5f;
        }

        // ===== Data index helpers =====

        int Next(int i) => (i + 1) % sprites.Count;
        int Prev(int i) => (i - 1 + sprites.Count) % sprites.Count;

        int TailNextIndex()
        {
            // индекс данных после последнего
            int lastOffset = pool.Count - 1;
            int lastData = (headDataIndex + lastOffset) % sprites.Count;
            return Next(lastData);
        }

        int HeadPrevIndex() => Prev(headDataIndex);

        void SyncHeadIndex()
        {
            // определяем head по спрайту первого
            var img = pool[0].GetComponentInChildren<Image>(true);
            if (!img || !img.sprite) return;
            int i = sprites.IndexOf(img.sprite);
            if (i >= 0) headDataIndex = i;
        }

        // ===== Binding & layout =====

        void Bind(RectTransform item, Sprite sp)
        {
            var img = item.GetComponentInChildren<Image>(true);
            if (!img) return;
            if (img.sprite == sp) return; // не трогаем, если уже тот же спрайт (чтобы не «мигало»)
            img.sprite = sp;
            // если у айтема стоит ContentSizeFitter, он сам адаптирует ширину
            // img.SetNativeSize(); // обычно НЕ нужно для UI с HLG
        }

        void ForceLayoutImmediate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }
    }
}