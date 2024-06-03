using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace BlackJack
{
    public enum Winner
    {
        Player,
        AI,
        Tie
    }
    
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<Card> deck = new List<Card>();
        [SerializeField] private Gambler[] gamblers;
        [SerializeField] private Button startGameButton;
        
        public static GameController Instance { get; private set; }
        public event EventHandler OnGameStarted;
        public event EventHandler OnGameOver;

        private Winner winner;


        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            
            OnGameStarted += OnOnGameStarted;
            OnGameOver += OnOnGameOver;

            startGameButton.onClick.AddListener(() =>
            {
                OnGameStarted?.Invoke(this, EventArgs.Empty);
            });
        }

        private void OnOnGameOver(object sender, EventArgs e)
        {
            SetWinner();
        }

        private void SetWinner()
        {
            if (gamblers[0].GetOpenHandPoint() > 21)
            {
                winner = Winner.AI;
                Debug.Log("AI WİN");
                return;
            }
            
            if (gamblers[1].GetOpenHandPoint() > 21)
            {
                winner = Winner.Player;
                Debug.Log("PLAYER WİN");
                return;
            }
            
            if (gamblers[0].GetOpenHandPoint() > gamblers[1].GetOpenHandPoint())
            {
                winner = Winner.Player;
                Debug.Log("PLAYER WİN");
                return;
            }

            if (gamblers[1].GetOpenHandPoint() > gamblers[0].GetOpenHandPoint())
            {
                winner = Winner.AI;
                Debug.Log("AI WİN");
                return;
            }

            Debug.Log("TİE");
            winner = Winner.Tie;
        }

        private void OnOnGameStarted(object sender, EventArgs e)
        {
            GiveStartCards();
        }
        
        private Card GetRandomCard()
        {
            var randomCard = deck[Random.Range(0, deck.Count)];

            return randomCard;
        }
        
        private void GiveStartCards()
        {
            GiveCard(gamblers[0], true);
            GiveCard(gamblers[0], true);
            GiveCard(gamblers[1], true);
            GiveCard(gamblers[1], false);
            gamblers[0].RaiseOnGamblerTakeCard();
            gamblers[1].RaiseOnGamblerTakeCard();
        }
        
        public Card GiveCard(Gambler gambler, bool isOpen)
        {
            var card = GetRandomCard();
            card.SetParent(gambler.transform,gambler.GetCardPosition(), isOpen);
            deck.Remove(card);
            gambler.AddCardToHand(card);

            return card;
        }

        public void RaiseOnGameOver()
        {
            OnGameOver?.Invoke(this,EventArgs.Empty);
        }

       
        
        
        
        
    }
}