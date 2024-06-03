using TMPro;
using UnityEngine;

namespace _CardPacks.Scripts
{
    public class CardPackPrizeUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text valueField;
        
        
        public void SetValue(int value)
        {
            valueField.text = $"+{value}";
        }
    }
}