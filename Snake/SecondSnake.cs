using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class SecondSnake : Snake
    {
        public SecondSnake(Vector2 coordinates, int cellSize, int OFFSETX, int OFFSETY) : base(coordinates, cellSize, OFFSETX, OFFSETY)
        {

        }

        public override void SnakeDirection()
        {
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.X)) && (snakeMovement != Direction.Right))
            {
                snakeMovement = Direction.Left;
                nextMove = true;
            }
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.V)) && (snakeMovement != Direction.Left))
            {
                snakeMovement = Direction.Right;
                nextMove = true;
            }
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.D)) && (snakeMovement != Direction.Down))
            {
                snakeMovement = Direction.Up;
                nextMove = true;
            }
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.C)) && (snakeMovement != Direction.Up))
            {
                snakeMovement = Direction.Down;
                nextMove = true;
            }
        }

        public override void Draw()
        {
            Raylib.DrawRectangleV(position, size, Color.Blue);
        }
    }
}
