namespace Scripts.Core.Systems.UI.Pages
{
    public interface IPageRouter
    {
       void Open(string id, IUIOpenParam openParam = null);
    }
}