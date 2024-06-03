using System;
using UnityEngine;

namespace HorseRace
{


    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas betCanvas;
        [SerializeField] private Canvas gameOverCanvas;
        [SerializeField] private Canvas commonCanvasUp;
        [SerializeField] private Canvas placementCanvas;
        
        
        private void Start()
        {
            RaceController.Instance.OnRaceStarted += OnRaceStarted;
            Horse.OnRaceOver += OnRaceOver;
        }

        private void OnRaceStarted(object sender, EventArgs e)
        {
            HideBetCanvas();
            HideCommonCanvas();
            ShowPlacementCanvas();
        }
        
        private void OnRaceOver(object sender, OnRaceOverEventArgs e)
        {
            ShowCommonCanvas();
            ShowGameOverCanvas();
            HidePlacementCanvas();
        }

        private void ShowBetCanvas()
        {
            betCanvas.gameObject.SetActive(true);
        }

        private void HideBetCanvas()
        {
            betCanvas.gameObject.SetActive(false);
        }

        private void ShowGameOverCanvas()
        {
            gameOverCanvas.gameObject.SetActive(true);
        }

        private void HideGameOverCanvas()
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        
        private void ShowPlacementCanvas()
        {
            placementCanvas.gameObject.SetActive(true);
        }

        private void HidePlacementCanvas()
        {
            placementCanvas.gameObject.SetActive(false);
        }
        
        private void ShowCommonCanvas()
        {
            commonCanvasUp.gameObject.SetActive(true);
        }

        private void HideCommonCanvas()
        {
            commonCanvasUp.gameObject.SetActive(false);
        }
    }
}
