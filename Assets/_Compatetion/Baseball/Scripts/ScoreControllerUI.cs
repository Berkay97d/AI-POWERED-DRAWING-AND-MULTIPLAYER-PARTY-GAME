using System;
using TMPro;
using UnityEngine;

namespace _Compatetion.Baseball.Scripts
{
    public class ScoreControllerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] contesterScoreTextArray;
        [SerializeField] private Contester[] contesterArray;


        private void Start()
        {
            foreach (var contesterScoreText in contesterScoreTextArray)
            {
                contesterScoreText.text = 0.ToString();
            }
        }


        private void Update()
        {
            for (var index = 0; index < contesterArray.Length; index++)
            {
                var contester = contesterArray[index];
                contesterScoreTextArray[index].text = contester.GetPoint().ToString();
            }
        }
    }
}