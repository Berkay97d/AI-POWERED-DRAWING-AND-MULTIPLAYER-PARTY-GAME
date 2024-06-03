using System;
using UnityEngine;

namespace DontFall
{
    public class AIMover : MonoBehaviour
    {
        private Rigidbody rb;
        private Vector3 target;
        private float speed;


        private void Update()
        {
            if (GameController.Instance.GetIsGameOver())
            {
                return;
            }
            
            Move();
        }

        private void Move()
        {
            const float tolerance = 0.1f;
            var targetNoY = new Vector3(target.x, transform.position.y, target.z);
            
            if (Vector3.Distance(transform.position, targetNoY) < tolerance)
            {
                var velo = new Vector3(0, rb.velocity.y, 0);
                rb.velocity = velo;
                return;
            }
            
            var direction = target - transform.position;
            
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-direction), Time.deltaTime * 25);
            
            var velocity = direction.normalized * speed;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

        public void SetTarget(Vector3 targetPos)
        {
            target = targetPos;
        }

        public void SetRigidbody(Rigidbody body)
        {
            rb = body;
        }

        public void SetSpeeed(float s)
        {
            speed = s;
        }
        
    }
}