using System.Collections.Generic;
using Scripts.Core.Systems.UI;
using Scripts.Core.Systems.UI.Pages;
using Scripts.Data.Gallery;
using Scripts.UI.Carousel;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages
{
    public class GalleryPage : BasePage
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private List<DynamicScrollView> _dynamicScrollViews;
        [SerializeField] private GalleryDataContainer _galleryDataContainer;


        public override string Id => PageId.Gallery;


        protected override void OnInitialization()
        {
            _backButton.onClick.AddListener(OnBackButtonDown);
            
            var data = _galleryDataContainer != null ? _galleryDataContainer.Data : null;
            if (data == null || _dynamicScrollViews == null || _dynamicScrollViews.Count == 0)
                return;

            var buckets = DistributeSequentiallyEvenly(data, _dynamicScrollViews.Count);
            for (var i = 0; i < _dynamicScrollViews.Count; i++)
            {
                var view = _dynamicScrollViews[i];
                view.Init(buckets[i]);
            }
        }

        private static List<List<GalleryData>> DistributeSequentiallyEvenly(IList<GalleryData> source, int bucketCount)
        {
            var result = new List<List<GalleryData>>(bucketCount);
            for (var i = 0; i < bucketCount; i++)
                result.Add(new List<GalleryData>());

            if (source == null || source.Count == 0 || bucketCount <= 0)
                return result;

            var total = source.Count;
            var baseSize = total / bucketCount;
            var remainder = total % bucketCount; // первые remainder корзин получат +1

            var idx = 0;
            for (var b = 0; b < bucketCount; b++)
            {
                var take = baseSize + (b < remainder ? 1 : 0);
                for (var j = 0; j < take && idx < total; j++, idx++)
                    result[b].Add(source[idx]);
            }

            return result;
        }
        
        private void OnBackButtonDown() => UIProvider.OpenPage(PageId.MainPage);
    }
}