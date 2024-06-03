using _DatabaseAPI.Scripts;
using _QuestSystem.Scripts;
using _TimeAPI.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _UserOperations.Scripts
{
    public class UserRegisterer : MonoBehaviour
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int UserIdLength = 16;


        [SerializeField] private QuestDataContainerSO questDataContainer;
        [SerializeField] private CreateAccountUI createAccountUI;
        

        private void Start()
        {
            if (IsRegistered())
            {
                LocalUser.LoadUserData();
                LocalUser.LoadCardDatas();

                return;
            }
            
            var userID = GenerateRandomUserID();
            
            createAccountUI.StartCreatingAccount(accountInfo =>
            {
                RegisterUserWithUserID(userID, accountInfo);
            });
        }

        
        private void RegisterUserWithUserID(string userID, CreateAccountUI.AccountInfo accountInfo)
        {
            LocalUser.SetUserID(userID);

            var userData = UserData.Default;
            userData.name = accountInfo.name;
            userData.quests = new QuestDatabaseData[questDataContainer.GetQuestCount()];

            TimeAPI.GetCurrentTime(time =>
            {
                var dailyGenerations = userData.dailyGenerations;
                for (var i = 0; i < dailyGenerations.Length; i++)
                {
                    var dailyGeneration = dailyGenerations[i];
                    if (dailyGeneration.startTime.Equals(TimeData.Zero))
                    {
                        dailyGeneration.startTime = time;
                        dailyGenerations[i] = dailyGeneration;
                    }
                }
                
                LocalUser.SetUserData(userData, () =>
                {
                    Debug.Log($"User successfully registered with user id : {userID}");
                    LocalUser.LoadUserData();
                    LocalUser.LoadCardDatas();
                });
            });
        }
        

        public static bool IsRegistered()
        {
            return LocalUser.GetUserID() != UserAPI.NullUserID;
        }


        public static string GenerateRandomUserID()
        {
            var userID = "";

            for (var i = 0; i < UserIdLength; i++)
            {
                var randomIndex = Random.Range(0, Chars.Length);
                var randomChar = Chars[randomIndex];
                userID += randomChar;
            }

            return userID;
        }
    }
}