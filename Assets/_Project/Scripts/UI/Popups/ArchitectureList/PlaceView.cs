using Scripts.Core.Systems.UI;
using Scripts.Data.Place;
using Scripts.UI.Popups.PlaceInfo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popups.ArchitectureList
{
    public class PlaceView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _previewImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Color _lightColor;
        [SerializeField] private Color _darkColor;
        private PlaceData _placeData;


        private void Awake()
        {
            _button.onClick.AddListener(OnButtonDown);
        }

        private void OnButtonDown()
        {
            var openParam = new PlaceInfoPopupOpenParam(_placeData);
            UIProvider.OpenPopup(PopupId.PlaceInfo, openParam);
        }

        public void SetData(PlaceData placeData)
        {
            _placeData = placeData;
            var nameText = $"{_placeData.Number}. {_placeData.Name}";
            _nameText.SetText(nameText);
            _previewImage.sprite = _placeData.Preview;
            _nameText.color = _placeData.IsLight ? _lightColor : _darkColor;
        }
    }
}