using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using Scripts.Data.Heraldry;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Heraldry
{
    public class HeraldryPage : BasePage
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private List<HeraldryButton> _heraldryButtons;
        [SerializeField] private TMP_Text _heraldryNameText;
        [SerializeField] private TMP_Text _heraldryDescriptionText;
        [SerializeField] private Image _heraldryImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _fadeDuration = 0.25f;
        [SerializeField] private float _textFadeDuration = 0.5f;
        [SerializeField] private float _heraldryFadeDuration = 0.5f;
        [SerializeField] private Image _fillImage;


        public override string Id => PageId.Heraldry;


        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
            _heraldryButtons.ForEach(item => item.Down += OnHeraldryButtonDown);
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            _heraldryButtons.ForEach(item => item.SetSelect(false));
            _canvasGroup.alpha = 0;
            _fillImage.fillAmount = 1;
        }

        private void OnHeraldryButtonDown(HeraldryData heraldryData)
        {
            if (_canvasGroup.alpha < 1)
            {
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(1, _fadeDuration);
            }
            
            _fillImage.DOKill();
            _heraldryImage.DOKill();
            _heraldryImage.DOFade(0, 0);
            _fillImage.fillAmount = 1;
            
            _scrollRect.verticalNormalizedPosition = 1;
            _heraldryNameText.SetText(heraldryData.Name);
            _heraldryDescriptionText.SetText(heraldryData.Description);
            _heraldryImage.sprite = heraldryData.Image;
           
            _fillImage.DOFillAmount(0, _textFadeDuration);
            _heraldryImage.DOFade(1, _heraldryFadeDuration);
        }

        private void OnBackButtonDown() => UIProvider.OpenPage(PageId.MainPage);
    }
}