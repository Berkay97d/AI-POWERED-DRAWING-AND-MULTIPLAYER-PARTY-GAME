using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BlackJack
{ 
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas inGameCanvas;
        [SerializeField] private Canvas betCanvas;
        [SerializeField] private GameObject buttons;
        

        private void Start()
        {
            GameController.Instance.OnGameStarted += OnGameStarted;
            Player.Instance.OnGamblerStop += OnGamblerStop;
        }

        private void OnGamblerStop(object sender, EventArgs e)
        {
            buttons.SetActive(false);
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            betCanvas.gameObject.SetActive(false);
            inGameCanvas.gameObject.SetActive(true);
        }
    }
}