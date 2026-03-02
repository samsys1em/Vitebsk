using System.Collections.Generic;
using Scripts.Data.Gallery;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.Carousel
{
    public class DynamicScrollView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private CarouselItem _carouselItemPrefab;
        [SerializeField] private List<GalleryData> _galleryDataList;
        [SerializeField] private float _scrollSpeed = 1f;
        [SerializeField] private bool _rightDirection = true;

        private ScrollRect _scrollRect;
        private readonly LinkedList<CarouselItem> _items = new LinkedList<CarouselItem>();
        private const float SpeedMultiplier = 0.01f;
        private float _halfCycleNorm;
        private bool _isDragging;


        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            var content = _scrollRect.content;


            for (var i = 0; i < 2; i++)
                foreach (var galleryData in _galleryDataList)
                {
                    var item = Instantiate(_carouselItemPrefab, content);
                    item.SetSprite(galleryData);
                    _items.AddFirst(item);
                }


            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            Canvas.ForceUpdateCanvases();

            _scrollRect.horizontalNormalizedPosition = .5f;
            var speedModifier = _scrollRect.content.rect.width / _scrollRect.viewport.rect.width;
            speedModifier = _rightDirection ? -speedModifier : speedModifier;
            _scrollSpeed *= speedModifier * SpeedMultiplier;

            RecalculateHalfCycleNorm();
        }

        private void RecalculateHalfCycleNorm()
        {
            var content = _scrollRect.content;
            var viewport = _scrollRect.viewport;

            if (content == null || viewport == null)
                return;

            if (_galleryDataList == null || _galleryDataList.Count == 0)
                return;

            if (content.childCount < _galleryDataList.Count * 2)
                return;


            var firstOfSet1 = content.GetChild(0) as RectTransform;
            var firstOfSet2 = content.GetChild(_galleryDataList.Count) as RectTransform;
            if (firstOfSet1 == null || firstOfSet2 == null)
                return;

            var setWidthPx = Mathf.Abs(firstOfSet2.anchoredPosition.x - firstOfSet1.anchoredPosition.x);
            var scrollablePx = Mathf.Max(1f, content.rect.width - viewport.rect.width);

            _halfCycleNorm = Mathf.Clamp01(setWidthPx / scrollablePx);
        }

        private void Update()
        {
            var pos = _scrollRect.horizontalNormalizedPosition;
            var half = _halfCycleNorm * 0.5f;
            var left = 0.5f - half;
            var right = 0.5f + half;

            if (pos < left)
                _scrollRect.horizontalNormalizedPosition = pos + _halfCycleNorm;
            else if (pos > right)
                _scrollRect.horizontalNormalizedPosition = pos - _halfCycleNorm;
        }

        private void FixedUpdate()
        {
            if (!_isDragging)
                _scrollRect.horizontalNormalizedPosition += _scrollSpeed * Time.fixedDeltaTime;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
        }

        public void Init(List<GalleryData> galleryDataList)
        {
            _galleryDataList = galleryDataList;
        }
    }
}