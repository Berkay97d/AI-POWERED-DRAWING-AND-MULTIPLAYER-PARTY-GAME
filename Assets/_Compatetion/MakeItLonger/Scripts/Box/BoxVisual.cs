using System;
using System.Collections;
using System.Collections.Generic;
using MakeItLonger;
using UnityEngine;

namespace MakeItLonger
{ 
    public class BoxVisual : MonoBehaviour
    {
        [SerializeField] private Box myBox;
        [SerializeField] private GameObject unplacedVisual;
        [SerializeField] private GameObject placedVisual;
        
        private void Start()
        {
            myBox.GetContester().OnBoxReleased += OnBoxReleased;
        }

        private void OnDestroy()
        {
            myBox.GetContester().OnBoxReleased -= OnBoxReleased;
        }
        
        private void OnBoxReleased(object sender, EventArgs e)
        {
            ActivatePlacedVisual();
        }

        private void ActivatePlacedVisual()
        {
            unplacedVisual.SetActive(false);
            placedVisual.SetActive(true);
        }

        
    }
}