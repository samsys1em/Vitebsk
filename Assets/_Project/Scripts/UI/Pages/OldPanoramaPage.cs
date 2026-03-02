using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages
{
    public class OldPanoramaPage : BasePage
    {
        public override string Id => PageId.OldPanorama;
        
        [SerializeField] private Button _backButton;


        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
        }

        private void OnBackButtonDown()
        {
            UIProvider.OpenPage(PageId.ArchitectureMenu);
        }
    }
}