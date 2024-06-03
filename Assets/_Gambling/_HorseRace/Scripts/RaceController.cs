
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseRace
{


    public class RaceController : MonoBehaviour
    {
        [SerializeField] private Horse[] horses;
        [SerializeField] private Button startRaceButton;

        public static RaceController Instance { get; private set; }
        public event EventHandler OnRaceStarted;

        private List<Horse> sortedHorses;
        private bool isRaceStarted;


        private void Awake()
        {
            Instance = this;

            sortedHorses = new List<Horse>(horses);
        }

        private void Start()
        {
            startRaceButton.onClick.AddListener(StartRace);
        }

        private void Update()
        {
            SortHorses();
        }

        private void SortHorses()
        {
            for (int i = 0; i < sortedHorses.Count - 1; i++)
            {
                if (sortedHorses[i].transform.position.z < sortedHorses[i + 1].transform.position.z)
                {
                    (sortedHorses[i], sortedHorses[i + 1]) = (sortedHorses[i + 1], sortedHorses[i]);
                }
            }
        }

        private void StartRace()
        {
            isRaceStarted = true;
            OnRaceStarted?.Invoke(this, EventArgs.Empty);
        }

        public Horse GetLeaderHorse()
        {
            return sortedHorses[0];
        }

        public bool GetIsRaceStarted()
        {
            return isRaceStarted;
        }

    }
}