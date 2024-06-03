using System;
using System.Collections;
using System.Collections.Generic;
using _CardPacks.Scripts;
using _DatabaseAPI.Scripts;
using _Painting;
using _TimeAPI.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using General;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class DailyCardGenerationController : MonoBehaviour
{
    private static readonly Duration CommonOpenDuration = new Duration
    {
        hours = 3
    };
        
    private static readonly Duration RareOpenDuration = new Duration
    {
        hours = 8
    };
        
    private static readonly Duration EpicOpenDuration = new Duration
    {
        days = 1
    };
        
    private static readonly Duration LegendaryOpenDuration = new Duration
    {
        days = 2
    };


    [SerializeField] private Transform generationAreaParent;
    [SerializeField] private Button dailyGenerationActivateButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GenerationArea[] generationAreas;
    [SerializeField] private GameObject[] onGenerationStartCloseCanvases;
    [SerializeField] private Transform cardCustomizationCanvas;
    [SerializeField] private Camera mainCamera;
    
    
    
    public event EventHandler OnDailyGenerationActivated;
    public event EventHandler OnDailyGenerationDisable;


    private GenerationArea[] m_GenerationAreas;
    private DailyGenerationData[] m_Datas;
    private Coroutine m_Routine;

    private bool isDailyGenerating = false;
    private CardRarity currentGenerationRarity;
    

    private void Awake()
    {
        m_GenerationAreas = generationAreaParent.GetComponentsInChildren<GenerationArea>(true);

        LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
        LocalUser.OnUserDataChanged += OnLocalUserDataChanged;
    }

    private void Start()
    {
        dailyGenerationActivateButton.onClick.AddListener((() =>
        {
            OnDailyGenerationActivated?.Invoke(this, EventArgs.Empty);
        }));
        
        closeButton.onClick.AddListener(() =>
        {
            OnDailyGenerationDisable.Invoke(this, EventArgs.Empty);
        });

        foreach (var generationArea in generationAreas)
        {
            generationArea.OnGenerateButtonClick += OnGenerateButtonClick;
        }
        
        PaintManager.OnPaintingComplete += OnPaintingComplete;
    }

    private void OnPaintingComplete(PaintingData obj)
    {
        if (isDailyGenerating)
        {
            TakePlayerFreeGeneration();
            isDailyGenerating = false;
        }
    }

    private void OnDestroy()
    {
        LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
        LocalUser.OnUserDataChanged -= OnLocalUserDataChanged;
        PaintManager.OnPaintingComplete -= OnPaintingComplete;
        
        foreach (var generationArea in generationAreas)
        {
            generationArea.OnGenerateButtonClick -= OnGenerateButtonClick;
        }
        
        LazyCoroutines.StopCoroutine(m_Routine);
    }

    private void TakePlayerFreeGeneration()
    {
        LocalUser.ResetDailyGeneration(currentGenerationRarity);
    }

    private void OnGenerateButtonClick(object sender, CardRarity e)
    {
        mainCamera.orthographic = true;
        
        SetIsDailyGenerating(true);
        currentGenerationRarity = e;
        
        foreach (var canvas in onGenerationStartCloseCanvases)
        {
            canvas.transform.localScale = Vector3.zero;
        }
        
        cardCustomizationCanvas.transform.localScale = Vector3.zero;
        cardCustomizationCanvas.gameObject.SetActive(true);
        cardCustomizationCanvas.DOScale(Vector3.one, .35f).SetEase(Ease.OutBack);
        
        foreach (var canvas in onGenerationStartCloseCanvases)
        {
            canvas.SetActive(false);
            canvas.transform.localScale = Vector3.one;
        }
    }

    private void OnLocalUserDataLoaded(UserData userData)
    {
        SetData(userData.dailyGenerations);
        
        Tick();
        m_Routine = LazyCoroutines.EverySeconds(() => 1f, Tick);
    }
    
    private void OnLocalUserDataChanged(UserData userData)
    {
        SetData(userData.dailyGenerations);
    }


    private void SetData(DailyGenerationData[] data)
    {
        m_Datas = data;
    }

    private void Tick()
    {
        TimeAPI.GetCurrentTime(SetCurrentTime);
    }

    private void SetCurrentTime(TimeData time)
    {
        for (var i = 0; i < m_GenerationAreas.Length; i++)
        {
            var duration = i switch
            {
                0 => CommonOpenDuration,
                1 => RareOpenDuration,
                2 => EpicOpenDuration,
                3 => LegendaryOpenDuration
            };
            var startTime = m_Datas[i].startTime;
            var elapsedTime = time - startTime;
            var timeLeft = duration.ToTimeSpan() - elapsedTime;
            
            m_GenerationAreas[i].SetTimeLeft(timeLeft);
        }
    }

    public bool GetIsDailyGenerating()
    {
        return isDailyGenerating;
    }

    public void SetIsDailyGenerating(bool isDaily)
    {
        isDailyGenerating = isDaily;
    }

    public CardRarity GetCurrentGenerationRarity()
    {
        return currentGenerationRarity;
    }

    public GenerationArea[] GetGenerationAreas()
    {
        return generationAreas;
    }
    
}
