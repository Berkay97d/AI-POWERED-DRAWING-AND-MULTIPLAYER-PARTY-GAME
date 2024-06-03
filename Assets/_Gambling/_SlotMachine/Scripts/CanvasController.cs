using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{ 
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas betCanvas;


        private void Start()
        {
            GameController.Instance.OnGameStarted += OnGameStarted;
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            betCanvas.gameObject.SetActive(false);
        }
    }
}