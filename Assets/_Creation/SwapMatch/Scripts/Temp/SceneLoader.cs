using System;
using System.Collections;
using System.Collections.Generic;
using _Creation.SwapMatch.Scripts;
using General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Creation.SwapMatch
{

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SwapMatchManager manager;


        private void Start()
        {
            manager.OnGameOver += OnGameOver;
        }

        private void OnGameOver(object sender, bool e)
        {
            LazyCoroutines.WaitForSeconds(3f, () =>
            {
                SceneManager.LoadScene(0);
            });
        }
    }
}