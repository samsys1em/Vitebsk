using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages
{
    public class MainPage : BasePage
    {
        [SerializeField] private Button _peoplesButton;
        [SerializeField] private Button _townHallButton;
        [SerializeField] private Button _watchButton;
        [SerializeField] private Button _architectureButton;
        [SerializeField] private Button _cityPlansButton;
        [SerializeField] private Button _heraldryButton;
        [SerializeField] private Button _ruleButton;
        [SerializeField] private Button _galleryButton;


        public override string Id => PageId.MainPage;


        protected override void OnInitialization()
        {
            _peoplesButton.onClick.AddListener(OnPeoplesButtonDown);
            _townHallButton.onClick.AddListener(OnTownHallButtonDown);
            _watchButton.onClick.AddListener(OnWatchButtonDown);
            _architectureButton.onClick.AddListener(OnArchitectureButtonDown);
            _cityPlansButton.onClick.AddListener(OnCityPlansButtonDown);
            _heraldryButton.onClick.AddListener(OnHeraldryButtonDown);
            _ruleButton.onClick.AddListener(OnRuleButtonDown);
            _galleryButton.onClick.AddListener(OnGalleryButtonDown);
        }

        private static void OnPeoplesButtonDown() => UIProvider.OpenPage(PageId.People);
        private static void OnTownHallButtonDown() => UIProvider.OpenPage(PageId.TownHall);
        private static void OnWatchButtonDown() => UIProvider.OpenPage(PageId.Watch);
        private static void OnArchitectureButtonDown() => UIProvider.OpenPage(PageId.ArchitectureMenu);
        private static void OnCityPlansButtonDown() => UIProvider.OpenPage(PageId.Maps);
        private static void OnHeraldryButtonDown() => UIProvider.OpenPage(PageId.Heraldry);
        private static void OnRuleButtonDown() => UIProvider.OpenPage(PageId.Rule);
        private static void OnGalleryButtonDown() => UIProvider.OpenPage(PageId.Gallery);
    }
}