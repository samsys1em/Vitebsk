namespace Scripts.Core.Systems.UI.Popups
{
    public interface IPopupRouter
    {
        void OpenPopup(string id, IUIOpenParam openParam = null);
        void CloseTopPopup(bool forceClose = false);
        void CloseAllPopups(bool forceClose = false);
    }
}