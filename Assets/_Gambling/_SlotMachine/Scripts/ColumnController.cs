using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SlotMachine
{
    public class ColumnController : MonoBehaviour
    {
        [SerializeField] private Icon[] icons;
        
        private float columnSpeed;
        private float stopTime;
        private List<Icon> iconList = new List<Icon>();
        private float iconGap = 0.4f;
        private float time = 0;
        private Vector3 startPos;
        private Icon middleIcon;
        private bool isMachineStartWorking = false;
        private bool isColumnStoped = false;

        private void Awake()
        {
            startPos = transform.position;

            SetRandomSpeed();
            SetRandomStopTime();
        }

        private void Start()
        {
            foreach (var icon in icons)
            {
                iconList.Add(icon);
            }
            
            GameController.Instance.OnGameStarted += OnGameStarted;
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            isMachineStartWorking = true;
        }

        private void MoveLowestToUppest()
        {
            var a = iconList[0];
            a.GoUppest(iconList[^1].transform.position, iconGap);
            iconList.Remove(a);
            iconList.Add(a);
        }

        private void MoveMiddleIconToMiddle(Icon middle)
        {
            bool moveUp = middle.transform.position.y < 0;

            if (moveUp)
            {
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + Mathf.Abs(middle.transform.position.y), transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x,
                    transform.position.y - Mathf.Abs(middle.transform.position.y), transform.position.z);
            }
        }

        private void Update()
        {
            if (!isMachineStartWorking) return;

            time += Time.deltaTime;
            if (time > stopTime)
            {
                columnSpeed = 0;
                FindMiddleIcon(icons);
                MoveMiddleIconToMiddle(middleIcon);
                isColumnStoped = true;
            }

            transform.Translate(Vector3.down * columnSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - startPos.y) >= iconGap)
            {
                startPos = transform.position;
                MoveLowestToUppest();
            }
        }

        private void FindMiddleIcon(Icon[] iconArr)
        {
            float closest = 100000f;

            foreach (var icon in iconArr)
            {
                var a = Vector3.Distance(Vector3.zero, icon.transform.position);
                if (a < closest)
                {
                    closest = a;
                    middleIcon = icon;
                }

            }
        }

        private void SetRandomSpeed()
        {
            columnSpeed = Random.Range(1, 1.5f);
        }

        private void SetRandomStopTime()
        {
            stopTime = Random.Range(2f, 5f);
        }

        public Icon GetMiddleIcon()
        {
            return middleIcon;
        }

        public bool GetIsColumnStopped()
        {
            return isColumnStoped;
        }
    }
}