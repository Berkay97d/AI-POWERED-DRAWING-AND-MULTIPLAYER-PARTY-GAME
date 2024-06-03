using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private ColumnController[] columnControllers;
        
        public static GameController Instance { get; private set; }
        public event EventHandler OnGameStarted;
        public event EventHandler OnGameOver;

        private bool isGameOverRaised = false; 
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            startGameButton.onClick.AddListener(RaiseOnGameStarted);
            OnGameOver += OnOnGameOver;
        }

        private void OnOnGameOver(object sender, EventArgs e)
        {
            DecidePlayerWinState();
        }

        private void RaiseOnGameStarted()
        {
            OnGameStarted?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            foreach (var column in columnControllers)
            {
                if (!column.GetIsColumnStopped())
                {
                    return;
                }
            }
            
            RaiseOnGameOver();
        }

        private void DecidePlayerWinState()
        {
            var first = columnControllers[0].GetMiddleIcon().GetId();
            var second = columnControllers[1].GetMiddleIcon().GetId();
            var third = columnControllers[2].GetMiddleIcon().GetId();

            if (first == second  && first == third)
            {
                Debug.Log("TRİPLE WİN");
                return;
            }

            if (first == second || first == third || second == third)
            {
                Debug.Log("DOUBLE WİN");
                return;
            }

            Debug.Log("LOSE");
        }

        private void RaiseOnGameOver()
        {
            if (isGameOverRaised) return;
            
            OnGameOver?.Invoke(this,EventArgs.Empty);
        }
        
        
    }
}