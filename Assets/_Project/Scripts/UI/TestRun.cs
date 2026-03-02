using System;
using Scripts.Core.Systems.UI;
using Scripts.UI.Pages;
using UnityEngine;

namespace Scripts.UI
{
    public enum PageType
    {
        None,
        MainPage,
        ArchitectureMenu,
        NewPanorama,
        OldPanorama,
        Heraldry,
    }

    [DefaultExecutionOrder(1)]
    public class TestRun : MonoBehaviour
    {
        [SerializeField] private PageType _pageType = PageType.ArchitectureMenu;


        public void Start()
        {
            switch (_pageType)
            {
                case PageType.ArchitectureMenu:
                    UIProvider.OpenPage(PageId.ArchitectureMenu);
                    break;
                case PageType.OldPanorama:
                    UIProvider.OpenPage(PageId.OldPanorama);
                    break;
                case PageType.NewPanorama:
                    UIProvider.OpenPage(PageId.NewPanorama);
                    break;
                case PageType.None:
                    break;
                case PageType.MainPage:
                    UIProvider.OpenPage(PageId.MainPage);
                    break;
                case PageType.Heraldry:
                    UIProvider.OpenPage(PageId.Heraldry);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}