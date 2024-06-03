using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DontFall
{


    public class PatternList : MonoBehaviour
    {
        [SerializeField] private Tile[] pattern1;
        [SerializeField] private Tile[] pattern2;
        [SerializeField] private Tile[] pattern3;
        [SerializeField] private Tile[] pattern4;
        [SerializeField] private Tile[] pattern5;
        [SerializeField] private Tile[] pattern6;
        [SerializeField] private Tile[] pattern7;
        [SerializeField] private Tile[] pattern8;
        [SerializeField] private Tile[] pattern9;
        [SerializeField] private Tile[] pattern10;
        [SerializeField] private Tile[] pattern11;
        [SerializeField] private Tile[] pattern12;
        [SerializeField] private Tile[] pattern13;
        [SerializeField] private Tile[] pattern14;
        [SerializeField] private Tile[] pattern15;
        [SerializeField] private Tile[] pattern16;
        [SerializeField] private Tile[] pattern17;
        [SerializeField] private Tile[] pattern18;
        

        private List<Tile[]> allPatterns = new List<Tile[]>();

        private void Start()
        {
            allPatterns.Add(pattern1);
            allPatterns.Add(pattern2);
            allPatterns.Add(pattern3);
            allPatterns.Add(pattern4);
            allPatterns.Add(pattern5);
            allPatterns.Add(pattern6);
            allPatterns.Add(pattern7);
            allPatterns.Add(pattern9);
            allPatterns.Add(pattern10);
            allPatterns.Add(pattern11);
            allPatterns.Add(pattern12);
            allPatterns.Add(pattern13);
            allPatterns.Add(pattern14);
            allPatterns.Add(pattern15);
            allPatterns.Add(pattern16);
            allPatterns.Add(pattern17);
            allPatterns.Add(pattern18);
            allPatterns.Add(pattern8);
            
        }

        public Tile[] GetRandomPattern()
        {
            int rand = Random.Range(0, allPatterns.Count);

            return allPatterns[rand];
        }
    }
}