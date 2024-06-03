using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFall
{ 
    public class PlayerController : Contester
    {
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private float speed;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override void Update()
        {
            base.Update();
            if (isDead) return;
            if (-1 * GetMoveDirection() == Vector3.zero) return;

            transform.forward = -1*GetMoveDirection();
            
            Move();
        }

        private void Move()
        {
            var moveVector = GetMoveDirection() * speed;
            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
        }

        public Vector3 GetMoveDirection()
        {
            var dir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized;
            return dir;
        }
    }
}