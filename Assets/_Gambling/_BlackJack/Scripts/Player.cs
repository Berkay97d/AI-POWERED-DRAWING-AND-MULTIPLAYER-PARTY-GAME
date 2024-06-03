using System;
using UnityEngine;
using UnityEngine.UI;

namespace BlackJack
{
    public class Player : Gambler
    {
        [SerializeField] private Button stopButton;
        [SerializeField] private Button getCardButton;
        [SerializeField] private Button getCardAndDoubleButton;

        public static Player Instance { get; private set; }


        private void Awake()
        {
            Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            
            getCardButton.onClick.AddListener(GetCard);
            stopButton.onClick.AddListener(Stop);
            getCardAndDoubleButton.onClick.AddListener(GetCardAndDouble);
        }

        private void GetCardAndDouble()
        {
            
        }

        
        
       
        
        
    }
}