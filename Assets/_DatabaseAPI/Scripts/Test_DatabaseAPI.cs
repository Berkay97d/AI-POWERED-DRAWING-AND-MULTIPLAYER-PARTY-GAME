using _UserOperations.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _DatabaseAPI.Scripts
{
    public class Test_DatabaseAPI : MonoBehaviour
    {
        [Header("GetCardsByUserID")]
        [SerializeField] private string userID_GetCardsByUserID;
        
        [Header("PostCard")]
        [SerializeField] private string userID_PostCard;
        [SerializeField] private CardData cardData_PostCard;
        
        [Header("DeleteCard")]
        [SerializeField] private string userID_DeleteCard;
        [SerializeField] private string cardID_DeleteCard;
        
        [Header("UpdateCard")]
        [SerializeField] private string userID_UpdateCard;
        [SerializeField] private string cardID_UpdateCard;
        [SerializeField] private string newUserID_UpdateCard;
        [SerializeField] private CardData cardData_UpdateCard;

        [Header("GetUserByID")]
        [SerializeField] private string userID_GetUserByID;

        [Header("PostUser")]
        [SerializeField] private UserData userData_PostUser;

        [Header("GetImageByID")]
        [SerializeField] private string imageID_GetImageByID;

        [Header("PostImage")]
        [SerializeField] private string imageAsBase64_PostImage;


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void GetCardsByUserID()
        {
            DatabaseAPI.GetCardsByUserID(userID_GetCardsByUserID, PrintGetCardsByUserID);
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void PostCard()
        {
            DatabaseAPI.PostCard(userID_PostCard, cardData_PostCard, response => Debug.Log(response));
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void DeleteCard()
        {
            DatabaseAPI.DeleteCard(userID_DeleteCard, cardID_DeleteCard, () => Debug.Log("Card deleted successfully!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UpdateCard()
        {
            DatabaseAPI.UpdateCard(userID_UpdateCard, cardID_UpdateCard, newUserID_UpdateCard, cardData_UpdateCard, () => Debug.Log("Card updated successfully!"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void GetUserByID()
        {
            DatabaseAPI.GetUserByID(userID_GetUserByID, response => Debug.Log(response));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void PostUser()
        {
            DatabaseAPI.PostUser(UserRegisterer.GenerateRandomUserID(), userData_PostUser, response => Debug.Log($"User is added successfully!\n{response}"));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void GetImageByID()
        {
            DatabaseAPI.GetImageByID(imageID_GetImageByID, response => Debug.Log(response));
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void PostImage()
        {
            DatabaseAPI.PostImage(imageAsBase64_PostImage, response => Debug.Log(response));
        }
        
        
        private static void PrintGetCardsByUserID(DatabaseAPI.CardPostResponseData[] responseData)
        {
            for (var i = 0; i < responseData.Length; i++)
            {
                Debug.Log($"[{i}]\n{responseData[i]}");
            }
        }
    }
}