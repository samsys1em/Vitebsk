using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Popups;
using Scripts.Data.Place;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.Popups.ImagePopup
{
    public class ImagePopup : BasePopup,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Images")]
        [SerializeField] private Image _imageFront;
        [SerializeField] private Image _imageBack;

        [Header("UI")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Button _buttonPrevious;
        [SerializeField] private Button _buttonNext;

        [Header("Animation")]
        [SerializeField] private float _fadeDuration = 0.3f;

        [Header("Swipe")]
        [Tooltip("Минимальное смещение по X для свайпа")]
        [SerializeField] private float _swipeThreshold = 100f;

        private List<PlaceImage> _placeImages;
        private int _currentIndex;

        private Sequence _fadeSequence;

        private bool _isDragging;
        private Vector2 _dragStartPointer;
        private Vector2 _dragDelta;

        public override string Id => PopupId.Image;

        private void Awake()
        {
            _buttonPrevious.onClick.AddListener(OnPreviousButtonDown);
            _buttonNext.onClick.AddListener(OnNextButtonDown);
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            var param = openParam as ImagePopupOpenParam;
            if (param == null)
                return;

            _imageBack.sprite = null;
            _imageFront.sprite = null;

            _placeImages = param.PlaceData.Images;
            _currentIndex = _placeImages.IndexOf(param.PlaceImage);

            UpdateInfo(false);
        }

        private void OnPreviousButtonDown()
        {
            if (_currentIndex == 0)
                return;

            _currentIndex--;
            UpdateInfo();
        }

        private void OnNextButtonDown()
        {
            if (_currentIndex == _placeImages.Count - 1)
                return;

            _currentIndex++;
            UpdateInfo();
        }

        private void UpdateInfo(bool crossFade = true)
        {
            var placeImage = _placeImages[_currentIndex];

            CrossFadeSprite(placeImage.Image, crossFade ? _fadeDuration : 0f);

            _titleText.SetText(placeImage.Title);

            UpdateNavigateButtons();
        }

        private void UpdateNavigateButtons()
        {
            _buttonPrevious.gameObject.SetActive(_currentIndex > 0);
            _buttonNext.gameObject.SetActive(_currentIndex < _placeImages.Count - 1);
        }

        private void CrossFadeSprite(Sprite newSprite, float duration)
        {
            _imageBack.sprite = _imageFront.sprite;
            _imageFront.sprite = newSprite;

            _imageBack.DOFade(1f, 0);
            _imageFront.DOFade(0f, 0);

            _fadeSequence?.Kill();
            _fadeSequence = DOTween.Sequence();

            _fadeSequence
                .Join(_imageBack.DOFade(0f, duration / 2))
                .Join(_imageFront.DOFade(1f, duration));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _dragStartPointer = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;

            _dragDelta = eventData.position - _dragStartPointer;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;

            _isDragging = false;

            if (Mathf.Abs(_dragDelta.x) < _swipeThreshold)
                return;

            if (Mathf.Abs(_dragDelta.x) < Mathf.Abs(_dragDelta.y))
                return;

            if (_dragDelta.x < 0)
                OnNextButtonDown();
            else
                OnPreviousButtonDown();
        }
    }


    public class ImagePopupOpenParam : IUIOpenParam
    {
        public ImagePopupOpenParam(PlaceData placeData, PlaceImage image)
        {
            PlaceData = placeData;
            PlaceImage = image;
        }

        public PlaceData PlaceData { get; }
        public PlaceImage PlaceImage { get; }
    }
}