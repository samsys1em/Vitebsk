using System;
using Scripts.Core.Systems.UI;
using Scripts.Data.Place;
using Scripts.UI.Popups;
using Scripts.UI.Popups.PlaceInfo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class PlaceDot : MonoBehaviour
    {
        [SerializeField] private PlaceData _placeData;
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private PlaceVisibleType _placeVisibleType;

        private Button _button;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonDown);

            if (_placeData == null)
                Debug.LogError("Place Data is not set.");

            _numberText.SetText($"{_placeData.Number}");
            var placeName = _placeData.Name;
            if (_placeVisibleType.Name != string.Empty)
                placeName += $"{'\n'}{_placeVisibleType.Name}";

            _nameText.SetText(placeName);
            _button.image.color = _placeVisibleType.Color;
        }

        private void OnButtonDown()
        {
            var openParam = new PlaceInfoPopupOpenParam(_placeData);
            UIProvider.OpenPopup(PopupId.PlaceInfo, openParam);
        }

        [ContextMenu("Set info")]
        private void SetInfo()
        {
            _numberText.SetText($"{_placeData.Number}");
            var placeName = _placeData.Name;
            if (_placeVisibleType.Name != string.Empty)
                placeName += $"{'\n'}{_placeVisibleType.Name}";

            _nameText.SetText(placeName);
            if (_button == null)
                _button = GetComponent<Button>();

            _button.image.color = _placeVisibleType.Color;
        }
    }
}