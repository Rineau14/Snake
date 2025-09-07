using Raylib_cs;
using System.Numerics;

namespace Snake
{
    class SnakeCell : Cell
    {
        public float bodySize;
        public float bodyDecreaseSize;
        public SnakeCell(Vector2 coordinates, int cellSize, int OFFSETX, int OFFSETY) : base(coordinates, cellSize, OFFSETX, OFFSETY)
        {
            bodyDecreaseSize = 0.1f;
            bodySize = cellSize - bodyDecreaseSize;
        }

        public void Reposition()
        {
            bodySize -= 0.1f;
            //!!! CENTRER !!!!!!!!
        }

        public override void Draw()
        {
            Raylib.DrawRectangle(posX, posY, (int)bodySize, (int)bodySize, Color.Green); 
        }

        public  void DrawSecond()
        {
            Raylib.DrawRectangle(posX, posY, (int)bodySize, (int)bodySize, Color.Blue);
        }
    }
}
