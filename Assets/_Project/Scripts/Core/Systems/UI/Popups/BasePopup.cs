using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.Systems.UI.Popups
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BasePopup : MonoBehaviour
    {
        public event Action CloseRequested;

        [SerializeField] private Button _backField;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _content;


        private CanvasGroup _canvasGroup;
        private Sequence _openSequence;
        private Sequence _closeSequence;


        public abstract string Id { get; }


        public void Initialization()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _backButton?.onClick.AddListener(BackRequest);
            _backField?.onClick.AddListener(BackRequest);
            OnInitialization();
            _content.SetActive(false);
        }

        public void Open(IUIOpenParam openParam = null)
        {
            _openSequence?.Kill();
            _closeSequence?.Kill();

            _canvasGroup.alpha = 0;
            _openSequence = DOTween.Sequence();
            _openSequence
                .AppendCallback(() => OnBeforeOpen(openParam))
                .AppendCallback(() => _content.SetActive(true))
                .Append(_canvasGroup.DOFade(1, .25f))
                .AppendCallback(() => OnAfterOpen(openParam))
                ;

            _openSequence.Play();
        }

        public void Close(bool forceClose = false)
        {
            if (forceClose)
            {
                OnBeforeClose(true);
                _content.SetActive(false);
                OnAfterClose(true);
            }
            else
            {
                _closeSequence = DOTween.Sequence();
                _closeSequence
                    .AppendCallback(() => OnBeforeClose(false))
                    .Append(_canvasGroup.DOFade(0, .25f))
                    .AppendCallback(() => _content.SetActive(false))
                    .AppendCallback(() => OnAfterClose(false))
                    ;

                _closeSequence.Play();
            }
        }

        private void BackRequest()
        {
            CloseRequested?.Invoke();
        }

        protected virtual void OnInitialization()
        {
        }

        protected virtual void OnBeforeOpen(IUIOpenParam openParam)
        {
        }

        protected virtual void OnAfterOpen(IUIOpenParam openParam)
        {
        }

        protected virtual void OnBeforeClose(bool forceClose)
        {
        }

        protected virtual void OnAfterClose(bool forceClose)
        {
        }
    }
}