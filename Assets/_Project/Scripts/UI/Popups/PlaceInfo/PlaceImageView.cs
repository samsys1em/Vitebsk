using Scripts.Core.Systems.UI;
using Scripts.Data.Place;
using Scripts.UI.Popups.ImagePopup;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popups.PlaceInfo
{
    public class PlaceImageView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private PlaceData _placeData;
        private PlaceImage _placeImage;


        private void Awake()
        {
            _button.onClick.AddListener(OnButtonDown);
        }

        public void SetData(PlaceData placeData, PlaceImage placeImage)
        {
            _placeData = placeData;
            _placeImage = placeImage;
            _image.sprite = _placeImage.Image;
        }

        private void OnButtonDown()
        {
            var openParam = new ImagePopupOpenParam(_placeData, _placeImage);
            UIProvider.OpenPopup(PopupId.Image, openParam);
        }
    }
}