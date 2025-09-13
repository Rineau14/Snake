using Raylib_cs;
using System.Numerics;

namespace Snake
{
    class SnakeCell : Cell
    {
        public int bodySize { get; private set; }
        public int bodyDecreaseSize { get; private set; }
        public SnakeCell(Vector2 coordinates, int cellSize, int OFFSETX, int OFFSETY) : base(coordinates, cellSize, OFFSETX, OFFSETY)
        {
            bodyDecreaseSize = 1;
            bodySize = cellSize - bodyDecreaseSize;
        }

        public override void Reposition(int bodySize)
        {
            bodySize --;
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
