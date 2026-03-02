using Scripts.Core.Systems.UI;
using Scripts.Data.Gallery;
using Scripts.UI.Popups;
using Scripts.UI.Popups.GalleryImage;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Carousel
{
    [RequireComponent(typeof(SpriteAspectRatioBinder))]
    public class CarouselItem : MonoBehaviour
    {
        [SerializeField] private SpriteAspectRatioBinder _aspectRatioBinder;
        [SerializeField] private Button _button;

        private GalleryData _galleryData;


        private void Awake()
        {
            _button.onClick.AddListener(OnButtonDown);
        }

        private void OnButtonDown()
        {
           UIProvider.OpenPopup(PopupId.GalleryImage, new GalleryImagePopupOpenParam(_galleryData));
        }

        public void SetSprite(GalleryData galleryData)
        {
            _galleryData = galleryData;
            _aspectRatioBinder.SetSprite(_galleryData.Sprite);
        }
    }
}