using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _UserOperations.Scripts
{
    public class CreateAccountUI : MonoBehaviour
    {
        [SerializeField] private GameObject main;
        [SerializeField] private Image background;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_Text errorMsg;
        [SerializeField] private float toggleDuration = 0.25f;


        private Action<AccountInfo> m_OnCreated;


        private void Awake()
        {
            confirmButton.onClick.AddListener(OnClickConfirmButton);
        }


        public void StartCreatingAccount(Action<AccountInfo> onCreated)
        {
            m_OnCreated = onCreated;
            Show();
        }


        private void OnClickConfirmButton()
        {
            if (IsNameFieldEmpty())
            {
                PopErrorMsg("Please enter a valid name!");
                return;
            }
            
            Hide();
            
            m_OnCreated?.Invoke(new AccountInfo
            {
                name = GetName()
            });
        }
        

        private void Show()
        {
            main.SetActive(true);
            
            background.DOColor(new Color(0f, 0f, 0f, 0.8f), toggleDuration)
                .SetEase(Ease.OutSine);

            panel.DOScale(Vector3.one, toggleDuration)
                .SetEase(Ease.OutBack);
        }

        private void Hide()
        {
            background.DOColor(new Color(0f, 0f, 0f, 0f), toggleDuration)
                .SetEase(Ease.InSine);

            panel.DOScale(Vector3.zero, toggleDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    main.SetActive(false);
                });
        }

        private void PopErrorMsg(string msg)
        {
            errorMsg.text = msg;
            errorMsg.transform.DOScale(Vector3.one, 0.2f)
                .From(Vector3.zero)
                .SetEase(Ease.OutBack);
        }

        private string GetName()
        {
            return nameInputField.text;
        }
        
        private bool IsNameFieldEmpty()
        {
            return GetName().Length <= 0;
        }



        public struct AccountInfo
        {
            public string name;
        }
    }
}