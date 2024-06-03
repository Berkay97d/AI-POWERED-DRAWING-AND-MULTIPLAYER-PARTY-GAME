using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace MakeItLonger
{
    public class InputObject : MonoBehaviour
    {
        [SerializeField] private BoxCollider inputCollider;
        [SerializeField] private float clickWaitTime;
        [SerializeField] private GameController gameController;
        
        public static event EventHandler OnPlayerClick;
        
        private float time = 0f;
        private float lastClickTime;

        private void Awake()
        {
            DisableInputObject();
        }

        private void Start()
        {
            gameController.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            gameController.OnGameStateChanged -= OnGameStateChanged;
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        private bool CheckIsClickable()
        {
            if (time - lastClickTime ! < clickWaitTime)
            {
                return false;
            }

            lastClickTime = time;
            return true;
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                LazyCoroutines.WaitForSeconds(0.2f, EnableInputObject);
            }
            else
            {
                DisableInputObject();
            }
        }
        
        private void OnMouseDown()
        {
            if (CheckIsClickable())
            {
                OnPlayerClick?.Invoke(this, EventArgs.Empty);
            }
        }

        private void DisableInputObject()
        {
            inputCollider.enabled = false;
        }

        private void EnableInputObject()
        {
            inputCollider.enabled = true;
        }
        
    }
}