using System;
using UnityEngine;

namespace RightPath
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private Vector3 offset;
        private Vector3 velocity = Vector3.zero;

        private float smoothTime = 0.1f;


        private void Awake()
        {
            offset = transform.position - target.position;
        }

        void FixedUpdate()
        {
            var position = this.transform.position;

            position.x = target.transform.position.x + offset.x;
            
            position.y = target.transform.position.y + offset.y;
            position.z = target.transform.position.z + offset.z;
        
            transform.position = Vector3.SmoothDamp(this.transform.position, position, ref velocity, smoothTime);
        }
    }
}