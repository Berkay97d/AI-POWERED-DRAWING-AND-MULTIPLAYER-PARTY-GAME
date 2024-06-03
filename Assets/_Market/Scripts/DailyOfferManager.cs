using System;
using System.Collections.Generic;
using _CardPacks.Scripts;
using _DatabaseAPI.Scripts;
using _PlayerCustomization.Scripts;
using _TimeAPI.Scripts;
using _UserOperations.Scripts;
using General;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Market.Scripts
{
    public class DailyOfferManager : MonoBehaviour
    {
        [SerializeField] private MarketDailyOffersSO marketDailyOffers;
        [SerializeField] private Transform dailyOffersParent;
        [SerializeField] private TMP_Text resetCounterField;
        [SerializeField] private int resetHour;
        [SerializeField] private int resetMinute;


        private MarketBuyButton[] m_DailyOfferButtons;
        private Coroutine m_Routine;
        private UserData m_UserData;


        private void Awake()
        {
            m_DailyOfferButtons = dailyOffersParent.GetComponentsInChildren<MarketBuyButton>();

            LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged += OnLocalUserDataChanged;
        }

        private void OnDestroy()
        {
            LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged -= OnLocalUserDataChanged;
            
            LazyCoroutines.StopCoroutine(m_Routine);
        }


        private void OnLocalUserDataLoaded(UserData userData)
        {
            SetUserData(userData);
            
            Tick();
            m_Routine = LazyCoroutines.EverySeconds(() => 1f, Tick);
        }

        private void OnLocalUserDataChanged(UserData userData)
        {
            SetUserData(userData);
        }
        
        
        private void Tick()
        {
            TimeAPI.GetCurrentTime(time =>
            {
                var lastDailyOfferTime = m_UserData.lastDailyOfferTime;

                if (lastDailyOfferTime.Equals(TimeData.Zero))
                {
                    GenerateDailyOffers(time);
                    return;
                }

                var resetDateTime = GetResetDateTime();
                var timeLeftToReset = resetDateTime - time.ToDateTime();
                
                SetResetCounter(timeLeftToReset);

                if (timeLeftToReset.TotalMilliseconds <= 0d)
                {
                    Debug.Log("reset");
                    GenerateDailyOffers(time);
                }
            });
        }

        private void SetResetCounter(TimeSpan timeSpan)
        {
            resetCounterField.text = $"New offers will appear in {timeSpan.ToCountdownString()}";
        }

        private void SetUserData(UserData userData)
        {
            m_UserData = userData;
            SetDailyOffers(userData.dailyOffers);
        }

        private void SetDailyOffers(DailyOfferData[] dailyOffers)
        {
            for (var i = 0; i < m_DailyOfferButtons.Length; i++)
            {
                var dailyOffer = i < dailyOffers.Length 
                    ? dailyOffers[i] 
                    : DailyOfferData.None;
                
                m_DailyOfferButtons[i].ApplyDailyOffer(dailyOffer);
            }
        }
        
        private void GenerateDailyOffers(TimeData time)
        {
            LocalUser.GetUserData(userData =>
            {
                userData.lastDailyOfferTime = time;
                userData.dailyOffers = GetDailyOffers(userData.skins);
                LocalUser.SetUserData(userData);
            });
        }

        private DailyOfferData[] GetDailyOffers(SkinData[] exclude)
        {
            const int maxOfferCount = 3;
            
            var offers = marketDailyOffers.GetAllOffers(exclude);
            var count = Mathf.Min(offers.Count, maxOfferCount);
            var indices = new List<int>();

            for (var i = 0; i < count; i++)
            {
                var index = Random.Range(0, offers.Count);

                if (indices.Contains(index))
                {
                    i--;
                    continue;
                }
                
                indices.Add(index);
            }

            var result = new DailyOfferData[count];

            for (var i = 0; i < count; i++)
            {
                var customizationOffer = offers[indices[i]];
                var offer = customizationOffer.ToDailyOfferData();
                result[i] = offer;
            }

            return result;
        }

        private DateTime GetResetDateTime()
        {
            var time = m_UserData.lastDailyOfferTime;
            var dateTime = time.ToDateTime();

            return dateTime
                .AddDays(1)
                .AddHours(resetHour - dateTime.Hour)
                .AddMinutes(resetMinute - dateTime.Minute)
                .AddSeconds(-dateTime.Second)
                .AddMilliseconds(-dateTime.Millisecond);
        }
    }
}