using Scripts.Pool;
using UnityEngine;

namespace Scripts.UI.ListView
{
    public abstract class ListItemView<TItem> : MonoBehaviour, IPoolable
    {
        public void Recycle() => OnRecycle();

        public void Release() => OnRelease();

        public void SetItem(TItem item) => OnSetItem(item);

        protected virtual void OnRecycle() { }
        
        protected virtual void OnRelease() { }
        
        protected virtual void OnSetItem(TItem item) { }
    }
}