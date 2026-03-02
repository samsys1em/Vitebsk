using UnityEngine;

namespace Scripts.Data.People
{
    [CreateAssetMenu(fileName = "People Data", menuName = "Data/People/Data")]
    public class PeopleData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField, TextArea] private string _description;


        public string Name => _name;
        public Sprite Sprite => _sprite;
        public string Description => _description;
    }
}