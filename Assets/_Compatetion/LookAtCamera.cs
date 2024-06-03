using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Mode mode;
    [SerializeField] private bool invert;

    private Transform m_CameraTransform;


    private void Awake()
    {
        m_CameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
            {
                var up = invert ? Vector3.down : Vector3.up;
                transform.LookAt(m_CameraTransform, up);
                break;
            }
            case Mode.LookForward:
            {
                var sign = invert ? -1f : 1f;
                transform.forward = sign * m_CameraTransform.forward;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public void SetMode(Mode _mode)
    {
        mode = _mode;
    }


    public void SetInvert(bool invert)
    {
        this.invert = invert;
    }


    public enum Mode
    {
        LookAt,
        LookForward
    }

}
