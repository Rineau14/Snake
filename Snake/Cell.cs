using Raylib_cs;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Threading.Tasks.Sources;

namespace Snake
{
    abstract class Cell
    {
        public Vector2 coordinates { get; private set; }
        public int cellSize { get; set; }
        protected int OFFSETX { get; set; }
        protected int OFFSETY { get; set; }
        public int posX {  get; private set; }
        public int posY { get; private set; }

        public int distStart { get; private set; }
        public int distEnd { get; set; }
        public int sumDist { get; private set; }
        public GridCell parent;

        public Cell(Vector2 coordinates, int cellSize, int OFFSETX = 0, int OFFSETY = 0)
        {
            this.coordinates = coordinates;
            this.cellSize = cellSize;
            posX = (int)coordinates.X * cellSize + OFFSETX;
            posY = (int)coordinates.Y * cellSize + OFFSETY;
        }

        public abstract void Draw();

        public virtual void Reposition(int currentCellSize)
        {
            cellSize = 20;
            posX += (currentCellSize - cellSize) / 2;
            posY += (currentCellSize - cellSize) / 2;
        }

    }
}
    
