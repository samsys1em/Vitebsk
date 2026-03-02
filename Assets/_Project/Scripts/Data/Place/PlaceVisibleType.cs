using UnityEngine;

namespace Scripts.Data.Place
{
    [CreateAssetMenu(fileName = "Place Visible Type", menuName = "Data/Place/Visible Type")]
    public class PlaceVisibleType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Color _color;


        public string Name => _name;
        public Color Color => _color;
    }
}