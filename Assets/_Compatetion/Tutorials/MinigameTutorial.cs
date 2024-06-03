using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTutorial : MonoBehaviour
{
    [SerializeField] private Transform mainTransform;
    [SerializeField] private Button skipButton;
    
    public event EventHandler OnTutorialOver;
    
    private bool isTutorial = true;
    
    
    private void Awake()
    {
        if (isTutorial)
        {
            mainTransform.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void Start()
    {
        skipButton.onClick.AddListener(SkipTutorial);
    }

    public void SkipTutorial()
    {
        mainTransform.gameObject.SetActive(false);
        Time.timeScale = 1;
        OnTutorialOver?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }
    
   

  
    
    
}
