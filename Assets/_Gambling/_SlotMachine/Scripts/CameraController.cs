using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using SlotMachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;


    private void Start()
    {
        GameController.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnGameStarted(object sender, EventArgs e)
    {
        GoGameCamera();
    }

    private void GoGameCamera()
    {
        camera.Priority = 8;
    }
}
