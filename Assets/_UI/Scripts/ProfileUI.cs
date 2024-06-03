using System;
using _DatabaseAPI.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LocalUser = _UserOperations.Scripts.LocalUser;
using Random = UnityEngine.Random;

namespace UI
{
    public class ProfileUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameField;
        [SerializeField] private TMP_Text gemField;
        [SerializeField] private TMP_Text coinField;
        [SerializeField] private TMP_Text energyField;
        

        private void Awake()
        {
            LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged += OnLocalUserDataChanged;
        }

        private void OnDestroy()
        {
            LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged -= OnLocalUserDataChanged;
        }


        private void OnLocalUserDataLoaded(UserData userData)
        {
            SetUserData(userData);
        }
        
        private void OnLocalUserDataChanged(UserData userData)
        {
            SetUserData(userData);
        }


        private void SetUserData(UserData userData)
        {
            SetName(userData.name);
            SetGemCount(userData.gem);
            SetCoinCount(userData.coin);
            SetEnergyCount(userData.energy);
            SetExperienceCount(userData.experience);
        }
        
        private void SetName(string newName)
        {
            nameField.text = newName;
        }

        private void SetGemCount(int count)
        {
            gemField.text = count.ToString();
        }

        private void SetCoinCount(int count)
        {
            coinField.text = count.ToString();
        }

        private void SetEnergyCount(int count)
        {
            energyField.text = $"{count}/9";
        }

        private void SetExperienceCount(int count)
        {
            // TODO calculate level here
            var level = count;
            // TODO calculate level progress (0f - 1f)
            var levelProgress = Random.Range(0f, 1f);
            
            /*SetLevel(level);
            SetLevelProgress(levelProgress);*/
        }

        /*private void SetLevel(int level)
        {
            levelField.text = level.ToString();
        }

        private void SetLevelProgress(float t)
        {
            levelProgress.fillAmount = t;
        }*/
    }
}