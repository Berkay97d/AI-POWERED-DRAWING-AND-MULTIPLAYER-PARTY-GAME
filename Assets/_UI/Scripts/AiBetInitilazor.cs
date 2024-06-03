using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class AiBetInitilazor : MonoBehaviour
{
    [SerializeField] private List<Card> allCards;


    private Card[] GetRandomBet()
    {
        var betCount = Random.Range(1, 6);
        var bet = new Card[betCount];
        
        for (int i = 0; i < betCount; i++)
        {
            bet[i] = allCards[Random.Range(0, allCards.Count - 1)];
        }

        return bet;
    }

    public int[] GetRandomAiBet()
    {
        return GetRandomBet()
            .Select(card => (int)card.GetCardData().Rarity)
            .ToArray();
    }
    
    
}
