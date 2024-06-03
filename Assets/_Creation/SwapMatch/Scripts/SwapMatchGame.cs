using System;
using System.Collections.Generic;
using System.Linq;
using ImageUtils;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Creation.SwapMatch.Scripts
{
    public class SwapMatchGame
    {
        private readonly string m_Name;
        private readonly Action m_OnComplete;
        private readonly SwapMatchTile m_TilePrefab;
        private readonly Texture2D m_Texture;
        private readonly int m_Row;
        private readonly int m_Column;
        private readonly Vector3 m_Offset;
        private readonly float m_Scale;
        private readonly SwapMatchTile[,] m_Tiles;
        private Transform m_Parent;
        private SwapMatchTile m_DiscardedTile;
        private bool m_AllowInput;


        public SwapMatchGame(
            string name,
            Action onComplete,
            SwapMatchTile tilePrefab,
            Texture2D texture,
            int row,
            int column,
            Vector3 offset = default,
            float scale = 1f)
        {
            m_Name = name;
            m_OnComplete = onComplete;
            m_TilePrefab = tilePrefab;
            m_Texture = texture;
            m_Row = row;
            m_Column = column;
            m_Offset = offset;
            m_Scale = scale;
            m_Tiles = new SwapMatchTile[column, row];
            
            Initialize();
            DiscardRightBottomTile();
            ScrambleTiles();
        }


        private void OnAnyTileSwap(SwapMatchTile a, SwapMatchTile b)
        {
            if (IsAllTilesInCorrectPosition())
            {
                OnCompleted();
            }
        }

        private void OnCompleted()
        {
            HideNumbers();
            ReturnDiscardedTile();
            m_OnComplete?.Invoke();
        }
        

        public bool GetAllowInput()
        {
            return m_AllowInput;
        }
        
        public void SetAllowInput(bool value)
        {
            m_AllowInput = value;
        }

        public SwapMatchTile GetTile(int x, int y)
        {
            return IsValidTilePosition(x, y)
                ? m_Tiles[x, y]
                : null;
        }

        public SwapMatchTile GetDiscardedTile()
        {
            return m_DiscardedTile;
        }

        public void SwapTiles(SwapMatchTile a, SwapMatchTile b)
        {
            var position = a.GetPosition();
            var otherPosition = b.GetPosition();
            
            GetTilePosition(a, out var x, out var y);
            GetTilePosition(b, out var otherX, out var otherY);

            if (a.IsDiscarded())
            {
                a.SetPosition(otherPosition);
            }
            else
            {
                a.MoveToPosition(otherPosition);
            }

            if (b.IsDiscarded())
            {
                b.SetPosition(position);
            }
            else
            {
                b.MoveToPosition(position);
            }
            
            m_Tiles[x, y] = b;
            m_Tiles[otherX, otherY] = a;
            
            OnAnyTileSwap(a, b);
        }

        public void InstantSwapTiles(SwapMatchTile a, SwapMatchTile b)
        {
            var position = a.GetPosition();
            var otherPosition = b.GetPosition();
            
            GetTilePosition(a, out var x, out var y);
            GetTilePosition(b, out var otherX, out var otherY);
                    
            a.SetPosition(otherPosition);
            b.SetPosition(position);
            m_Tiles[x, y] = b;
            m_Tiles[otherX, otherY] = a;
        }

        public IReadOnlyList<SwapMatchTile> GetNeighbourTiles(SwapMatchTile tile)
        {
            var neighbourTiles = new List<SwapMatchTile>();
            
            GetTilePosition(tile, out var x, out var y);

            var leftTile = GetTile(x - 1, y);
            var rightTile = GetTile(x + 1, y);
            var upTile = GetTile(x, y + 1);
            var downTile = GetTile(x, y - 1);

            if (leftTile)
            {
                neighbourTiles.Add(leftTile);
            }

            if (rightTile)
            {
                neighbourTiles.Add(rightTile);
            }

            if (upTile)
            {
                neighbourTiles.Add(upTile);
            }

            if (downTile)
            {
                neighbourTiles.Add(downTile);
            }

            return neighbourTiles;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            var localPosition = GetLocalPosition(x, y);
            localPosition *= m_Scale;
            return localPosition + m_Offset;
        }

        public Vector3 GetLocalPosition(int x, int y)
        {
            const float pixelPerUnit = 100f;
            
            var totalSizeX = m_Texture.width / pixelPerUnit;
            var totalSizeY = m_Texture.height / pixelPerUnit;
            var sizeX = totalSizeX / m_Column;
            var sizeY = totalSizeY / m_Row;
            var centerOffset = new Vector3(totalSizeX - sizeX, totalSizeY - sizeY) * -0.5f;
            var positionX = x * sizeX;
            var positionY = y * sizeY;
            
            return new Vector3(positionX, positionY) + centerOffset;
        }

        public void GetTilePosition(SwapMatchTile tile, out int x, out int y)
        {
            x = -1;
            y = -1;

            for (var i = 0; i < m_Column; i++)
            {
                for (var j = 0; j < m_Row; j++)
                {
                    if (m_Tiles[i, j] != tile) continue;

                    x = i;
                    y = j;
                }
            }
        }
        
        public void SmoothScrambleTiles(int iteration)
        {
            for (var i = 0; i < iteration; i++)
            {
                var neighbourTiles = GetNeighbourTiles(m_DiscardedTile);
                var otherTile = neighbourTiles[Random.Range(0, neighbourTiles.Count)];

                InstantSwapTiles(m_DiscardedTile, otherTile);
            }

            var positions = new List<Vector3>();
            var index = 0;

            foreach (var tile in m_Tiles)
            {
                GetTilePosition(tile, out var x, out var y);
                var position = GetWorldPosition(x, y);
                positions.Add(position);

                var i = index;
                tile.MoveToPosition(m_Parent.position, () =>
                {
                    tile.MoveToPosition(positions[i]);
                });

                index += 1;
            }
        }


        private void Initialize()
        {
            m_Parent = new GameObject(m_Name).transform;
            var sprites = ImageSplitter.Split(m_Texture, m_Row, m_Column);
            var counter = 0;

            var backgroundSprite = new GameObject("Background")
                .AddComponent<SpriteRenderer>();
            backgroundSprite.sprite = m_Texture.ToSprite();
            backgroundSprite.color = new Color(0.2f, 0.2f, 0.2f);
            backgroundSprite.transform.SetParent(m_Parent);
            backgroundSprite.transform.position += Vector3.forward * 0.01f;

            for (var y = 0; y < m_Row; y++)
            {
                for (var x = 0; x < m_Column; x++)
                {
                    var sprite = sprites[counter];
                    var tile = Object.Instantiate(m_TilePrefab, m_Parent);
                    var position = GetLocalPosition(x, y);

                    m_Tiles[x, y] = tile;
                    tile.name = $"({x}, {y})";
                    tile.SetGame(this);
                    tile.SetInitialPosition(x, y);
                    tile.SetPosition(position);
                    tile.SetSprite(sprite);
                    tile.SetNumber((m_Column * m_Row) - ((m_Row - x - 1) + y * m_Row));
                    tile.MatchColliderWithSpriteSize(sprite);
                    
                    counter += 1;
                }
            }

            m_Parent.position += m_Offset;
            m_Parent.localScale = Vector3.one * m_Scale;
        }

        private void ScrambleTiles()
        {
            const int iteration = 200;

            for (var i = 0; i < iteration; i++)
            {
                var neighbourTiles = GetNeighbourTiles(m_DiscardedTile);
                var otherTile = neighbourTiles[Random.Range(0, neighbourTiles.Count)];

                InstantSwapTiles(m_DiscardedTile, otherTile);
            }
        }

        private void DiscardRightBottomTile()
        {
            var tile = m_Tiles[m_Column - 1, 0];
            
            tile.Discard();
            m_DiscardedTile = tile;
        }
        
        private void DiscardRandomTile()
        {
            var x = Random.Range(0, m_Column);
            var y = Random.Range(0, m_Row);
            var tile = m_Tiles[x, y];

            tile.Discard();
            m_DiscardedTile = tile;
        }

        private void HideNumbers()
        {
            foreach (var tile in m_Tiles)
            {
                tile.HideNumber();
            }
        }
        
        private void ReturnDiscardedTile()
        {
            m_DiscardedTile.Return();
        }

        private bool IsAllTilesInCorrectPosition()
        {
            for (var y = 0; y < m_Row; y++)
            {
                for (var x = 0; x < m_Column; x++)
                {
                    if (!m_Tiles[x, y].IsInitialPosition(x, y)) return false;
                }
            }

            return true;
        }

        private bool IsValidTilePosition(int x, int y)
        {
            if (x < 0) return false;

            if (x >= m_Column) return false;

            if (y < 0) return false;

            if (y >= m_Row) return false;

            return true;
        }
    }
}