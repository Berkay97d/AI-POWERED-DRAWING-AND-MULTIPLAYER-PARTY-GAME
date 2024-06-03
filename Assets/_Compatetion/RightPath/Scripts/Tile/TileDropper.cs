using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;



namespace RightPath
{
    public class TileDropper : MonoBehaviour
    {
        [SerializeField] private Tile myTile;

        private bool isTileTriggered;
        

        private void OnTriggerEnter(Collider other)
        {
            if (isTileTriggered) return;

            isTileTriggered = true;

            if (myTile.GetIsInRightPath())
            {
                LazyCoroutines.WaitForSeconds(.1f, () =>
                {
                    myTile.ShowTileIsSafe();
                });
                return;
            }

            LazyCoroutines.WaitForSeconds(.1f, () =>
            {
                myTile.Drop();
            });
        }
    }    
}

