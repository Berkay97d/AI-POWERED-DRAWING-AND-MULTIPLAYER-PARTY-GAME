using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DontFall
{
    public class ContesterInfoUI : MonoBehaviour
    {
        [SerializeField] private Contester contester;
        [SerializeField] private GameObject deadImage;


        private void Start()
        {
            contester.OnContesterDrop += OnContesterDrop;
        }

        private void OnContesterDrop(object sender, Contester e)
        {
            deadImage.SetActive(true);
        }
    }
}