// ListView.cs
using System.Collections.Generic;
using Scripts.Pool;
using UnityEngine;

namespace Scripts.UI.ListView
{
    public abstract class ListView<TItem> : MonoBehaviour
    {
        [SerializeField] private Transform _poolParent;
        [SerializeField] private Transform _content;
        [SerializeField] private ListItemView<TItem> _listItemViewPrefab;

        private readonly List<ListItemView<TItem>> _itemViews = new List<ListItemView<TItem>>();
        private readonly List<TItem> _items = new List<TItem>();
        private ObjectsPool<ListItemView<TItem>> _pool;


        public void Initialization()
        {
            _pool = new ObjectsPool<ListItemView<TItem>>(_content, _poolParent, _listItemViewPrefab);
        }

        public void AddItem(TItem item)
        {
            if (item == null)
                return;

            var itemView = _pool.Get(view => view.SetItem(item));
            _items.Add(item);
            _itemViews.Add(itemView);
        }

        public void AddItems(IEnumerable<TItem> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
                AddItem(item);
        }

        public void InsertAt(int index, TItem item)
        {
            if (item == null)
                return;

            var itemView = _pool.Get(view => view.SetItem(item));
            if (index < 0) index = 0;
            if (index > _items.Count) index = _items.Count;

            _items.Insert(index, item);
            _itemViews.Insert(index, itemView);
            itemView.transform.SetSiblingIndex(index);
        }

        public void RemoveItem(TItem item)
        {
            var itemIndex = _items.IndexOf(item);
            if (itemIndex < 0)
                return;

            _items.RemoveAt(itemIndex);
            _pool.Return(_itemViews[itemIndex]);
            _itemViews.RemoveAt(itemIndex);
        }

        public ListItemView<TItem> DetachItem(TItem item)
        {
            var index = _items.IndexOf(item);
            if (index < 0)
                return null;

            var view = _itemViews[index];
            _items.RemoveAt(index);
            _itemViews.RemoveAt(index);
            return view;
        }

        public void ReinsertExistingAt(int index, TItem item, ListItemView<TItem> view)
        {
            if (view == null)
                return;

            if (index < 0) index = 0;
            if (index > _items.Count) index = _items.Count;

            _items.Insert(index, item);
            _itemViews.Insert(index, view);
            view.transform.SetParent(_content, false);
            view.transform.SetSiblingIndex(index);
        }

        public void RemoveAll()
        {
            _items.Clear();
            _itemViews.Reverse();
            foreach (var itemView in _itemViews)
                _pool.Return(itemView);
            _itemViews.Clear();
        }
    }
}
