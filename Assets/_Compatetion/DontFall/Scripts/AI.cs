using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DontFall
{
    public enum AILevel
    {
        Low,
        LowMid,
        Mid,
        MidGood,
        Good
    }
    
    public class AI : Contester
    {
        [SerializeField] private AILevel aiLevel;
        [SerializeField] private float speed;
        [SerializeField] private float randomMoveDirectionChangeTime;
        [SerializeField] private KillDetector kill;
        
        
        private bool isDangerActive = false;
        private bool isMoving = false;
        private float moveTime;
        private Tile targetTile;
        private AIMover aiMover;
        private bool isRealPlayerDead;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            AreaController.Instance.OnWarningShake += OnWarningShake;
            AreaController.Instance.OnWarningDone += OnWarningDone;
            kill.OnRealPlayerDied += OnRealPlayerDied;
            
            aiMover = GetComponent<AIMover>();
            aiMover.SetRigidbody(rb);
            aiMover.SetSpeeed(speed);
        }

        private void OnDestroy()
        {
            AreaController.Instance.OnWarningShake -= OnWarningShake;
            AreaController.Instance.OnWarningDone -= OnWarningDone;
            kill.OnRealPlayerDied -= OnRealPlayerDied;
        }

        private void OnRealPlayerDied(object sender, EventArgs e)
        {
            isRealPlayerDead = true;
        }

        protected override void Update()
        {
            base.Update();
            
            if (isDangerActive) return;
            if (isDead) return;
            if (!GetIsGrounded()) return;

            if (!isMoving)
            {
                MoveTartgetTile();
            }
            
        }
        
        private void MoveTartgetTile()
        {
            StartCoroutine(InnerRoutine());
            
            IEnumerator InnerRoutine()
            {
                while (!isMoving)
                {
                    isMoving = true;
                    var tiles = AreaController.Instance.GetAllTiles();
                    var randomTile = tiles[Random.Range(0, tiles.Length)];
                    aiMover.SetTarget(randomTile.transform.position);
                    yield return new WaitForSeconds(randomMoveDirectionChangeTime);
                    isMoving = false;
                    yield break;

                }
            }
        }

        private void OnWarningShake(object sender, EventArgs e)
        {
            SetIsDangerActive(true);
            MoveOnWarning();
        }
        
        private void OnWarningDone(object sender, EventArgs e)
        {
            StartCoroutine(InnerRoutine());
            
            IEnumerator InnerRoutine()
            {
                yield return new WaitForSeconds(.7f);
                SetIsDangerActive(false);
            }
        }
        
        private void MoveOnWarning()
        {
            if (GetSuccessOrNot())
            {
                MoveTrueTile();
                return;
            }
            
            MoveFalseTile();
        }
        
        private void MoveFalseTile()
        {
            var tiles = AreaController.Instance.GetAllTiles();
            var randomTile = tiles[Random.Range(0, tiles.Length)];
           
            if (randomTile.GetIsInDangerZone()) 
            {
                aiMover.SetTarget(randomTile.transform.position);
                return;
            }
            
            MoveFalseTile();
        }
        
        private void MoveTrueTile()
        {
            var tiles = AreaController.Instance.GetAllTiles();
            var randomTile = tiles[Random.Range(0, tiles.Length)];
           
            if (randomTile.GetIsInDangerZone())
            {
                MoveTrueTile();
                return;
            }
            
            aiMover.SetTarget(randomTile.transform.position);
        }
        
        private bool GetSuccessOrNot()
        {
            var randomNumber = Random.Range(0, 100);

            if (!isRealPlayerDead)
            {
                switch (aiLevel)
                {
                    case AILevel.Low:
                        return randomNumber < 60;

                    case AILevel.LowMid: 
                        return randomNumber < 70;
                
                    case AILevel.Mid: 
                        return randomNumber < 75;
                
                    case AILevel.MidGood: 
                        return randomNumber < 85;
                
                    case AILevel.Good: 
                        return randomNumber < 90;
                }

                return false;
            }

            switch (aiLevel)
            {
                case AILevel.Low:
                    return randomNumber < 40;

                case AILevel.LowMid: 
                    return randomNumber < 50;
                
                case AILevel.Mid: 
                    return randomNumber < 55;
                
                case AILevel.MidGood: 
                    return randomNumber < 65;
                
                case AILevel.Good: 
                    return randomNumber < 70;
            }

            return false;
        }

        private void SetIsDangerActive(bool isActive)
        {
            isDangerActive = isActive;
        }

        public Vector3 GetVelocity()
        {
            return rb.velocity;
        }
        
        
        
    }
}