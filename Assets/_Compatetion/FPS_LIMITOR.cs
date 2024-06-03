using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_LIMITOR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        //Debug.Log((int)(1f/Time.deltaTime));
    }
}
