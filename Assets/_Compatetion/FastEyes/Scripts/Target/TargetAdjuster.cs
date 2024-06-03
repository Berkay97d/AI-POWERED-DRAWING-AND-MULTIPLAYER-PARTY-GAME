using System;
using System.Collections;
using System.Collections.Generic;
using MakeItLonger;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


namespace FastEyes
{ 
    public class TargetAdjuster : MonoBehaviour
    {
        [SerializeField] private Target[] targets;
        [SerializeField] private Color[] colors;
        [SerializeField] private float maxMixerNumber;
        [SerializeField] private float minMixerNumber;
        [SerializeField] private float mixWaitTime;
        [SerializeField] private float colorVReduceRatio;
                                        
        public static event EventHandler<Color> OnColorOpened;
        public static TargetAdjuster Instance { get; private set; }
        public event EventHandler OnTargetsBecameShotable;
        
        private Color lastColor;
        private int currentMixerNumer;
        private bool isTargetsShootable = false;
        private Target differentTartget;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            TargetGroupMover.Instance.OnNewTargetGroupSpawned += OnNewTargetGroupSpawned;
            GameController.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            TargetGroupMover.Instance.OnNewTargetGroupSpawned -= OnNewTargetGroupSpawned;
            GameController.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                StartCoroutine(MixerColors());
            }
        }

        private void OnNewTargetGroupSpawned(object sender, EventArgs e)
        {
            StartCoroutine(MixerColors());
        }
        
        private Color PlaceColorToSpots()
        {
            var randomColor = colors[Random.Range(0, colors.Length-1)];
            var differentSpot = targets[Random.Range(0, targets.Length-1)];

            if (randomColor == lastColor)
            {
                return PlaceColorToSpots();
            }

            foreach (var target in targets)
            {
                target.SetIsDifferent(false);
            }
            
            foreach (var spot in targets)
            {
                if (spot != differentSpot)
                {
                    spot.GetComponent<Renderer>().material.color = randomColor;
                }
                else
                {
                    spot.GetComponent<Renderer>().material.color = GetDifferentColor(randomColor);
                    spot.SetIsDifferent(true);
                    differentTartget = spot;
                }
            }

            lastColor = randomColor;
            isTargetsShootable = true;
            OnTargetsBecameShotable?.Invoke(this, EventArgs.Empty);
            return lastColor;
        }

        private IEnumerator MixerColors()
        {
            isTargetsShootable = false;
            var mixNum = Random.Range(minMixerNumber, maxMixerNumber + 1);
            
            GameController.Instance.IncreaseTurnCounter();

            while (currentMixerNumer < mixNum)
            {
                yield return new WaitForSeconds(mixWaitTime);
                foreach (var spot in targets)
                {
                    var randomColor = colors[Random.Range(0, colors.Length-1)];
                    spot.GetComponent<Renderer>().material.color = randomColor;
                }

                currentMixerNumer++;
            }

            currentMixerNumer = 0;
            PlaceColorToSpots();
            OnColorOpened?.Invoke(this, lastColor);
        }

        private Color GetDifferentColor(Color color)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            return Color.HSVToRGB(h, s, v - colorVReduceRatio);
        }

        public bool GetIsShootable()
        {
            return isTargetsShootable;
        }

        public void SetIsShootable(bool isAble)
        {
            isTargetsShootable = isAble;
        }

        public Target GetDifferentTarget()
        {
            return differentTartget;
        }

        public Target GetRandomTarget()
        {
            var rand = Random.Range(0, targets.Length);
            if (targets[rand].GetIsDifferent())
            {
                GetRandomTarget();
            }
            return targets[rand];
        }
        
        
    }
}