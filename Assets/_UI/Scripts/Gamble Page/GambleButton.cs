using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
    public enum GambleType
    {
        Blackjack,
        Horse,
        Slot
    }
    
    public class GambleButton : MonoBehaviour
    {
        [SerializeField] private GambleType gambleType;

        private Button button;


        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(TryLoadGambleScene);
        }

        private void TryLoadGambleScene()
        {
            //TODO enerji yeterli mi check
            
            switch (gambleType)
            {
                case GambleType.Blackjack:
                    SceneManager.LoadScene(7);
                    break;
                case GambleType.Horse:
                    SceneManager.LoadScene(8);
                    break;
                case GambleType.Slot:
                    SceneManager.LoadScene(9);
                    break;
            }
        }
    }
}