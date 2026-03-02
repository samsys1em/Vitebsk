using UnityEngine;

namespace Scripts.Data.Watch
{
    [CreateAssetMenu(fileName = "Watch Data", menuName = "Data/Watch/Data")]
    public class WatchData : ScriptableObject
    {
        [SerializeField] private string _year;
        [SerializeField] private Sprite _sprite;

        public string Year => _year;
        public Sprite Sprite => _sprite;
    }
}