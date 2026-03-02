using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Data
{
    public abstract class DataContainer<TType> : ScriptableObject
    {
        [SerializeField] private List<TType> _data = new List<TType>();

        
        public List<TType> Data => _data;
    }
}