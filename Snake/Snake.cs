using Raylib_cs;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;

namespace Snake
{
    class Snake : Cell
    {
        public Vector2 coordinates;
        protected Vector2 position;
        protected Vector2 size;
        public int score;

        public bool isGrowing { get; set;}
        protected bool nextMove = false;
        protected enum Direction{Neutral, Up, Down, Left, Right}

        protected Direction snakeMovement = Direction.Neutral;


        public Snake(Vector2 coordinates, int cellSize, int OFFSETX, int OFFSETY) : base(coordinates, cellSize, OFFSETX, OFFSETY)
        {
            this.coordinates = coordinates;
            position = new Vector2(posX, posY);
            size = new Vector2(cellSize, cellSize);
            isGrowing = false;
        }

        public virtual void SnakeDirection()
        {
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.Left)) && (snakeMovement != Direction.Right))
            {
                snakeMovement = Direction.Left;
                nextMove = true;
            }
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.Right)) && (snakeMovement != Direction.Left))
            {
                snakeMovement = Direction.Right;
                nextMove = true;
            }
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.Up)) && (snakeMovement != Direction.Down))
            {
                snakeMovement = Direction.Up;
                nextMove = true;
            }
            if ((!nextMove) && (Raylib.IsKeyDown(KeyboardKey.Down)) && (snakeMovement != Direction.Up))
            {
                snakeMovement = Direction.Down;
                nextMove = true;
            }
        }

        public virtual void UpdateCoordinates(int nbColumns, int nbRows)
        {
            switch (snakeMovement)
            {
                case Direction.Left:
                    coordinates.X -= 1;
                    if (coordinates.X < 0)
                        coordinates.X = nbColumns - 1;
                    break;
                case Direction.Right:
                    coordinates.X += 1;
                    if (coordinates.X > nbColumns - 1)
                        coordinates.X = 0;
                    break;
                case Direction.Up:
                    coordinates.Y -= 1;
                    if (coordinates.Y < 0)
                        coordinates.Y = nbRows - 1;
                    break;
                case Direction.Down:
                    coordinates.Y += 1;
                    if (coordinates.Y > nbRows - 1)
                        coordinates.Y = 0;
                    break;
                default:
                    break;
            }
            nextMove = false;
        }
        public void UpdatePosition(int cellSize, int OFFSETX, int OFFSETY)
        {
            position = new Vector2(coordinates.X * cellSize + OFFSETX, coordinates.Y * cellSize + OFFSETY);
        }

        public void SnakeNeutral()
        {
            snakeMovement = Direction.Neutral;
        }

        public override void Draw()
        {
            Raylib.DrawRectangleV(position, size, Color.Green);
        }
    }
}
