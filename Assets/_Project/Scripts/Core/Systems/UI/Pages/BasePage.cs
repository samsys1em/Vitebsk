using UnityEngine;

namespace Scripts.Core.Systems.UI.Pages
{
    public abstract class BasePage : MonoBehaviour
    {
        [SerializeField] private GameObject _content;


        protected GameObject Content => _content;


        public abstract string Id { get; }


        public void Initialization()
        {
            _content.SetActive(false);
            OnInitialization();
        }

        public void Open(IUIOpenParam openParam)
        {
            OnBeforeOpen(openParam);
            _content.SetActive(true);
            OnAfterOpen(openParam);
        }

        public void Close()
        {
            OnBeforeClose();
            _content.SetActive(false);
            OnAfterClose();
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

        protected virtual void OnBeforeClose()
        {
        }

        protected virtual void OnAfterClose()
        {
        }
    }
}