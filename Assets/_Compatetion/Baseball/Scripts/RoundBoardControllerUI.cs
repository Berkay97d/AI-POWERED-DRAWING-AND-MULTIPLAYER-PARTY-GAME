using System;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace _Compatetion.Baseball.Scripts
{
    public class RoundBoardControllerUI : MonoBehaviour
    {
        [SerializeField] private Image[] greenImageArray;
        [SerializeField] private Image[] orangeImageArray;


        private void Start()
        {
            FruitManager.Instance.OnFruitsCreated += OnFruitsCreated;
        }

        private void OnFruitsCreated()
        {
            var currentRound = FruitManager.Instance.GetCurrentRound();
            
            orangeImageArray[currentRound].gameObject.SetActive(true);

            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                greenImageArray[currentRound].gameObject.SetActive(true);
            });
        }
    }
}
