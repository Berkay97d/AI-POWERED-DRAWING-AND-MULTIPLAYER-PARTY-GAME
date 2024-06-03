using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CompetationBettingController : MonoBehaviour
{
    [SerializeField] private EnterTest_Script matchmakingObj;
    [SerializeField] private Transform bettingMainTransform;
    [SerializeField] private Transform compPageMainTransform;
    [SerializeField] private GameObject upInfo;
    [SerializeField] private Button doneButton;
    [SerializeField] private Button backButton;
    [SerializeField] private CompetationMenuController competationMenuController;
    [SerializeField] private AiBetInitilazor aiBetInitilazor;
    [SerializeField] private CompetationBettingCardInitilazor competationBettingCardInitilazor;
    
    
    public static bool isBettingPageActive = false;
   

   
    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            competationBettingCardInitilazor.TakeAllCardsBackFromBet();
            GoBackToCompPage();
        });
        doneButton.onClick.AddListener(GetMatchMaking);
        doneButton.interactable = false;
        competationBettingCardInitilazor.OnBetCardsChanged += OnBetCardsChanged;
    }

    private void OnDestroy()
    {
        competationBettingCardInitilazor.OnBetCardsChanged -= OnBetCardsChanged;
    }

    private void OnBetCardsChanged(object sender, int e)
    {
        if (e <= 0)
        {
            doneButton.interactable = false;
            return;
        }
        
        doneButton.interactable = true;
        
    }

    private void GetMatchMaking()
    {
        isBettingPageActive = false;
        
        bettingMainTransform.gameObject.SetActive(false);
        matchmakingObj.gameObject.SetActive(true);
        matchmakingObj.InitMatchMaking(competationMenuController.GetPlayerCount(), competationBettingCardInitilazor.GetBet());
    }
    
    private void GoBackToCompPage()
    {
        isBettingPageActive = false;
        
        bettingMainTransform.gameObject.SetActive(false);
        compPageMainTransform.gameObject.SetActive(true);
        compPageMainTransform.DOScale(Vector3.one, 0.35f);
        upInfo.SetActive(true);
    }
}
