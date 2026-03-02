namespace Scripts.Core.Systems.UI
{
    public interface IUISystem
    {
        void OpenPage(string id, IUIOpenParam openParam = null);
        void OpenPopup(string id, IUIOpenParam openParam = null);
        void CloseTopPopup(bool forceClose = false);
        void CloseAllPopups(bool forceClose = false);
    }
}