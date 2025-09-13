using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class AISnake : Snake
    {
        List<Cell> path = new List<Cell>();

        List<Cell> openList = new List<Cell>();
        List<Cell> closeList = new List<Cell>();
        public AISnake(Vector2 coordinates, int cellSize, int OFFSETX, int OFFSETY) : base(coordinates, cellSize, OFFSETX, OFFSETY)
        {

        }

        public Vector2 Pathfinding(List<GridCell> cells, List<GridCell> walls, List<SnakeCell> snakeCells, List<SnakeCell> secondSnakeCells, Apple apple, Vector2 aiSnakeCoord)
        {
            openList.Clear();
            closeList.Clear();
            int minSum = cells.Count;
            GridCell currentNode = new GridCell(aiSnakeCoord, cellSize, OFFSETX, OFFSETY);

            foreach (GridCell wall in walls)
                closeList.Add(wall);
            foreach (SnakeCell snakeCell in snakeCells)
                closeList.Add(snakeCell);
            foreach (SnakeCell secondSnakeCell in secondSnakeCells)
                closeList.Add(secondSnakeCell);

            while (true)
            {
                foreach (GridCell cell in cells)
                {
                    if ((!closeList.Contains(cell)) && (!openList.Contains(cell)))
                    {
                        if (((cell.coordinates.X == currentNode.coordinates.X - 1) && (cell.coordinates.Y == currentNode.coordinates.Y)) ||
                            ((cell.coordinates.X == currentNode.coordinates.X) && (cell.coordinates.Y == currentNode.coordinates.Y - 1)) ||
                            ((cell.coordinates.X == currentNode.coordinates.X + 1) && (cell.coordinates.Y == currentNode.coordinates.Y)) ||
                            ((cell.coordinates.X == currentNode.coordinates.X) && (cell.coordinates.Y == currentNode.coordinates.Y + 1)))
                        {
                            openList.Add(cell);
                            cell.parent = currentNode;
                        }
                    }
                    foreach (SnakeCell secondSnakeCell in secondSnakeCells)
                    {
                        if (cell.coordinates == secondSnakeCell.coordinates)
                        {
                            openList.Remove(cell);
                        }
                    }
                    foreach (SnakeCell SnakeCell in snakeCells)
                    {
                        if (cell.coordinates == SnakeCell.coordinates)
                        {
                            openList.Remove(cell);
                        }
                    }
                }

                foreach (GridCell cell in openList)
                {
                    cell.distEnd = Math.Abs((int)cell.coordinates.X - (int)apple.coordinates.X) + Math.Abs((int)cell.coordinates.Y - (int)apple.coordinates.Y);
                    if (cell.distEnd < minSum)
                    {
                        minSum = cell.distEnd;
                        currentNode = cell;
                    }
                }
                minSum = cells.Count;
                closeList.Add(currentNode);
                openList.Remove(currentNode);
                if (openList.Count == 0)
                    break;
                
                if (currentNode.coordinates == apple.coordinates)
                {
                    break;
                }
            }

            GridCell parentChain = currentNode;
            GridCell parentTarget = currentNode;
            while (true)
            {
                parentChain = parentChain.parent;
                if (parentChain is null)
                {
                    return new Vector2(coordinates.X, coordinates.Y+1);
                }
                else if (parentChain.coordinates != aiSnakeCoord)
                {
                    path.Add(parentChain);
                    parentTarget = parentChain;
                }
                else
                    return parentTarget.coordinates;
            }
        }

        //public void drawPath()
        //{
        //    foreach (Cell cell in openList)
        //        Raylib.DrawRectangle(cell.posX, cell.posY, cellSize, cellSize, Color.Orange);
        //}

        public override void Draw()
        {
            Raylib.DrawRectangleV(position, size, Color.Blue);
        }
    }
}
