using System.Linq;
using Scripts.Core.Data;
using Scripts.Core.Systems.UI.Pages;
using Scripts.Core.Systems.UI.Popups;
using Scripts.Data;
using UnityEngine;

namespace Scripts.Core.Systems.UI
{
    public class UIProvider : MonoBehaviour
    {
        [SerializeField] private RectTransform _pageRoot;
        [SerializeField] private RectTransform _popupRoot;
        [SerializeField] private DataContainer<BasePage> _pageDataContainer;
        [SerializeField] private DataContainer<BasePopup> _popupDataContainer;

        private static UIProvider _instance;
        private IUISystem _uiSystem;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Initialize();
            }
            else
                Destroy(gameObject);
        }

        private void Initialize()
        {
            var pages = _pageDataContainer.Data.Select(page => Instantiate(page, _pageRoot));
            var popups = _popupDataContainer.Data.Select(popup => Instantiate(popup, _popupRoot));
            var pageRouter = new PageRouter(pages);
            var popupRouter = new PopupRouter(popups);
            _uiSystem = new UISystem(pageRouter, popupRouter);
        }

        public static void OpenPage(string id, IUIOpenParam openParam = null)
        {
            _instance._uiSystem.OpenPage(id, openParam);
        }


        public static void OpenPopup(string id, IUIOpenParam openParam = null)
        {
            _instance._uiSystem.OpenPopup(id, openParam);
        }

        public static void CloseTopPopup(bool forceClose = false)
        {
            _instance._uiSystem.CloseTopPopup(forceClose);
        }

        public static void CloseAllPopups(bool forceClose = false)
        {
            _instance._uiSystem.CloseAllPopups(forceClose);
        }
    }
}