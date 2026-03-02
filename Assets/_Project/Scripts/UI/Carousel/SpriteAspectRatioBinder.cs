using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Carousel
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class SpriteAspectRatioBinder : MonoBehaviour
    {
        [SerializeField] private AspectRatioFitter _fitter;
        [SerializeField] private Image _image;

        private void Reset()
        {
            _image = GetComponent<Image>();
            if (_fitter == null)
                _fitter = gameObject.GetComponent<AspectRatioFitter>() ?? gameObject.AddComponent<AspectRatioFitter>();

            _fitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            UpdateAspect();
        }

        private void Awake()
        {
            if (_image == null) _image = GetComponent<Image>();
            if (_fitter == null) _fitter = GetComponent<AspectRatioFitter>();
            if (_fitter != null) _fitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            UpdateAspect();
        }

        private void OnEnable()
        {
            UpdateAspect();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            if (_image == null) _image = GetComponent<Image>();
            if (_fitter == null) _fitter = GetComponent<AspectRatioFitter>();
            if (_fitter != null) _fitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            UpdateAspect();
        }
#endif

        /// <summary>
        /// Вызови вручную, если ты меняешь sprite в рантайме.
        /// </summary>
        public void UpdateAspect()
        {
            if (_image == null || _fitter == null) return;

            var spr = _image.sprite;
            if (spr == null)
                return;

            var r = spr.rect;
            if (r.height <= 0f)
                return;

            // реальное соотношение сторон спрайта
            _fitter.aspectRatio = r.width / r.height;
            // PreserveAspect у Image не нужен, т.к. размер задаёт fitter
            _image.preserveAspect = false;
        }

        /// <summary>
        /// Опционально — удобный setter для смены спрайта с автоматическим обновлением.
        /// </summary>
        public void SetSprite(Sprite s)
        {
            if (_image == null) _image = GetComponent<Image>();
            _image.sprite = s;
            UpdateAspect();
            // Если нужно, форсируем перелэйаут:
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
    }
}