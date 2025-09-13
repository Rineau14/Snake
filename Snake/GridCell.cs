using Raylib_cs;
using System.Numerics;


namespace Snake
{
    class GridCell : Cell
    {
        public GridCell(Vector2 coordinates, int cellSize, int OFFSETX, int OFFSETY) : base(coordinates, cellSize, OFFSETX, OFFSETY)
        {

        }
        public override void Draw()
        {
            Raylib.DrawRectangleLines(posX, posY, cellSize, cellSize, Color.Red);
        }

        public void DrawWall()
        {
            Raylib.DrawRectangle(posX, posY, cellSize, cellSize, Color.Black);
        }

        public void DrawChoice()
        {
            Raylib.DrawRectangle(posX, posY, cellSize, cellSize, Color.Yellow);
        }
    }
}
