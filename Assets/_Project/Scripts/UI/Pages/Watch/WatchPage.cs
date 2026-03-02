using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using Scripts.Data.Watch;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Watch
{
    public class WatchPage : BasePage
    {
        public override string Id => PageId.Watch;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private List<WatchButton> _watchButtons;
        [SerializeField] private Image _watchImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.25f;
        [SerializeField] private float _heraldryFadeDuration = 0.5f;
        [SerializeField] private List<Image> _imageList;


        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
            _watchButtons.ForEach(item => item.Down += OnWatchButtonDown);
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            _watchButtons.ForEach(item => item.SetSelect(false));
            _canvasGroup.alpha = 0;
            _imageList.ForEach(item => item.color = new Color(1 ,1, 1, 0));
            _imageList[0].color = new Color(1, 1, 1, 1);
            _watchButtons[0].ButtonDown();
        }

        private void OnWatchButtonDown(WatchData watchData)
        {
            if (_canvasGroup.alpha < 1)
            {
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(1, _fadeDuration);
            }
            
            _watchImage.DOKill();
            _watchImage.DOFade(0, 0);
            _watchImage.sprite = watchData.Sprite;
            _watchImage.DOFade(1, _heraldryFadeDuration);
        }

        private void OnBackButtonDown()
        {
            UIProvider.OpenPage(PageId.MainPage);
        }
    }
}