using UnityEditor;
using UnityEngine;

namespace Scripts.Data.Gallery
{
    [CreateAssetMenu(fileName = "Gallery Data", menuName = "Data/Gallery/Data")]
    public class GalleryData : ScriptableObject
    {
        [SerializeField, TextArea] private string _name;
        [SerializeField] private Sprite _sprite;


        public string Name => _name;
        public Sprite Sprite => _sprite;
    }
}