using _DatabaseAPI.Scripts;
using _MasterUser;
using _UserOperations.Scripts;
using NaughtyAttributes;
using StableDiffusionAPI;
using UnityEngine;

public class MasterUserInitilazor : MonoBehaviour
{
    [SerializeField] private Texture2D[] cardImages;
    [SerializeField] private CardData[] cards;

    
    private int m_Index;


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void PostCards()
    {
        UserAPI.SetUserData(GetUserID(), UserData.Default, PostCard);
    }
    
    private void PostCard()
    {
        var image = cardImages[m_Index];
        var base64 = TextureUtils.ToBase64String(image);
        
        DatabaseAPI.PostImage(base64, response =>
        {
            var data = cards[m_Index];
            data.imageID = response.body;
            
            DatabaseAPI.PostCard(GetUserID(), data, _ =>
            {
                Debug.Log($"[{m_Index}]: Completed!");
                
                if (m_Index == cards.Length - 1)
                {
                    m_Index = 0;
                    return;
                }

                m_Index += 1;
                PostCard();
            });
        });
    }

    private string GetUserID()
    {
        return MasterUser.MasterUserId;
    }
}
