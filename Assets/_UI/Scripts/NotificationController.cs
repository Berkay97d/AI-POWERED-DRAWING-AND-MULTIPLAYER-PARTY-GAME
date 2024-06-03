using System;
using System.Collections;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using General;
using UnityEngine;

namespace UI
{
    public class NotificationController : MonoBehaviour
    {
        [SerializeField] private NotificationImage achivementNotificationImage;
        [SerializeField] private NotificationImage dailyGenerationNotificationImage;
        [SerializeField] private NotificationImage bookNotificationImage;
        [SerializeField] private NotificationImage marketNotificationImage;

        [SerializeField] private DailyCardGenerationController dailyCardGenerationController;
        [SerializeField] private QuestPanelUI questPanelUI;
        


        private void Start()
        {
            LazyCoroutines.WaitForSeconds(3f, () =>
            {
                Control(ControlDailyGeneration(), dailyGenerationNotificationImage);
            });
            
            questPanelUI.OnNotificationCountChanged += OnQuestPanelNotificationCountChanged;
        }

        private void OnDestroy()
        {
            questPanelUI.OnNotificationCountChanged -= OnQuestPanelNotificationCountChanged;
        }

        private void OnQuestPanelNotificationCountChanged(int obj)
        {
            achivementNotificationImage.SetNotificationNumber(obj);
        }

        private void Control(int notificationNumber, NotificationImage notificationImage)
        {
            notificationImage.SetNotificationNumber(notificationNumber);
        }
        
        private int ControlDailyGeneration()
        {
            var readyGenerationNumber = 0;
            
            foreach (var generationArea in dailyCardGenerationController.GetGenerationAreas())
            {
                if (generationArea.GetIsReadyToGenerate())
                {
                    readyGenerationNumber++;
                }
            }
            

            return readyGenerationNumber;
        }

        

       
    }
}