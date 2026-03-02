using Scripts.Data.Heraldry;
using Scripts.UI.Pages.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Pages.Heraldry
{
    [RequireComponent(typeof(Button))]
    public class HeraldryButton : DateButton<HeraldryData>
    {
        protected override void OnAwake()
        {
            YearText.SetText($"{Data.Year}");
        }
    }
}