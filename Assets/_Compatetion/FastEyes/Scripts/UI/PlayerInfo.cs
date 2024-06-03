using System;
using System.Collections;
using System.Collections.Generic;
using FastEyes;
using TMPro;
using UnityEngine;

namespace FastEyes
{ 
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private Contester contester;
        [SerializeField] private TMP_Text text;


        private void Start()
        {
            UpdateScoreText();
        }

        public void UpdateScoreText()
        {
            text.text = contester.GetScore().ToString();
        }
    }
}