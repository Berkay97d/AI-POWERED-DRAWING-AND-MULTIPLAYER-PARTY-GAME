using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RightPath
{
    
    public class GameController : MonoBehaviour
    {
        #region path options
        
        [SerializeField] private Tile[] possibleRightPath1;
        [SerializeField] private Tile[] possibleRightPath2;
        [SerializeField] private Tile[] possibleRightPath3;
        [SerializeField] private Tile[] possibleRightPath4;
        [SerializeField] private Tile[] possibleRightPath5;

        private readonly List<Tile[]> allPossibleRightPats = new();
        
        #endregion
        

        private void Awake()
        {
            InitializeRightPath();
        }

        private void InitializeRightPath()
        {
            var rightPath = SelectRandomRightPath();

            foreach (var tile in rightPath)
            {
                tile.MarkAsRightPath();
            }
        }

        private Tile[] SelectRandomRightPath()
        {
            allPossibleRightPats.Add(possibleRightPath1);
            allPossibleRightPats.Add(possibleRightPath2);
            allPossibleRightPats.Add(possibleRightPath3);
            allPossibleRightPats.Add(possibleRightPath4);
            allPossibleRightPats.Add(possibleRightPath5);

            return allPossibleRightPats[Random.Range(0, allPossibleRightPats.Count)];
        }
        
    }
}