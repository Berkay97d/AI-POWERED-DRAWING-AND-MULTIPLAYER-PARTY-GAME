using System.Collections.Generic;
using NaughtyAttributes;
using UI;
using UnityEngine;

namespace _MasterUser
{
    public class Test_Master_User : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform parent;


        private readonly List<Card> m_Cards = new();


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SpawnCards()
        {
            foreach (var card in m_Cards)
            {
                Destroy(card.gameObject);
            }
            
            m_Cards.Clear();
            
            MasterUser.GetCards(responses =>
            {
                foreach (var response in responses)
                {
                    var card = Instantiate(cardPrefab, parent);
                    m_Cards.Add(card);
                    card.SetCardData(response.card_data);
                }
            });
        }
        
    }
}