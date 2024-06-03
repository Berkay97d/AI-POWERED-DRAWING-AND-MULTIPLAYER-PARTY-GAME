using _UserOperations.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _QuestSystem.Scripts
{
    public class Test_Quest_System : MonoBehaviour
    {
        [SerializeField] private QuestType questType;
        [SerializeField] private int count;


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void AddQuestProgress()
        {
            LocalUser.AddQuestProgress(questType, count);
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void CompleteAllQuests()
        {
            LocalUser.GetUserData(userData =>
            {
                var questDataContainer = QuestDataContainerSO.GetInstance();
                var quests = questDataContainer.GetQuests();

                for (var i = 0; i < userData.quests.Length; i++)
                {
                    var quest = userData.quests[i];

                    quest.count = quests[i].actionCount;
                    quest.isClaimed = false;
                    userData.quests[i] = quest;
                }
                
                LocalUser.SetUserData(userData);
            });
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void ResetQuests()
        {
            LocalUser.GetUserData(userData =>
            {
                var questDataContainer = QuestDataContainerSO.GetInstance();
                userData.quests = new QuestDatabaseData[questDataContainer.GetQuestCount()];
                LocalUser.SetUserData(userData);
            });
        }
    }
}