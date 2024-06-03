using System;
using General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FastEyes.Temp
{
    public class SceneLoader : MonoBehaviour
    {
        private bool isSceneLoaded = false;
        
        private void Start()
        {
            GameController.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GameOver)
            {
                if (isSceneLoaded) return;

                isSceneLoaded = true;
                
                LazyCoroutines.WaitForSeconds(6f, () =>
                {
                    SceneManager.LoadScene(0);
                });
            }
        }
    }
}