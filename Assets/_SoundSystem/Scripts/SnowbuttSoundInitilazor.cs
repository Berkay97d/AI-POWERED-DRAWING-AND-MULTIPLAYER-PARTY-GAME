using System;
using System.Collections;
using System.Collections.Generic;
using _SoundSystem.Scripts;
using UnityEngine;

public class SnowbuttSoundInitilazor : MonoBehaviour
{
    private SoundManager m_SoundManager;


    private void Awake()
    {
        m_SoundManager = GetComponent<SoundManager>();
    }

    
    private void Start()
    {
        m_SoundManager.PlaySnowbuttBackgroundSound();
    }
}
