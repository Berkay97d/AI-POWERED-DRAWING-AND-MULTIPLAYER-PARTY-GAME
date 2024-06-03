using NaughtyAttributes;
using UnityEngine;

namespace _CardPacks.Scripts
{
    public class Test_Card_Pack : MonoBehaviour
    {
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void OpenCommonCardPack()
        {
            CardPackOpener.OpenCardPack(CardPackType.Common);
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void OpenRareCardPack()
        {
            CardPackOpener.OpenCardPack(CardPackType.Rare);
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void OpenLegendaryCardPack()
        {
            CardPackOpener.OpenCardPack(CardPackType.Legendary);
        }
    }
}