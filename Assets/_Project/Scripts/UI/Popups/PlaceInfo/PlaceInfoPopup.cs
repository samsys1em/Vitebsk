using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Popups;
using Scripts.Data.Place;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popups.PlaceInfo
{
    public class PlaceInfoPopup : BasePopup
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _buttonPrevious;
        [SerializeField] private Button _buttonNext;
        [SerializeField] private PlaceImageView _placeImageViewPrefab;
        [SerializeField] private RectTransform _placeImageViewContainer;
        [SerializeField] private float _slideStep = 250;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private int _maxImages = 4;

        private readonly List<PlaceImageView> _placeImageViews = new List<PlaceImageView>();
        private TweenerCore<float, float, FloatOptions> _scrollTween;


        public override string Id => PopupId.PlaceInfo;


        protected override void OnInitialization()
        {
            _buttonNext.onClick.AddListener(OnNextButtonDown);
            _buttonPrevious.onClick.AddListener(OnPreviousButtonDown);
            _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }

        private void OnScrollbarValueChanged(float value)
        {
            if (_placeImageViews.Count <= _maxImages)
                return;
            
            _buttonPrevious.gameObject.SetActive(value > 0);
            _buttonNext.gameObject.SetActive(value < 1);
        }

        private void OnNextButtonDown()
        {
            var targetValue = _scrollbar.value + 0.2f;
            if (targetValue > 0.9)
                targetValue = 1;
            _scrollTween?.Kill();
            _scrollTween = DOTween.To(
                () => _scrollbar.value,
                x => _scrollbar.value = x,
                targetValue,
                .1f
            );
        }

        private void OnPreviousButtonDown()
        {
            var targetValue = _scrollbar.value - 0.2f;
            if (targetValue < 0.1)
                targetValue = 0;
            _scrollTween?.Kill();
            _scrollTween = DOTween.To(
                () => _scrollbar.value,
                x => _scrollbar.value = x,
                targetValue,
                .1f
            );
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            var param = openParam as PlaceInfoPopupOpenParam;
            if (param == null)
                return;

            _nameText.SetText(param.PlaceData.Name);
            _descriptionText.SetText(param.PlaceData.Description);
            var images = param.PlaceData.Images;
            foreach (var placeImage in images)
            {
                var view = Instantiate(_placeImageViewPrefab, _placeImageViewContainer);
                view.SetData(param.PlaceData, placeImage);
                _placeImageViews.Add(view);
            }
        }

        protected override void OnAfterOpen(IUIOpenParam openParam)
        {
            OnScrollbarValueChanged(_scrollbar.value);
        }

        protected override void OnAfterClose(bool forceClose)
        {
            _placeImageViews.ForEach(view => Destroy(view.gameObject));
            _placeImageViews.Clear();
        }
    }


    public class PlaceInfoPopupOpenParam : IUIOpenParam
    {
        public PlaceInfoPopupOpenParam(PlaceData placeData)
        {
            PlaceData = placeData;
        }

        public PlaceData PlaceData { get; }
    }
}