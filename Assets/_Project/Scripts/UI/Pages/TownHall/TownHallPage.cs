using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.TownHall
{
    public class TownHallPage : BasePage
    {
        [SerializeField] private Button _backButton;
        
        
        public override string Id => PageId.TownHall;
        
        
        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
        }

        private void OnBackButtonDown() => UIProvider.OpenPage(PageId.MainPage);
    }
}