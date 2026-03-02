using Scripts.Core.Data;
using Scripts.Core.Systems.UI.Popups;
using UnityEngine;

namespace Scripts.Core.Systems.UI.Data
{
    [CreateAssetMenu(fileName = "Popups Container", menuName = "Data/UI/Popups Container")]
    public class PopupsContainer : DataContainer<BasePopup>
    {
    }
}