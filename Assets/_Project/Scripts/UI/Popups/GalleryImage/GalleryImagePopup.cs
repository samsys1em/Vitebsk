using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Popups;
using Scripts.Data.Gallery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popups.GalleryImage
{
    public class GalleryImagePopup : BasePopup
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;


        public override string Id => PopupId.GalleryImage;


        protected override void OnBeforeOpen(IUIOpenParam openParam)
        {
            var param = openParam as GalleryImagePopupOpenParam;
            if (param == null)
                return;

            _image.sprite = param.GalleryData.Sprite;
            _text.SetText(param.GalleryData.Name);
        }
    }

    public class GalleryImagePopupOpenParam : IUIOpenParam
    {
        public GalleryImagePopupOpenParam(GalleryData galleryData)
        {
            GalleryData = galleryData;
        }

        public GalleryData GalleryData { get; }
    }
}