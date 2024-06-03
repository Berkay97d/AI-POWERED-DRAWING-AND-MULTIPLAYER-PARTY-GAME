using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MakeItLonger
{
    public class Contester : MonoBehaviour
    {
        [SerializeField] protected ContesterGrid contesterGrid;
        [SerializeField] protected GameController gameController;
        
        public event EventHandler<OnBoxReleasedEventArgs> OnBoxReleased;
        
        protected Box CurrentBox;
        private int roundScore = 0;

        
        public ContesterGrid GetContesterGrid()
        {
            return contesterGrid;
        }

        public void SetCurrentBox(Box box)
        {
            CurrentBox = box;
        }

        public Box GetCurrentBox()
        {
            return CurrentBox;
        }

        public void RaiseOnBoxReleased(OnBoxReleasedEventArgs args)
        {
            OnBoxReleased?.Invoke(this, args);
            
            if (gameController.GetGameState() == GameState.GamePlaying)
            {
                gameController.InitBox(this);
            }
        }

        protected int GetRoundScore()
        {
            return roundScore;
        }

        protected void IncreaseRoundScore()
        {
            roundScore++;
        }

    }
}