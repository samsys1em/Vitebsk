using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Systems.UI.Pages
{
    public class PageRouter : IPageRouter
    {
        private readonly Dictionary<string, BasePage> _pages;
        private BasePage _currentPage;

        public PageRouter(IEnumerable<BasePage> pages)
        {
            _pages = new Dictionary<string, BasePage>();

            foreach (var page in pages)
            {
                _pages.Add(page.Id, page);
                page.Initialization();
            }
        }

        public void Open(string id, IUIOpenParam openParam = null)
        {
            if (!_pages.TryGetValue(id, out var page))
            {
                Debug.LogError($"No page found for {id}");
                return;
            }

            if (_currentPage == page)
                return;

            _currentPage?.Close();
            _currentPage = page;
            _currentPage.Open(openParam);
        }
    }
}