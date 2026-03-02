using System.Collections.Generic;
using Scripts.Core.Systems.UI.Popups;
using Scripts.Data.Place;
using UnityEngine;

namespace Scripts.UI.Popups.ArchitectureList
{
    public class ArchitectureListPopup : BasePopup
    {
        [SerializeField] private PlaceDataContainer _placeDataContainer;
        [SerializeField] private RectTransform _architectureListContainer;
        [SerializeField] private PlaceView _placeViewPrefab;

        private readonly List<PlaceView> _views = new List<PlaceView>();

        
        public override string Id => PopupId.ArchitectureList;


        protected override void OnInitialization()
        {
            var places = _placeDataContainer.Data;
            foreach (var placeData in places)
            {
                var view = Instantiate(_placeViewPrefab, _architectureListContainer);
                view.SetData(placeData);
                _views.Add(view);
            }
        }
    }
}