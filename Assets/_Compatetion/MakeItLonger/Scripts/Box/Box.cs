using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace MakeItLonger
{
    public class OnBoxReleasedEventArgs : EventArgs
    {
        public Grid DropGrid;
        public Contester contester;
        public Box box;

    }
    
    public class Box : MonoBehaviour
    {
        [SerializeField] private float moveWaitTime;
        [SerializeField] private float dropSpeed;
        private GameController gameController;
        
        public static event EventHandler OnBoxesDestroy;
        
        private float moveTime;
        private Contester contester;
        private ContesterGrid contesterGrid;
        private GridDirection gridDirection;
        private float time;
        private Grid currentGrid;
        private bool isMoving = false;
        private Tween moveTween;


        private void Awake()
        {
            gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
            gameController.OnBoxInit += OnBoxInit;
            moveTime = moveWaitTime;
        }

        private void Start()
        {
            contester.OnBoxReleased += InternalOnBoxReleased;
            gameController.OnRoundOver += OnRoundOver;
        }
        
        private void OnRoundOver(object sender, OnRoundOverArgs e)
        {
            moveTween.Kill();
            DestroyBox();
        }

        private void DestroyBox()
        {
            StartCoroutine(InnerRoutine());
            
            IEnumerator InnerRoutine()
            {
                transform.DOScale(Vector3.zero, .5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    var childRenderers = transform.GetComponentsInChildren<MeshRenderer>();
                    foreach (var renderer in childRenderers)
                    {
                        renderer.enabled = false;
                    }
                });

                yield return new WaitForSeconds(1f);
                OnBoxesDestroy?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            gameController.OnBoxInit -= OnBoxInit;
            contester.OnBoxReleased -= InternalOnBoxReleased;
            gameController.OnRoundOver -= OnRoundOver;
        }
        
        private void Update()
        {
            Move();
        }

        private void OnBoxInit(object sender, OnBoxInitEventArgs e)
        {
            if (e.initedBox == this)
            {
                gridDirection = contesterGrid.GetNextBoxesGridDirection();
                SetIsMoving(true);
            }
        }
        
        private void InternalOnBoxReleased(object sender, OnBoxReleasedEventArgs e)
        {
            if (e.box == this)
            {
                SetIsMoving(false);
                Drop(e.DropGrid);
            }
        }
        
        public void RaiseOnBoxReleased(Box box, Contester cont)
        {
            OnBoxReleasedEventArgs eventArgs = new OnBoxReleasedEventArgs();
            var contGrid = cont.GetContesterGrid();
            var boxGridPos = contGrid.GetGridFromWorld(box.transform.position);
            eventArgs.DropGrid = cont.GetContesterGrid().CalculateBoxDropGrid(boxGridPos.xIndex, boxGridPos.yIndex, contGrid);
            eventArgs.box = box;
            eventArgs.contester = cont;
            contester.RaiseOnBoxReleased(eventArgs);
        }

        private void Move()
        {
            if (!isMoving) return;

            time += Time.deltaTime;

            
            
            if (time > moveTime)
            {
                
                time = 0;
                if (currentGrid.xIndex == 0 && gridDirection == GridDirection.Left || currentGrid.xIndex == 4 && gridDirection == GridDirection.Right)
                {
                    ChangeDirection();
                }
                var targetGrid = contesterGrid.GetNextGrid(currentGrid, gridDirection); 
                SetBoxPositionFromGrid(targetGrid);
                
            }
            
        }

        public void Drop(Grid grid)
        {
            moveTween = transform.DOMove(grid.transform.position, dropSpeed).SetSpeedBased().OnComplete((() =>
            {
                if (contester is Player)
                {
                    MakeItLongerSoundInitilazor.PlayBoxDropSound();
                }
            }));
        }

        private void ChangeDirection()
        {
            gridDirection = gridDirection == GridDirection.Left ? GridDirection.Right : GridDirection.Left;
        }
        
        public void SetBoxPositionFromGrid(Grid grid)
        {
            currentGrid = grid;
            transform.position = grid.transform.position;
        }
        
        private void SetIsMoving(bool isMove)
        {
            isMoving = isMove;
        }
        
        public void SetContesterGrid(ContesterGrid cGrid)
        {
            contesterGrid = cGrid;
        }

        public void SetContester(Contester con)
        {
            contester = con;
        }

        public Contester GetContester()
        {
            return contester;
        }

        public void IncreaseSpeed(int level)
        {
            for (int i = 0; i < level; i++)
            {
                moveTime /= 1.2f;
            }
        }
        
    }
}