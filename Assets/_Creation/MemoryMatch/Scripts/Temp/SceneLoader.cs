using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Compatetion.MemoryMatch
{

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private MemoryMatchManager manager;


        private void Start()
        {
            manager.GetGame().OnGameOver += OnGameOver;
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