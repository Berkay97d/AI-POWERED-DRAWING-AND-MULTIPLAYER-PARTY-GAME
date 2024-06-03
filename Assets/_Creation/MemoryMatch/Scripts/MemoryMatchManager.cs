using _PaintToCard.Scripts;
using UnityEngine;

namespace _Compatetion.MemoryMatch
{
    public class MemoryMatchManager : MonoBehaviour
    {
        [SerializeField] private MemoryMatchPlayerScoreUI[] playerScores;
        [SerializeField] private MemoryMatchPlayerTimerUI[] playerTimers;
        [SerializeField] private MemoryMatchTile tilePrefab;
        [SerializeField] private Sprite[] pairSprites;
        [SerializeField] private Texture2D testTexture;
        [SerializeField] private int row;
        [SerializeField] private int column;
        [SerializeField] private float timePerTurn = 15;


        private MemoryMatchGame m_Game;
        

        private void Start()
        {
            var player1 = new HumanMemoryMatchPlayer("al2");
            var player2 = new AIMemoryMatchPlayer("berkay");
            m_Game = CreateGame("Game 1");

            m_Game.AddPlayer(player1);
            m_Game.AddPlayer(player2);
            AssignGameToPlayer(player1, m_Game);
            AssignGameToPlayer(player2, m_Game);
            m_Game.SetCurrentPlayerIndex(0);

            var players = m_Game.GetPlayers();

            for (var i = 0; i < players.Count; i++)
            {
                playerScores[i].SetPlayer(players[i]);
                playerTimers[i].SetPlayer(players[i]);
            }
            
            player1.SetScore(0);
            player2.SetScore(0);
            
            player2.Awake();
        }

        private void Update()
        {
            foreach (var player in m_Game.GetPlayers())
            {
                player.Update();
            }
        }


        public MemoryMatchGame GetGame()
        {
            return m_Game;
        }
        

        private MemoryMatchGame CreateGame(string gameName, Vector3 offset = default)
        {
            var game = new MemoryMatchGame(gameName, tilePrefab, pairSprites, GetTexture(), row, column, timePerTurn, offset);
            return game;
        }

        private void AssignGameToPlayer(MemoryMatchPlayer player, MemoryMatchGame game)
        {
            player.AssignGame(game);
        }

        private Texture2D GetTexture()
        {
            var texture = PaintToCardConverter.GetGeneratedImage();
            
            return texture ? texture : testTexture;
        }
    }
}