using System;
using System.Collections.Generic;

namespace Scripts.Core.Systems.UI.Popups
{
    public class PopupRouter : IPopupRouter
    {
        private readonly Dictionary<string, BasePopup> _popups;
        private readonly Stack<BasePopup> _popupStack;


        public PopupRouter(IEnumerable<BasePopup> popups)
        {
            _popups = new Dictionary<string, BasePopup>();
            _popupStack = new Stack<BasePopup>();
            foreach (var popup in popups)
            {
                _popups.Add(popup.Id, popup);
                popup.CloseRequested += OnCloseRequested;
                popup.Initialization();
            }
        }


        public void OpenPopup(string id, IUIOpenParam openParam = null)
        {
            if (_popups.TryGetValue(id, out var popup) == false)
                throw new Exception($"Popup with id {id} not found");

            _popupStack.Push(popup);
            popup.Open(openParam);
            popup.transform.SetAsLastSibling();
        }

        public void CloseTopPopup(bool forceClose = false)
        {
            if (_popupStack.Count < 1)
                return;

            var popup = _popupStack.Pop();
            popup.Close(forceClose);
        }

        public void CloseAllPopups(bool forceClose = false)
        {
            while (_popupStack.Count > 0)
                CloseTopPopup(forceClose);
        }

        private void OnCloseRequested()
        {
            CloseTopPopup();
        }
    }
}