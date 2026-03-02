using System;
using UnityEngine;

namespace Scripts.Data.Place
{
    [Serializable]
    public class PlaceImage
    {
        [SerializeField] private string _title;
        [SerializeField] private Sprite _image;

        public PlaceImage(Sprite sprite, string spriteName)
        {
            _title = spriteName;
            _image = sprite;
        }


        public string Title => _title;
        public Sprite Image => _image;
    }
}