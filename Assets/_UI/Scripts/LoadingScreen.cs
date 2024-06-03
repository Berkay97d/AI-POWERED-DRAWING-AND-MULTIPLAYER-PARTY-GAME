using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private static readonly string[] dotPhases = {"", ".", "..", "...","", ".", "..", "...","", ".", "..", "...","", ".", "..", "...","", ".", "..", "..."};


    [SerializeField] private AnimationCurve fillCurve;
    [SerializeField] private float fillDuration;
    [SerializeField] private Image imageToFill;
    [SerializeField] private TMP_Text dotText;
    [SerializeField] private GameObject canvas;
    [SerializeField] private float updateInterval; 
    
    
    private int currentDotPhase;
    private float fillAmount;
    private float timer;
    private float dotTimer;
    private bool localUserLodaded;


    private void Awake()
    {
        LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
    }

    private void Start()
    {
        fillAmount = 0f;
        dotText.text = dotPhases[0];
        imageToFill.fillAmount = fillAmount;
        canvas.SetActive(true);
    }

    private void OnDestroy()
    {
        LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
    }

    private void Update()
    {
        if (!UserRegisterer.IsRegistered())
        {
            return;
        }
        
        dotTimer += Time.deltaTime;

        if (dotTimer >= updateInterval)
        {
            dotTimer = 0;
            currentDotPhase = (currentDotPhase + 1) % dotPhases.Length;
            dotText.text = dotPhases[currentDotPhase];
        }

        if (fillAmount >= 0.75f && !localUserLodaded)
        {
            return;
        }
        
        if (fillAmount >= 1f)
        {
            canvas.SetActive(false);
        }
        
        timer += Time.deltaTime;

        var t = Mathf.Clamp01(timer / fillDuration);
        fillAmount = fillCurve.Evaluate(t);
        imageToFill.fillAmount = fillAmount;
    }


    private void OnLocalUserDataLoaded(UserData userData)
    {
        localUserLodaded = true;
    }
}
