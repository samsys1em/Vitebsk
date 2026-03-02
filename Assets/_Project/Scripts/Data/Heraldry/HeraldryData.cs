using UnityEngine;

namespace Scripts.Data.Heraldry
{
    [CreateAssetMenu(fileName = "Heraldry Data", menuName = "Data/Heraldry/Data")]
    public class HeraldryData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _year;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Sprite _image;


        public string Name => _name;
        public int Year => _year;
        public string Description => _description;
        public Sprite Image => _image;
    }
}