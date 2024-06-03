using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionChecker : MonoBehaviour
{
    [SerializeField] private float connectionCheckWaitTime;
    [SerializeField] private Transform connectionLossCanvasTransform;
    
    public event EventHandler OnConnectionGone;
    public event EventHandler OnConnectionBack;
    
    private float time;
    private bool isConnected = true;
    
    
    
    
    private void Update()
    {
        time += Time.deltaTime;

        if (!(time > connectionCheckWaitTime)) return;
        
        time = 0;
            
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            connectionLossCanvasTransform.gameObject.SetActive(true);

            if (!isConnected) return;
            
            OnConnectionGone?.Invoke(this, EventArgs.Empty);
            isConnected = false;
            
            return;
        }
        
        connectionLossCanvasTransform.gameObject.SetActive(false);

        if (isConnected) return;
        
        OnConnectionBack?.Invoke(this, EventArgs.Empty);
        isConnected = true;
        


    }
}
