using TMPro;
using UnityEngine;

namespace UI
{
    public class LogMessageUI : MonoBehaviour
    {
        private static readonly int PopUpHash = Animator.StringToHash("popup");
        
        
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Animator animator;


        public void SetText(string value)
        {
            textField.text = value;
        }

        public void PopUp()
        {
            animator.SetTrigger(PopUpHash);
        }
    }
}