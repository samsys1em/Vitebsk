using System.Collections.Generic;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Maps
{
    public class MapsPage : BasePage
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private List<GameObject> _plans;
        [SerializeField] private Toggle _mapMarkToggle;

        private IEnumerable<PlaceDot> _placeDots;
        private int _currentPlanIndex;


        public override string Id => PageId.Maps;


        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
            _placeDots = GetComponentsInChildren<PlaceDot>(true);
            _mapMarkToggle.onValueChanged.AddListener(OnMapMarkToggled);
            _nextButton.onClick.AddListener(NextPlan);
            _previousButton.onClick.AddListener(PreviousPlan);
        }

        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            _mapMarkToggle.isOn = true;
            _currentPlanIndex = 0;
            _previousButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(true);
            
            _plans.ForEach(plan => plan.SetActive(false));
            _plans[0].SetActive(true);
        }

        private void NextPlan()
        {
            if (_currentPlanIndex >= _plans.Count - 1)
                return;

            _plans[_currentPlanIndex].SetActive(false);
            _currentPlanIndex++;
            _plans[_currentPlanIndex].SetActive(true);
            if (_currentPlanIndex >= _plans.Count - 1)
                _nextButton.gameObject.SetActive(false);

            if (_previousButton.gameObject.activeSelf == false)
                _previousButton.gameObject.SetActive(true);
        }

        private void PreviousPlan()
        {
            if (_currentPlanIndex <= 0)
                return;

            _plans[_currentPlanIndex].SetActive(false);
            _currentPlanIndex--;
            _plans[_currentPlanIndex].SetActive(true);
            
            if (_currentPlanIndex <= 0)
                _previousButton.gameObject.SetActive(false);

            if (_nextButton.gameObject.activeSelf == false)
                _nextButton.gameObject.SetActive(true);
        }

        private void OnMapMarkToggled(bool value)
        {
            SetEnableMapMark(value);
        }

        private void SetEnableMapMark(bool value)
        {
            foreach (var placeDot in _placeDots)
                placeDot.gameObject.SetActive(value);
        }
        
        private void OnBackButtonDown() => UIProvider.OpenPage(PageId.MainPage);
    }
}