using System;
using System.Collections.Generic;
using System.Linq;
using EnterAndExitUI_Competation.Scripts;
using General;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DontFall
{
    public class SceneLoader : MonoBehaviour
    {
        private void Start()
        {
            GameController.Instance.OnGameOver += OnGameOver;
        }

        private void OnGameOver(object sender, List<Contester> e)
        {
            var playerOrder = e
                .Select(contester => contester.GetIndex())
                .ToArray();
            
            GameController.Instance.ZoomWinner(e[0]);
            Debug.Log(e[0].gameObject.name);
                
            LazyCoroutines.WaitForSeconds(5f, () =>
            {
                ExitTest_Script.SetPlayerOrder(playerOrder);
                SceneManager.LoadScene(6);
            });
        }
    }
}