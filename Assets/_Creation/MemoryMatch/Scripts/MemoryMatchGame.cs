using System;
using System.Collections.Generic;
using System.Linq;
using _DatabaseAPI.Scripts;
using _PaintToCard.Scripts;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using ImageUtils;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Compatetion.MemoryMatch
{
    public class MemoryMatchGame
    {
        public event EventHandler<bool> OnGameOver;
        public event EventHandler<bool> OnPairMatch;
        public event Action<MemoryMatchTile> OnAnyTileClicked;

        private readonly string m_Name;
        private readonly MemoryMatchTile m_TilePrefab;
        private readonly Sprite[] m_PairSprites;
        private readonly Texture2D m_Texture;
        private readonly int m_Row;
        private readonly int m_Column;
        private readonly Vector3 m_Offset;
        private readonly MemoryMatchTile[,] m_Tiles;
        private readonly List<MemoryMatchPlayer> m_Players;
        private readonly float m_TimePerTurn;

        private MemoryMatchTile m_FirstSelectedTile;
        private MemoryMatchTile m_SecondSelectedTile;
        private int m_CurrentPlayerIndex;
        private int m_ClickCount;
        private bool m_IsPlaying;
        private bool m_IsBusy;


        public MemoryMatchGame(
            string name, 
            MemoryMatchTile tilePrefab, 
            Sprite[] pairSprites, 
            Texture2D texture, 
            int row, 
            int column,
            float timePerTurn,
            Vector3 offset = default)
        {
            m_Name = name;
            m_TilePrefab = tilePrefab;
            m_PairSprites = pairSprites;
            m_Texture = texture;
            m_Row = row;
            m_Column = column;
            m_Offset = offset;
            m_Tiles = new MemoryMatchTile[column, row];
            m_Players = new List<MemoryMatchPlayer>();
            m_TimePerTurn = timePerTurn;

            Initialize();
            StartGame();
        }


        public void OnTileClicked(MemoryMatchTile tile)
        {
            m_ClickCount += 1;
            OnAnyTileClicked?.Invoke(tile);
        }
        
        public void OnTileSelected(MemoryMatchTile tile)
        {
            if (!m_FirstSelectedTile)
            {
                m_FirstSelectedTile = tile;
            }

            else if (!m_SecondSelectedTile)
            {
                m_SecondSelectedTile = tile;
                CheckPair();
            }
        }

        public void RevealAllTilesForSeconds(float seconds)
        {
            if (m_FirstSelectedTile)
            {
                m_FirstSelectedTile.SetIsClickable(true);
            }

            if (m_SecondSelectedTile)
            {
                m_SecondSelectedTile.SetIsClickable(true);
            }
            
            m_FirstSelectedTile = null;
            m_SecondSelectedTile = null;
            m_ClickCount = 0;

            foreach (var tile in m_Tiles)
            {
                if (!tile.GetIsClickable()) continue;
                
                tile.RevealForSeconds(seconds);
            }
        }

        public void AutoMatchSinglePair()
        {
            var clickableTiles = GetAllClickableTiles();
            
            if (clickableTiles.Count <= 0) return;
            
            var randomTile = clickableTiles[Random.Range(0, clickableTiles.Count)];

            clickableTiles.Remove(randomTile);

            var pairTile = clickableTiles
                .FirstOrDefault(tile => tile.GetPairIndex() == randomTile.GetPairIndex());
            
            if (!pairTile) return;
            
            randomTile.Click();
            pairTile.Click();
        }

        public MemoryMatchPlayer GetLocalPlayer()
        {
            return m_Players[0];
        }

        public int GetRow()
        {
            return m_Row;
        }

        public int GetColumn()
        {
            return m_Column;
        }

        public void AddPlayer(MemoryMatchPlayer player)
        {
            m_Players.Add(player);
        }

        public IReadOnlyList<MemoryMatchPlayer> GetPlayers()
        {
            return m_Players;
        }

        public MemoryMatchTile GetTile(int x, int y)
        {
            return m_Tiles[x, y];
        }

        public List<MemoryMatchTile> GetAllClickableTiles()
        {
            var result = new List<MemoryMatchTile>();

            for (var x = 0; x < m_Column; x++)
            {
                for (var y = 0; y < m_Row; y++)
                {
                    var tile = m_Tiles[x, y];
                    
                    if (!tile.GetIsClickable()) continue;
                    
                    result.Add(tile);
                }
            }

            return result;
        }

        public int GetPlayerCount()
        {
            return m_Players.Count;
        }

        public bool IsPlayerTurn(MemoryMatchPlayer player)
        {
            return m_Players[m_CurrentPlayerIndex] == player;
        }

        public bool IsPlaying()
        {
            return m_IsPlaying;
        }

        public bool IsMaxClickReached()
        {
            return m_ClickCount >= 2;
        }

        public bool GetIsBusy()
        {
            return m_IsBusy;
        }

        public void SetIsBusy(bool value)
        {
            m_IsBusy = value;
        }

        public void MoveNextPlayer()
        {
            if (m_FirstSelectedTile)
            {
                m_FirstSelectedTile.OnMatchFailed();
                m_FirstSelectedTile = null;
            }
            
            var index = (m_CurrentPlayerIndex + 1) % GetPlayerCount();

            SetCurrentPlayerIndex(index);
        }
        
        public void SetCurrentPlayerIndex(int index)
        {
            m_CurrentPlayerIndex = index;
            
            var player = m_Players[index];
            TickPlayerTurn(player);
            player.SetMoveTimer(m_TimePerTurn);
        }

        
        private void OnWin()
        {
            LocalUser.AddQuestProgress(QuestType.Win_Generation);
            LocalUser.AddQuestProgress(QuestType.Win_MemoryMatch);
            
            LocalUser.AddCard(new CardData
            {
                imageID = PaintToCardConverter.GetGeneratedImageID()
            }, response =>
            {
                OnGameOver?.Invoke(this, true);
            });
        }

        private void OnLose()
        {
            OnGameOver?.Invoke(this, false);
        }

        private void OnDraw()
        {
            OnGameOver?.Invoke(this, false);
        }
        
        
        private void StartGame()
        {
            m_IsPlaying = true;
        }
        
        private void EndGame()
        {
            m_IsPlaying = false;
            FinalizeGame();
        }

        private void FinalizeGame()
        {
            MemoryMatchPlayer winnerPlayer = null;
            var bestScore = int.MinValue;
            var isDraw = false;
            
            foreach (var player in m_Players)
            {
                var score = player.GetScore();

                if (score == bestScore)
                {
                    isDraw = true;
                    continue;
                }
                
                if (score < bestScore) continue;

                winnerPlayer = player;
                bestScore = score;
                isDraw = false;
            }

            if (isDraw)
            {
                OnDraw();
                return;
            }

            var isWin = winnerPlayer == m_Players[0];

            if (isWin)
            {
                OnWin();
                return;
            }
            
            OnLose();
        }

        private bool GetIsAllTilesOpen()
        {
            for (var x = 0; x < m_Column; x++)
            {
                for (var y = 0; y < m_Row; y++)
                {
                    if (!m_Tiles[x, y].GetIsClickable()) continue;

                    return false;
                }
            }

            return true;
        }

        private void TickPlayerTurn(MemoryMatchPlayer player)
        {
            player.MakeMove();
        }

        private void CheckPair()
        {
            var firstPairIndex = m_FirstSelectedTile.GetPairIndex();
            var secondPairIndex = m_SecondSelectedTile.GetPairIndex();
            var isMatched = firstPairIndex == secondPairIndex;

            if (isMatched)
            {
                var player = m_Players[m_CurrentPlayerIndex];
                player.AddScore(1);
                
                m_FirstSelectedTile.OnMatchSucceed();
                m_SecondSelectedTile.OnMatchSucceed();
                
                OnPairMatch?.Invoke(this, true);

                if (GetIsAllTilesOpen())
                {
                    EndGame();
                }

                else
                {
                    SetCurrentPlayerIndex(m_CurrentPlayerIndex);
                }
            }

            else
            {
                m_FirstSelectedTile.OnMatchFailed();
                m_SecondSelectedTile.OnMatchFailed();
                
                OnPairMatch?.Invoke(this, false);
                MoveNextPlayer();
            }
            
            m_FirstSelectedTile = null;
            m_SecondSelectedTile = null;
            m_ClickCount = 0;
        }

        private void Initialize()
        {
            const float pixelPerUnit = 100f;
            const float scale = 4f;

            var parent = new GameObject(m_Name).transform;
            var sprites = ImageSplitter.Split(m_Texture, m_Row, m_Column);
            var totalSizeX = m_Texture.width / pixelPerUnit;
            var totalSizeY = m_Texture.height / pixelPerUnit;
            var sizeX = totalSizeX / m_Column;
            var sizeY = totalSizeY / m_Row;
            var centerOffset = new Vector3(totalSizeX - sizeX, totalSizeY - sizeY) * -0.5f;
            var pairCount = m_Row * m_Column / 2;
            var pairIndices = new int[m_Row * m_Column];
            var counter = 0;

            for (var i = 0; i < pairIndices.Length; i++)
            {
                pairIndices[i] = -1;
            }
            
            for (var i = 0; i < pairCount; i++)
            {
                int randomPairIndex;
                
                while (true)
                { 
                    randomPairIndex = Random.Range(0, m_PairSprites.Length);
                    
                    if (!pairIndices.Contains(randomPairIndex)) break;
                }
                
                pairIndices[2 * i] = randomPairIndex;
                pairIndices[2 * i + 1] = randomPairIndex;
            }
            
            var count = pairIndices.Length;
            for (var i = count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (pairIndices[i], pairIndices[randomIndex]) = (pairIndices[randomIndex], pairIndices[i]);
            }

            for (var y = 0; y < m_Row; y++)
            {
                for (var x = 0; x < m_Column; x++)
                {
                    var sprite = sprites[counter];
                    var tile = Object.Instantiate(m_TilePrefab, parent);
                    var positionX = x * sizeX;
                    var positionY = y * sizeY;
                    var position = new Vector3(positionX, positionY) + centerOffset + m_Offset;
                    var pairIndex = pairIndices[counter];
                    var randomPairSprite = m_PairSprites[pairIndex];

                    m_Tiles[x, y] = tile;
                    tile.Inject(this);
                    tile.name = $"({x}, {y})";
                    tile.transform.position = position;
                    tile.SetGridPosition(x, y);
                    tile.SetPairIndex(pairIndex);
                    tile.SetHiddenSprite(sprite);
                    tile.MatchColliderWithSpriteSize(sprite);
                    tile.SetFrontFaceSprite(randomPairSprite);
                    tile.MatchFrontFaceWithSpriteSize(sprite);
                    tile.MatchBackFaceWithSpriteSize(sprite);
                    tile.InstantClose();
                    
                    counter += 1;
                }
            }
            
            parent.localScale = Vector3.one * scale;
        }
    }
}