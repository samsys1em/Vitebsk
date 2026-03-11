using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Popups;
using Scripts.Data.Place;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popups.PlaceInfo
{
    public class PlaceInfoPopup : BasePopup
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private ScrollRect _descriptionScrollRect;

        [SerializeField] private Button _buttonPrevious;
        [SerializeField] private Button _buttonNext;

        [SerializeField] private PlaceImageView _placeImageViewPrefab;
        [SerializeField] private RectTransform _placeImageViewContainer;
        [SerializeField] private Scrollbar _scrollbar;

        [SerializeField] private int _maxImages = 4;
        [SerializeField] private ScrollRect _scrollRect;

        private readonly List<PlaceImageView> _placeImageViews = new List<PlaceImageView>();

        private TweenerCore<float, float, FloatOptions> _scrollTween;

        private CanvasGroup _nextCg;
        private CanvasGroup _prevCg;

        private bool _suppressScrollbarCallback;

        private Coroutine _refreshCoroutine;
        private Coroutine _resetCoroutine;
        private Coroutine _descriptionResetCoroutine;

        public override string Id => PopupId.PlaceInfo;

        protected override void OnInitialization()
        {
            _buttonNext.onClick.AddListener(OnNextButtonDown);
            _buttonPrevious.onClick.AddListener(OnPreviousButtonDown);
            _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

            _nextCg = GetOrAddCanvasGroup(_buttonNext);
            _prevCg = GetOrAddCanvasGroup(_buttonPrevious);
        }

        private void ResetScrollToStartAfterLayout()
        {
            if (_resetCoroutine != null) StopCoroutine(_resetCoroutine);
            _resetCoroutine = StartCoroutine(CoResetScrollToStartAfterLayout());
        }

        private IEnumerator CoResetScrollToStartAfterLayout()
        {
            yield return null;
            yield return new WaitForEndOfFrame();

            Canvas.ForceUpdateCanvases();

            _scrollTween?.Kill();
            _scrollTween = null;

            _suppressScrollbarCallback = true;

            if (_scrollRect != null)
            {
                _scrollRect.StopMovement();
                _scrollRect.horizontalNormalizedPosition = 0f;
            }

            if (_scrollbar != null)
                _scrollbar.SetValueWithoutNotify(0f);

            _suppressScrollbarCallback = false;

            UpdateNavButtons(_scrollbar.value);

            _resetCoroutine = null;
        }

        private void ResetDescriptionScroll()
        {
            if (_descriptionResetCoroutine != null)
                StopCoroutine(_descriptionResetCoroutine);

            _descriptionResetCoroutine = StartCoroutine(CoResetDescriptionScroll());
        }

        private IEnumerator CoResetDescriptionScroll()
        {
            yield return null;
            yield return new WaitForEndOfFrame();

            Canvas.ForceUpdateCanvases();

            if (_descriptionScrollRect != null)
            {
                _descriptionScrollRect.StopMovement();
                _descriptionScrollRect.verticalNormalizedPosition = 1f;
            }

            _descriptionResetCoroutine = null;
        }

        private static CanvasGroup GetOrAddCanvasGroup(Button b)
        {
            var cg = b.GetComponent<CanvasGroup>();
            if (cg == null) cg = b.gameObject.AddComponent<CanvasGroup>();
            return cg;
        }

        private void OnScrollbarValueChanged(float value)
        {
            if (_suppressScrollbarCallback)
                return;

            UpdateNavButtons(value);
        }

        private void UpdateNavButtons(float value)
        {
            if (_placeImageViews.Count <= _maxImages)
            {
                SetButtonState(_buttonPrevious, _prevCg, false);
                SetButtonState(_buttonNext, _nextCg, false);
                return;
            }

            SetButtonState(_buttonPrevious, _prevCg, value > 0f);
            SetButtonState(_buttonNext, _nextCg, value < 1f);
        }

        private static void SetButtonState(Button button, CanvasGroup cg, bool visible)
        {
            button.interactable = visible;
            cg.alpha = visible ? 1f : 0f;
            cg.blocksRaycasts = visible;
            cg.interactable = visible;
        }

        private void OnNextButtonDown()
        {
            var targetValue = _scrollbar.value + 0.2f;

            if (targetValue > 0.9f)
                targetValue = 1f;

            _scrollTween?.Kill();

            _scrollTween = DOTween.To(
                () => _scrollbar.value,
                x => _scrollbar.value = x,
                targetValue,
                0.1f
            );
        }

        private void OnPreviousButtonDown()
        {
            var targetValue = _scrollbar.value - 0.2f;

            if (targetValue < 0.1f)
                targetValue = 0f;

            _scrollTween?.Kill();

            _scrollTween = DOTween.To(
                () => _scrollbar.value,
                x => _scrollbar.value = x,
                targetValue,
                0.1f
            );
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            var param = openParam as PlaceInfoPopupOpenParam;

            if (param == null)
                return;

            ResetUiState();

            _nameText.SetText(param.PlaceData.Name);
            _descriptionText.SetText(param.PlaceData.Description);

            ResetDescriptionScroll();

            foreach (var placeImage in param.PlaceData.Images)
            {
                var view = Instantiate(_placeImageViewPrefab, _placeImageViewContainer);
                view.SetData(param.PlaceData, placeImage);
                _placeImageViews.Add(view);
            }

            ResetScrollToStartAfterLayout();
        }

        private void ResetUiState()
        {
            _refreshCoroutine = StopCoroutineSafe(_refreshCoroutine);

            _scrollTween?.Kill();
            _scrollTween = null;

            foreach (var t in _placeImageViews.Where(t => t != null))
                Destroy(t.gameObject);

            _placeImageViews.Clear();

            _suppressScrollbarCallback = true;
            _scrollbar.SetValueWithoutNotify(0f);
            _suppressScrollbarCallback = false;

            SetButtonState(_buttonPrevious, _prevCg, false);
            SetButtonState(_buttonNext, _nextCg, false);
        }

        private IEnumerator RefreshAfterLayout()
        {
            yield return new WaitForEndOfFrame();

            UpdateNavButtons(_scrollbar.value);

            _refreshCoroutine = null;
        }

        private static Coroutine StopCoroutineSafe(Coroutine coroutine) => coroutine;

        protected override void OnAfterClose(bool forceClose)
        {
            _refreshCoroutine = StopCoroutineSafe(_refreshCoroutine);

            _scrollTween?.Kill();
            _scrollTween = null;

            for (int i = 0; i < _placeImageViews.Count; i++)
            {
                if (_placeImageViews[i] != null)
                    Destroy(_placeImageViews[i].gameObject);
            }

            _placeImageViews.Clear();

            _suppressScrollbarCallback = true;
            _scrollbar.SetValueWithoutNotify(0f);
            _suppressScrollbarCallback = false;

            SetButtonState(_buttonPrevious, _prevCg, false);
            SetButtonState(_buttonNext, _nextCg, false);
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