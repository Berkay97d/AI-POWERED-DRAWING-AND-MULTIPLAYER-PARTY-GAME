using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MakeItLonger
{
    public enum AiLevel
    {
        LOW,
        MID,
        HIGH,
    }
    
    public class Ai : Contester
    {
        [SerializeField] private AiLevel aiLevel;
        [SerializeField] private int successDecreaseRatioByLine;
        [SerializeField] private float minimumResponseTime;
        [SerializeField] private float maximumResponseTime;
        

        private int targetColumn;
        private float lastDropTime;
        private float time;
        private bool isDecided = false;
        private IEnumerator routine;

        private void Start()
        {
            SelectRandomTargetColumn();

            gameController.OnGameStateChanged += OnGameStateChanged;
            gameController.OnRoundOver += OnRoundOver;
        }

        private void OnDisable()
        {
            gameController.OnGameStateChanged -= OnGameStateChanged;
            gameController.OnRoundOver -= OnRoundOver;
        }
        
        private void Update()
        {
            if (!gameController.GetIsGameStarted()) return;
            if (gameController.GetGameState() != GameState.GamePlaying) return;
            if (CurrentBox == null) return;

            time += Time.deltaTime;

            if (time - lastDropTime > GetRandomResponseTime())
            {
                DecideDrop();
            }
        }

        private void OnRoundOver(object sender, OnRoundOverArgs e)
        {
            if (e.WinnerContester == this)
            { 
                IncreaseRoundScore();

                if (GetRoundScore() == 2)
                {
                    gameController.OverTheGame(this);
                }
            }
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                time = 0;
                lastDropTime = 0;
                SelectRandomTargetColumn();
            }
            else
            {
                StopCoroutine(DropRoutine());
            }
        }

        private void SelectRandomTargetColumn()
        {
            Debug.Log("TARGET COLUMN SELECTED");
            targetColumn = Random.Range(0, 5);
        }

        private bool GetSuccessOrNot(AiLevel aLevel, int targetLine)
        {
            var randNum = Random.Range(0, 100);
            var decreaser = targetLine * successDecreaseRatioByLine ;
            
            if (aLevel == AiLevel.HIGH)
            {
                return randNum > 4 + decreaser;
            }
            if (aLevel == AiLevel.MID)
            {
                return randNum > 14 + decreaser;
            }
            if (aLevel == AiLevel.LOW)
            {
                return randNum > 24 + decreaser;
            }

            return false;
        }
        
        private void DecideDrop()
        {
            StartCoroutine(DropRoutine());
        }
        
        private IEnumerator DropRoutine()
        {
            if (isDecided) yield break;
                
            isDecided = true;
                
            var isSuccess = GetSuccessOrNot(aiLevel, GetContesterGrid().GetProperLineIndex());
                
            if (isSuccess)
            {
                while (true)
                {
                    yield return null;

                    if (CurrentBox == null) yield break;

                    if (CurrentBox.GetContester().GetContesterGrid().GetGridFromWorld(CurrentBox.transform.position).xIndex == targetColumn)
                    {
                        lastDropTime = time;
                        CurrentBox.RaiseOnBoxReleased(CurrentBox, this);
                        isDecided = false;
                        yield break;
                    }
                }
            }
                
            while (true)
            {
                yield return null;
                    
                if (CurrentBox == null) yield break;
                    
                if (CurrentBox.GetContester().GetContesterGrid().GetGridFromWorld(CurrentBox.transform.position).xIndex != targetColumn)
                {
                    lastDropTime = time;
                    CurrentBox.RaiseOnBoxReleased(CurrentBox, this);
                    isDecided = false;
                    yield break;
                }
            }
        }

        private float GetRandomResponseTime()
        {
            return Random.Range(minimumResponseTime, maximumResponseTime);
        }
        
        
    }
}