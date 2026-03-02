using DG.Tweening;
using Scripts.Data.Watch;
using Scripts.UI.Pages.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Watch
{
    public class WatchButton : DateButton<WatchData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _fadeDuration = 1;


        protected override void OnAwake()
        {
            YearText.SetText($"{Data.Year}");
        }

        protected override void OnSelectChanged(bool value)
        {
            _image.DOKill();
            _image.DOFade(value ? 1 : 0, _fadeDuration);
        }
    }
}