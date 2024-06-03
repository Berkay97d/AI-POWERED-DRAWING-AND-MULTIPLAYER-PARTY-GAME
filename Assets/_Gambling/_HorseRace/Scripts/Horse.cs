using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HorseRace
{


    public class OnRaceOverEventArgs : EventArgs
    {
        public Horse WinnerHorse;
    }

    public class Horse : MonoBehaviour
    {
        [SerializeField] private float speedChangeTime;
        [SerializeField] private string horseName;
        [SerializeField] private int horseNumber;
        [SerializeField] private float multiplier;
        
        public static event EventHandler<OnRaceOverEventArgs> OnRaceOver;

        private static float MaxMoveSpeed = 12;
        private static float MinMoveSpeed = 8;
        private float currentMoveSpeed;
        private float deltaTime;
        private bool isRaceOver;


        private void Start()
        {
            currentMoveSpeed = GetRandomMoveSpeed();
            OnRaceOver += OnOnRaceOver;
        }

        private void OnOnRaceOver(object sender, OnRaceOverEventArgs e)
        {
            StopHorse();
            OverRace();
            LogWinner(e);
        }

        private void Update()
        {
            if (!RaceController.Instance.GetIsRaceStarted()) return;
            MoveHorse();
            UpdateMoveSpeed();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnRaceOver?.Invoke(this, new OnRaceOverEventArgs
            {
                WinnerHorse = this
            });
        }

        private void MoveHorse()
        {
            transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);
        }

        private void StopHorse()
        {
            currentMoveSpeed = 0;
        }

        private void OverRace()
        {
            isRaceOver = true;
        }

        private float GetRandomMoveSpeed()
        {
            return currentMoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
        }

        private void UpdateMoveSpeed()
        {
            if (isRaceOver) return;

            deltaTime += Time.deltaTime;

            if (deltaTime >= speedChangeTime)
            {
                deltaTime = 0;
                currentMoveSpeed = GetRandomMoveSpeed();
            }
        }

        private static void LogWinner(OnRaceOverEventArgs e)
        {
            Debug.Log("WİNNER: " + e.WinnerHorse.name);
        }

        public string GetHorseName()
        {
            return horseName;
        }

        public int GetHorseNumber()
        {
            return horseNumber;
        }

        public float GetMultiplier()
        {
            return multiplier;
        }

    }
}