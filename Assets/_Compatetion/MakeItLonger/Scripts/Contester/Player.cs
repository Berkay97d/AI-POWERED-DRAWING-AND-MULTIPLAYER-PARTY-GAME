using System;
using System.Collections;
using System.Collections.Generic;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using UnityEngine;

namespace MakeItLonger
{
    public class Player : Contester
    {
        private int winnedRoundCount = 0;
        
        
        
        void Start()
        {
            InputObject.OnPlayerClick += OnPlayerClick;
            gameController.OnRoundOver += OnRoundOver;
        }
        
        private void OnDestroy()
        {
            InputObject.OnPlayerClick -= OnPlayerClick;
        }
        
        private void OnRoundOver(object sender, OnRoundOverArgs e)
        {
            if (e.WinnerContester == this)
            {
                winnedRoundCount++;

                if (winnedRoundCount == 2)
                {
                    LocalUser.AddQuestProgress(QuestType.Win_Competition);
                    LocalUser.AddQuestProgress(QuestType.Win_Stack);
                }
                IncreaseRoundScore();

                if (GetRoundScore() == 2)
                {
                    gameController.OverTheGame(this);
                }
            }
        }

        private void OnPlayerClick(object sender, EventArgs e)
        {
            StartCoroutine(InnerRoutine());
            
            IEnumerator InnerRoutine()
            {
                while (CurrentBox == null)
                {
                    yield return new WaitForSeconds(.1f);
                }

                CurrentBox.RaiseOnBoxReleased(CurrentBox, this);    
            }
            
        }

        
        
        
        
    }
}