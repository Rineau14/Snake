using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Snake
{
    static class Collision
    {
        static public bool CheckChoicesCollision(Snake snake, List<GridCell> choices, Level level)
        {
            if (snake.coordinates == choices[0].coordinates)
            {
                level.currentLevel = Level.levelName.free;
                level.LevelFree();
                return true;
            }
            else if (snake.coordinates == choices[1].coordinates)
            {
                level.currentLevel = Level.levelName.freeWall;
                level.LevelFreeWall();
                return true;
            }
            else if (snake.coordinates == choices[2].coordinates)
            {
                level.currentLevel = Level.levelName.story;
                level.LevelA();
                return true;
            }
            else if (snake.coordinates == choices[3].coordinates)
            {
                level.currentLevel = Level.levelName.mapEditor;
                level.currentStep = Level.mapEditorStep.sizeSet;
                return true;
            }
            else if (snake.coordinates == choices[4].coordinates)
            {
                level.currentLevel = Level.levelName.vsPlayer;
                level.LevelVersus();
                return true;
            }
            else if (snake.coordinates == choices[5].coordinates)
            {
                level.currentLevel |= Level.levelName.vsAI;
                level.LevelVersus();
                return true;
            }
            return false;
        }

        static public bool CheckAppleCollision(Snake snake, Apple apple)
        {
            if (snake.coordinates == apple.coordinates)
            {
                snake.score++;
                snake.isGrowing = true;
                return true;
            }
            return false;
        }

        static public bool CheckLoosingCollision(Snake snake, List<GridCell> walls, List<SnakeCell> snakeCells, List<SnakeCell> secondSnakeCells)
        {
            foreach (GridCell cell in walls)
            {
                if (snake.coordinates == cell.coordinates)
                    return true;
            }
                
            foreach (SnakeCell snakeCell in snakeCells)
            {
                if (snake.coordinates == snakeCell.coordinates)
                    return true;
            }

            foreach (SnakeCell secondSnakeCell in secondSnakeCells)
            {
                if (snake.coordinates == secondSnakeCell.coordinates)
                    return true;
            }

            return false;
        }
    }
}
