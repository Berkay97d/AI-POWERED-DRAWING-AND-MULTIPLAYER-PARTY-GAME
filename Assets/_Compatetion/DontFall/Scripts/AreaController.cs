using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DontFall
{
    public class AreaController : MonoBehaviour
    {
        
        [SerializeField] private float patternHappenTime;
        [SerializeField] private float warningFinishTime;
        [SerializeField] private float recoveryHappenTime;
        [SerializeField] private Tile[] allTiles;
        
        public static AreaController Instance { get; private set; }
        public event EventHandler OnWarningShake;
        public event EventHandler OnWarningDone;
        
        private PatternList patternList;
        private List<Tile> dangerZone = new List<Tile>();
        private float patternTime = 0;
        private float warningTime = 0;
        private float recoveryTime = 0;
        private bool isWarningShakeDone = false;
        private bool isPatternDecided = false;


        private void Awake()
        {
            Instance = this;
            patternList = GetComponent<PatternList>();
        }

        private void Update()
        {
            if (GameController.Instance.GetIsGameOver())
            {
                return;
            }
            
            DoPattern();
        }
        
        private void DoPattern()
        {
            if (IsPatternTimeCome())
            {
                if (!isPatternDecided)
                {
                    dangerZone = GetPattern();
                }

                DoWarningShake(dangerZone);
                
                if (IsWarningTimeCome())
                {
                    foreach (var stone in dangerZone)
                    {
                        stone.GoDown();
                    }

                    DoRecover(dangerZone);
                }
            }
        }

        private void DoRecover(List<Tile> dangerZone)
        {
            recoveryTime += Time.deltaTime;
            if (recoveryTime >= recoveryHappenTime)
            {
                foreach (var stone in dangerZone)
                {
                    stone.GoUp();
                    stone.SetIsInDangerZone(false);
                }
                
                ResetRecoverTimer();
                ResetWarningTimer();
                ResetPatternTimer();
                SetIsWarningShakeDone(false);
                isPatternDecided = false;
                StartCoroutine(RaiseWarningDoneByTime());
            }
        }

        private IEnumerator RaiseWarningDoneByTime()
        {
            yield return new WaitForSeconds(1f);
            
            OnWarningDone?.Invoke(this,EventArgs.Empty);
        }

        private void DoWarningShake(List<Tile> dangerZone)
        {
            if (isWarningShakeDone)
            {
                return;
            }
            
            foreach (var stone in dangerZone)
            {
                stone.transform.DOShakePosition(1.5f, Vector3.one / 20);
            }
            OnWarningShake?.Invoke(this,EventArgs.Empty);
            SetIsWarningShakeDone(true);
        }

        private List<Tile> GetPattern()
        {
            List<Tile> pattern = new List<Tile>();
            var randomPattern = patternList.GetRandomPattern();
            
            foreach (var tile in randomPattern)
            {
                pattern.Add(tile);
                tile.SetIsInDangerZone(true);
            }

            isPatternDecided = true;
            
            return pattern;
        }

        private bool IsPatternTimeCome()
        {
            patternTime += Time.deltaTime;

            if (patternTime >= patternHappenTime)
            {
                return true;
            }

            return false;
        }

        private bool IsWarningTimeCome()
        {
            warningTime += Time.deltaTime;
            
            if (warningTime >= warningFinishTime)
            {
                return true;
            }

            return false;
        }
        
        private void SetIsWarningShakeDone(bool isDone)
        {
            isWarningShakeDone = isDone;
        }

        private void ResetPatternTimer()
        {
            patternTime = 0;
        }

        private void ResetWarningTimer()
        {
            warningTime = 0;
        }

        private void ResetRecoverTimer()
        {
            recoveryTime = 0;
        }

        public Tile GetRandomTile()
        {
            return allTiles[Random.Range(0, allTiles.Length - 1)];
        }

        public Tile[] GetAllTiles()
        {
            return allTiles;
        }
    }
}