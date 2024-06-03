using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MakeItLonger
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Vector3 gridSize;
        private GameController gameController;
        
        public int xIndex;
        public int yIndex;
        public bool isFull = false;

        private void Start()
        {
            gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
            gameController.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            gameController.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                isFull = false;
            }
        }

        public Vector3 GetGridSize()
        {
            return gridSize;
        }
    }
}