using TMPro;
using UnityEngine;

namespace _CardPacks.Scripts
{
    public class CardPackPrize : MonoBehaviour
    {
        private static readonly int PreviewHash = Animator.StringToHash("preview");
        private static readonly int DisappearHash = Animator.StringToHash("disappear");
        
        
        [SerializeField] private Animator animator;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text valueField;


        private int m_Value;
        
        
        public void Preview()
        {
            animator.SetTrigger(PreviewHash);
        }

        public void Disappear()
        {
            animator.SetTrigger(DisappearHash);
        }

        public void SetValue(int value)
        {
            m_Value = value;
            valueField.text = $"+{value}";
        }

        public void SetOrder(int value)
        {
            canvas.sortingOrder = value;
        }
    }
}