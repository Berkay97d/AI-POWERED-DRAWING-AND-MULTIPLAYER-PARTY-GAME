using System.Collections.Generic;
using UnityEngine;

namespace _Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        private const string TutorialPhaseKey = "Tutorial_Phase";


        [SerializeField] private int phase;
        [SerializeReference] private List<Tutorial> tutorials;


        private int m_TutorialIndex = -1;
        private bool m_Began;
        private bool m_Completed;


        private void Start()
        {
            if (GetTutorialPhase() != phase) return;
            
            Begin();
        }

        private void Update()
        {
            if (!m_Began) return;
            
            if (m_Completed) return;

            var tutorial = GetTutorial(m_TutorialIndex);
            
            tutorial.OnUpdate();
        }


        public void Begin()
        {
            Highlighter.SetActive(true);
            NextTutorial();

            m_Began = true;
        }


        private void OnCurrentTutorialComplete()
        {
            if (IsLastTutorial())
            {
                OnAllTutorialsComplete();
                return;
            }
            
            NextTutorial();
        }

        private void OnAllTutorialsComplete()
        {
            m_Completed = true;
            NextTutorialPhase();
            Debug.Log("all tutorials complete!");
        }
        
        
        private void NextTutorial()
        {
            m_TutorialIndex += 1;
            var tutorial = GetTutorial(m_TutorialIndex);
            tutorial.onComplete.AddListener(OnCurrentTutorialComplete);
            tutorial.Begin();
        }

        private Tutorial GetTutorial(int index)
        {
            return tutorials[index];
        }

        private bool IsLastTutorial()
        {
            return m_TutorialIndex == tutorials.Count - 1;
        }


        public static bool IsTutorialFinished()
        {
            return GetTutorialPhase() >= 3;
        }
        

        private static void NextTutorialPhase()
        {
            var phase = GetTutorialPhase();

            phase += 1;
            SetTutorialPhase(phase);
        }
        
        private static int GetTutorialPhase()
        {
            return PlayerPrefs.GetInt(TutorialPhaseKey, 0);
        }

        private static void SetTutorialPhase(int value)
        {
            PlayerPrefs.SetInt(TutorialPhaseKey, value);
        }
    }
}