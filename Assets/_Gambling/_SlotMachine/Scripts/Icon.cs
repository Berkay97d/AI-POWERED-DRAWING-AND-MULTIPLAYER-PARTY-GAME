using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class Icon : MonoBehaviour
    {
        [SerializeField] private int id;

        private Vector3 startPos;


        private void Start()
        {
            startPos = transform.position;
        }

        public int GetId()
        {
            return id;
        }

        public void GoUppest(Vector3 uppestIconPos, float gap)
        {
            transform.position = uppestIconPos + new Vector3(0, gap, 0);
        }
    }
}