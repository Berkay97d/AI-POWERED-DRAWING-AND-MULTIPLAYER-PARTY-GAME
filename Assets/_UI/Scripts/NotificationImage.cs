using TMPro;
using UnityEngine;

namespace UI
{
    public class NotificationImage : MonoBehaviour
    {
        [SerializeField] private TMP_Text notificationNumberText;
        [SerializeField] private Transform mainTransform;
        
        
        private int activeNotificationNumber;

        
        private void UpdateNotificationImage()
        {
            notificationNumberText.text = activeNotificationNumber.ToString();

            if (activeNotificationNumber == 0)
            {
                HideNotificationImage();
                return;
            }
            
            ShowNotificationImage();
        }

        private void ShowNotificationImage()
        {
            mainTransform.gameObject.SetActive(true);
        }

        private void HideNotificationImage()
        {
            mainTransform.gameObject.SetActive(false);
        }

        public void SetNotificationNumber(int number)
        {
            activeNotificationNumber = number;
            
            UpdateNotificationImage();
        }
        
       
        
        
    }
}