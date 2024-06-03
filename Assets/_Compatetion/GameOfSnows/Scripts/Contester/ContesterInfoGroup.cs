using System;
using _Compatetion.GameOfSnows.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Compatetion.GameOfSnows.Ozer.Scripts
{
    public class ContesterInfoGroup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] textArray;
        

        private void Update()
        {
            var contesterArray = GameManager.Instance.GetContesterArray();

            for (var i = 0; i < textArray.Length; i++)
            {
                if (i < contesterArray.Length)
                {
                    var contester = contesterArray[i];
                    textArray[i].text = contester.GetPoint().ToString();
                }
                else
                {
                    textArray[i].text = string.Empty;
                }
            }
        }
    }
}