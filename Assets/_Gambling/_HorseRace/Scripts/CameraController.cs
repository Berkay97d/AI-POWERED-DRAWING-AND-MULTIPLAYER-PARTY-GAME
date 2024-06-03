using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseRace
{


    public class CameraController : MonoBehaviour
    {
        [SerializeField] private RaceController raceController;


        private void LateUpdate()
        {
            Follow(raceController.GetLeaderHorse().transform);
        }

        private void Follow(Transform target)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.transform.position.z);
        }
    }
}