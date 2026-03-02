using Scripts.Core.Systems.UI.Pages;
using Scripts.Core.Systems.UI.Popups;

namespace Scripts.Core.Systems.UI
{
    public class UISystem : IUISystem
    {
        private readonly IPageRouter _pageRouter;
        private readonly IPopupRouter _popupRouter;


        public UISystem(IPageRouter pageRouter, IPopupRouter popupRouter)
        {
            _pageRouter = pageRouter;
            _popupRouter = popupRouter;
        }


        public void OpenPage(string id, IUIOpenParam openParam = null)
        {
            CloseAllPopups();
            _pageRouter.Open(id, openParam);
        }

        public void OpenPopup(string id, IUIOpenParam openParam = null)
        {
            _popupRouter.OpenPopup(id, openParam);
        }

        public void CloseTopPopup(bool forceClose = false)
        {
            _popupRouter.CloseTopPopup();
        }

        public void CloseAllPopups(bool forceClose = false)
        {
            _popupRouter.CloseAllPopups();
        }
    }
}