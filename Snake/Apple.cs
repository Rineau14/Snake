using Raylib_cs;
using System;
using System.Numerics;

namespace Snake
{
    sealed class Apple
    {
        private readonly int rangeColumns;
        private readonly int rangeRows;
        public Vector2 coordinates { get; private set; }
        public int posX { get; private set; }
        public int posY { get; private set; }

        Random rand = new Random();

        public Apple(int nbColumns, int nbRows)
        {
            rangeColumns = nbColumns;
            rangeRows = nbRows;
        }

        public void CreateApplePositon(int cellSize, int OFFSETX, int OFFSETY)
        {
            int appleColumn = rand.Next(rangeColumns);
            int appleRow =  rand.Next(rangeRows);
            coordinates = new Vector2(appleColumn, appleRow);
            posX = appleColumn * cellSize + cellSize / 2 + OFFSETX;
            posY = appleRow * cellSize + cellSize / 2 + OFFSETY;
        }

        public void Draw()
        {
            Raylib.DrawCircle(posX, posY, 10, Color.Red);
        }
    }
}
