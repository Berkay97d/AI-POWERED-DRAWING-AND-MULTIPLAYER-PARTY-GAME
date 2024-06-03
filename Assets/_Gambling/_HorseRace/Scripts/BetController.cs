using System;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HorseRace
{
    public class BetController : MonoBehaviour
    {
        [SerializeField] private HorseBetArea[] horseBetAreas;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;
        
        [SerializeField] private TMP_Text betCountText;

        [SerializeField] private FastBetButton[] fastBetButtons;
        
        
        public static BetController Instance;
        public event EventHandler OnBetChanged;
        
        private HorseBetArea selectedHorseBetArea;
        private const int MinusPlusSize = 5;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var horseBetArea in horseBetAreas)
            {
                horseBetArea.OnBetAreaButtonClicked += OnBetAreaButtonClicked;
            }

            foreach (var fastBetButton in fastBetButtons)
            {
                fastBetButton.OnFastBetButtonClick += OnFastBetButtonClick;
            } 
            
            minusButton.onClick.AddListener(MinusBet);
            plusButton.onClick.AddListener(PlusBet);
        }
        
        private void OnDestroy()
        {
            foreach (var horseBetArea in horseBetAreas)
            {
                horseBetArea.OnBetAreaButtonClicked -= OnBetAreaButtonClicked;
            }
            
            foreach (var fastBetButton in fastBetButtons)
            {
                fastBetButton.OnFastBetButtonClick -= OnFastBetButtonClick;
            } 
        }

        private void Update()
        {
            UpdateBetCountText();
        }

        private void OnFastBetButtonClick(object sender, int e)
        {
            SetBetOfSelected(e);
        }
        
        private void OnBetAreaButtonClicked(object sender, HorseBetArea e)
        {
            UpdateSelectedBetArea(e);
        }

        private void UpdateSelectedBetArea(HorseBetArea e)
        {
            if (selectedHorseBetArea == e) return;

            if (selectedHorseBetArea)
            {
                e.ActivateSelectedUi();
                selectedHorseBetArea.ActivateUnselectedUi();
                selectedHorseBetArea = e;
                
            }
            else
            {
                e.ActivateSelectedUi();
                selectedHorseBetArea = e;
                
            }
        }

        private void UpdateBetCountText()
        {
            if (!selectedHorseBetArea)
            {
                betCountText.text = 0.ToString();
                return;
            }
            
            betCountText.text = selectedHorseBetArea.GetPlacedBetCount().ToString();
        }

        private int GetMaxBetCount()
        {
            //TODO LOCALUSERIN COIN COUNT'UNU RETURN EDECEĞİZ
            return 100;
        }

        private void PlusBet()
        {
            selectedHorseBetArea.SetPlacedBetCount(selectedHorseBetArea.GetPlacedBetCount() + MinusPlusSize);
            OnBetChanged?.Invoke(this, EventArgs.Empty);
        }
        
        private void MinusBet()
        {
            if (selectedHorseBetArea.GetPlacedBetCount() <= 0)
            {
                return;   
            }
            
            selectedHorseBetArea.SetPlacedBetCount(selectedHorseBetArea.GetPlacedBetCount() - MinusPlusSize);
            OnBetChanged?.Invoke(this, EventArgs.Empty);
        }
        
        private void SetBetOfSelected(int amount)
        {
            if (!selectedHorseBetArea) return;

            selectedHorseBetArea.SetPlacedBetCount(amount);
            OnBetChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}