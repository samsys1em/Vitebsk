using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data.Place
{
    [CreateAssetMenu(fileName = "Place Data", menuName = "Data/Place/Data")]
    public class PlaceData : ScriptableObject
    {
        [SerializeField] private string _number;
        [SerializeField, TextArea] private string _name;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Sprite _preview;
        [SerializeField] private List<PlaceImage> _images;
        [SerializeField] private bool _isLight;


        public string Number => _number;
        public string Name => _name;
        public string Description => _description;
        public Sprite Preview => _preview;
        public bool IsLight => _isLight;
        public List<PlaceImage> Images => _images;

        [Header("Editor")] [SerializeField] private List<Sprite> _sprites;


        [ContextMenu("Clear Sprites")]
        private void ClearSprites()
        {
            _sprites.Clear();
        }
        
        [ContextMenu("Add Sprites")]
        private void SetSprites()
        {
            _images = new List<PlaceImage>();
            foreach (var sprite in _sprites)
            {
                var title = sprite.name.Substring((_images.Count + 1) / 10 + 1);
                title = title.TrimStart('.');
                title = title.Trim();
                var placeImage = new PlaceImage(sprite, title);
                _images.Add(placeImage);
            }
        }
    }
}