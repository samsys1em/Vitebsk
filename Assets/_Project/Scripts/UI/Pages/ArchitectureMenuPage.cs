using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using Scripts.UI.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages
{
    public class ArchitectureMenuPage : BasePage
    {
        [SerializeField] private Button _buttonArchitecture;
        [SerializeField] private Button _buttonOldPanorama;
        [SerializeField] private Button _buttonNewPanorama;
        [SerializeField] private Button _buttonPrevious;


        public override string Id => PageId.ArchitectureMenu;


        private void Awake()
        {
            _buttonArchitecture.onClick.AddListener(OnButtonArchitectureDown);
            _buttonOldPanorama.onClick.AddListener(OnButtonOldPanoramaDown);
            _buttonNewPanorama.onClick.AddListener(OnButtonNewPanoramaDown);
            _buttonPrevious.onClick.AddListener(OnButtonPreviousDown);
        }

        private void OnButtonArchitectureDown()
        {
            UIProvider.OpenPopup(PopupId.ArchitectureList);
        }

        private void OnButtonOldPanoramaDown()
        {
            UIProvider.OpenPage(PageId.OldPanorama);
        }

        private void OnButtonNewPanoramaDown()
        {
            UIProvider.OpenPage(PageId.NewPanorama);
        }

        private void OnButtonPreviousDown()
        {
            UIProvider.OpenPage(PageId.MainPage);
        }
    }
}