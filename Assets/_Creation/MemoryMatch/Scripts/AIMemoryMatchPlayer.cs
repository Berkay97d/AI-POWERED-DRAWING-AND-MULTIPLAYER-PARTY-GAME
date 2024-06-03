using System.Collections;
using General;
using UnityEngine;

namespace _Compatetion.MemoryMatch
{
    public class AIMemoryMatchPlayer : MemoryMatchPlayer
    {
        private const int NullTile = -1;
        private const int UnClickableTile = -2;
        
        
        private int[,] m_Vision;


        public AIMemoryMatchPlayer(string name) : base(name) {}


        public void Awake()
        {
            game.OnAnyTileClicked += OnAnyTileClicked;
            
            CreateVision();
        }


        private void OnAnyTileClicked(MemoryMatchTile tile)
        {
            if (!ShouldRemember()) return;
            
            var x = tile.GetGridPositionX();
            var y = tile.GetGridPositionY();

            m_Vision[x, y] = tile.GetPairIndex();
        }
        
        
        public override void MakeMove()
        {
            LazyCoroutines.StartCoroutine(Routine());

            
            IEnumerator Routine()
            {
                yield return new WaitForSeconds(0.5f);

                UpdateVision();

                if (!TryGetPairFromVision(out var a, out var b))
                {
                    GetRandomPair(out a, out b);
                }
                
                a.Click();
                yield return new WaitForSeconds(0.5f);
                b.Click();
            }
        }


        private bool ShouldRemember()
        {
            const float memoryRate = 0.5f;

            return Random.Range(0f, 1f) <= memoryRate;
        }
        
        private bool TryGetPairFromVision(out MemoryMatchTile a, out MemoryMatchTile b)
        {
            var column = game.GetColumn();
            var row = game.GetRow();
            
            for (var i = 0; i < column; i++)
            {
                for (var j = 0; j < row; j++)
                {
                    for (var k = i + 1; k < column; k++)
                    {
                        for (var l = j; l < row; l++)
                        {
                            a = game.GetTile(i, j);
                            b = game.GetTile(k, l);

                            var aPairIndex = m_Vision[i, j];
                            var bPairIndex = m_Vision[k, l];

                            if (aPairIndex is NullTile or UnClickableTile) continue;
                            
                            if (bPairIndex is NullTile or UnClickableTile) continue;

                            if (aPairIndex == bPairIndex) return true;
                        }
                    }
                }
            }

            a = null;
            b = null;
            
            return false;
        }

        private void GetRandomPair(out MemoryMatchTile a, out MemoryMatchTile b)
        {
            var tiles = game.GetAllClickableTiles();

            a = tiles[Random.Range(0, tiles.Count)];
            tiles.Remove(a);
            b = tiles[Random.Range(0, tiles.Count)];
        }

        private void UpdateVision()
        {
            var column = game.GetColumn();
            var row = game.GetRow();
            
            for (var i = 0; i < column; i++)
            {
                for (var j = 0; j < row; j++)
                {
                    if (game.GetTile(i, j).GetIsClickable()) continue;

                    m_Vision[i, j] = UnClickableTile;
                }
            }
        }
        
        private void CreateVision()
        {
            var column = game.GetColumn();
            var row = game.GetRow();
            
            m_Vision = new int[column, row];

            for (var i = 0; i < column; i++)
            {
                for (var j = 0; j < row; j++)
                {
                    m_Vision[i, j] = NullTile;
                }
            }
        }

        private void PrintVision()
        {
            var output = "";
            var row = game.GetRow();
            var column = game.GetColumn();

            for (var j = row - 1; j >= 0; j--)
            {
                for (var i = 0; i < column; i++)
                {
                    if (i == column - 1)
                    {
                        output += $"{m_Vision[i, j]}\n";
                        continue;
                    }

                    output += $"{m_Vision[i, j]}|";
                }
            }

            Debug.Log(output);
        }
    }
}