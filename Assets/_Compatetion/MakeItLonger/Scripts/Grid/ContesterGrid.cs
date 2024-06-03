using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MakeItLonger
{
    public enum GridDirection
    {
        Right,
        Left
    }
    
    public class ContesterGrid : MonoBehaviour
    {
        
        [SerializeField] private Vector3 gridStartPos;
        [SerializeField] private Grid grid;
        [SerializeField] private Contester contester;
        
        
        private int gridXNumber = 5;
        private int gridYNumber = 100;
        private int gridZNumber = 1;
        private Grid[,] grids = new Grid[5, 100];


        private void Awake()
        {
            InitGridSystem();
        }

        private void Start()
        {
            contester.OnBoxReleased += OnBoxReleased;
        }

        private void OnBoxReleased(object sender, OnBoxReleasedEventArgs e)
        {
            e.DropGrid.isFull = true;
        }

        private void InitGridSystem()
        {
            for (int x = 0; x < gridXNumber; x++)
            {
                for (int y = 0; y < gridYNumber; y++)
                {
                    var pos = new Vector3(gridStartPos.x + (x * grid.GetGridSize().x), gridStartPos.y + (y * grid.GetGridSize().y),
                        gridStartPos.z);
                    var a = Instantiate(grid, pos, Quaternion.identity);
                    a.xIndex = x;
                    a.yIndex = y;
                    grids[x, y] = a;
                }
            }
        }
        
        public Grid[,] GetAllGrid()
        {
            return grids;
        }

        public Grid GetNextGrid(Grid currentGrid, GridDirection gridDirection)
        {
            if (gridDirection == GridDirection.Right)
            {
                return grids[currentGrid.xIndex + 1, currentGrid.yIndex];
            }
            
            return grids[currentGrid.xIndex -1, currentGrid.yIndex];
        }

        public int GetProperLineIndex()
        {
            bool isAllLineEmpty;
            
            for (int y = 0; y < 100; y++)
            {
                isAllLineEmpty = true;
                
                for (int x = 0; x < 5; x++)
                {
                    if (grids[x,y].isFull)
                    {
                        isAllLineEmpty = false;
                    }
                }

                if (isAllLineEmpty)
                {
                    return y;
                }
            }

            return -1;
        }

        public int GetRandomColumnIndex()
        {
            return Random.Range(0, 5);
        }

        public GridDirection GetNextBoxesGridDirection()
        {
            var line = GetProperLineIndex();

            return line % 2 == 0 ? GridDirection.Right : GridDirection.Left;
        }

        public Grid CalculateBoxDropGrid(int releasedXGrid, int releasedYGrid, ContesterGrid contesterGrid)
        {
            var allGrid = contesterGrid.GetAllGrid();

            for (int i = releasedYGrid; i >= 0; i--)
            {
                if (allGrid[releasedXGrid, i].isFull)
                {
                    return grids[releasedXGrid, i + 1];
                }
            }
            
            return grids[releasedXGrid, 0];
        }

        public Vector3 GetWorldPositionFromGridPosition(int x, int y)
        {
            return GetAllGrid()[x, y].transform.position;
        }

        public Grid GetGridFromWorld(Vector3 pos)
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    if (grids[x,y].transform.position == pos)
                    {
                        return grids[x, y];
                    }
                }
            }

            return null;
        }

        public int GetHeightOfStack()
        {
            for (int y = 99; y >= 0; y--)
            {
                for (int x = 4; x >= 0; x--)
                {
                    if (grids[x, y].isFull)
                    {
                        return y + 1;
                    }
                }
            }
            
            return -1;
        }

        

    }
}