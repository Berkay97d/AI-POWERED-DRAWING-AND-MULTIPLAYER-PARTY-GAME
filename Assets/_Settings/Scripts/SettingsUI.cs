using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace _Settings.Scripts
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private GameObject main;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Image background;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private float toggleDuration = 0.25f;


        private void Awake()
        {
            LoadMusicVolume();
            LoadSfxVolume();
            
            closeButton.onClick.AddListener(OnClickCloseButton);
            musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
        }


        private void OnClickCloseButton()
        {
            Close();
        }
        
        private void OnMusicSliderValueChanged(float value)
        {
            GameSettings.SetMusicVolume(value);
        }

        private void OnSfxSliderValueChanged(float value)
        {
            GameSettings.SetSfxVolume(value);
        }
        
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Open()
        {
            main.SetActive(true);
            
            background.DOColor(new Color(0f, 0f, 0f, 0.8f), toggleDuration)
                .SetEase(Ease.OutSine);

            panel.DOScale(Vector3.one, toggleDuration)
                .SetEase(Ease.OutBack);
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Close()
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
        

        private void LoadMusicVolume()
        {
            musicSlider.value = GameSettings.GetMusicVolume();
        }

        private void LoadSfxVolume()
        {
            sfxSlider.value = GameSettings.GetSfxVolume();
        }
    }
}