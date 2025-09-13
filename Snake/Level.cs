using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    sealed class Level
    {
        public int[,]? grid;
        public enum levelName {none, free, freeWall, story, mapEditor, vsPlayer, vsAI}
        public levelName currentLevel;
        public enum storyName { A,B,C,D,E}
        public storyName currentStory;
        public enum mapEditorStep {none, sizeSet, wallSet, opponent, ready}
        public mapEditorStep currentStep = mapEditorStep.none;
        int initColumn = 5;
        int initRow = 5;
        public bool vsPlayer { get; set; } = false;
        public bool vsAI { get; set; } = false;
        bool enterIsDown = false;
        
        public void Menu()
        {
            currentLevel = levelName.none;
            grid = new int[,] {{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 2, 0, 2, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1,},
                               { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
                               { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                               { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1 },
                               { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                               { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1,},
                               { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 2, 0, 2, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }};
        }
        public void LevelFree()
        {
            currentLevel = levelName.free;
            grid = new int[,] {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
        }
        public void LevelFreeWall()
        {
            currentLevel = levelName.freeWall;
            grid = new int[,] {{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}};
        }
        public void LevelA()
        {
            currentLevel = levelName.story;
            currentStory = storyName.A;
            grid = new int[,] {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};
        }
        public void LevelB()
        {
            currentStory = storyName.B;
            grid = new int[,] {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 1, 1, 0, 0, 0, 0, 1, 1, 0 },
                               { 0, 1, 1, 0, 0, 0, 0, 1, 1, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 1, 1, 0, 0, 0, 0, 1, 1, 0 },
                               { 0, 1, 1, 0, 0, 0, 0, 1, 1, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};
        }

        public void LevelC()
        {
            currentStory = storyName.C;
            grid = new int[,] {{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }};
        }

        public void LevelD()
        {
            currentStory = storyName.D;
            grid = new int[,] {{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 },
                               { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,0 }};
        }
        public void LevelE()
        {
            currentStory = storyName.E;
            grid = new int[,] {{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                               { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0 },
                               { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                               { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0 },
                               { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0 },
                               { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0 },
                               { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                               { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0 },
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }};
        }

        public void LevelVersus()
        {
            grid = new int[,] {{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}};
        }

        public void MapEditor(List<GridCell> cells)
        {
            if (Raylib.IsKeyUp(KeyboardKey.Enter))
            {
                enterIsDown = false;
            }
            if (currentStep == mapEditorStep.sizeSet)
            {
                if ((Raylib.IsKeyPressed(KeyboardKey.Right)) && (initColumn < 30))
                {
                    initColumn++;
                }
                if ((Raylib.IsKeyPressed(KeyboardKey.Left)) && (initColumn > 5))
                {
                    initColumn--;
                }
                if ((Raylib.IsKeyPressed(KeyboardKey.Up)) && (initRow < 30))
                {
                    initRow++;
                }
                if ((Raylib.IsKeyPressed(KeyboardKey.Down)) && (initRow > 5))
                {
                    initRow--;
                }
                grid = new int[initRow, initColumn];
                if ((Raylib.IsKeyPressed(KeyboardKey.Enter)) && (enterIsDown == false))
                {
                    enterIsDown = true;
                    currentStep = mapEditorStep.opponent;
                }
            }

            if (currentStep == mapEditorStep.opponent)
            {
                
                if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    vsPlayer = false;
                    vsAI = false;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Left))
                {
                    vsPlayer = true;
                    vsAI = false;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Right))
                {
                    vsPlayer = false;
                    vsAI = true;
                }
                if ((Raylib.IsKeyPressed(KeyboardKey.Enter)) && (enterIsDown == false))
                {
                    enterIsDown = true;
                    currentStep = mapEditorStep.wallSet;
                }
            }

            if (currentStep == mapEditorStep.wallSet)
            {
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    int mouseX = Raylib.GetMouseX();
                    int mouseY = Raylib.GetMouseY();
                    foreach (GridCell cell in cells)
                    {
                        if ((mouseX >= cell.posX) && 
                           (mouseX <= cell.posX + cell.cellSize) && 
                           (mouseY >= cell.posY) && 
                           ( mouseY <= cell.posY + cell.cellSize))
                        {
                            if (grid[(int)cell.coordinates.Y, (int)cell.coordinates.X] == 0)
                                grid[(int)cell.coordinates.Y, (int)cell.coordinates.X] = 1;
                            else
                                grid[(int)cell.coordinates.Y, (int)cell.coordinates.X] = 0;
                        }
                    }
                }
                if ((Raylib.IsKeyPressed(KeyboardKey.Enter)) &&(enterIsDown == false))
                {
                    if (vsPlayer)
                    {
                        currentLevel = levelName.vsPlayer;
                    }
                    if (vsAI)
                    {
                        currentLevel = levelName.vsAI;
                    }
                    currentStep = mapEditorStep.ready;
                }
            }

            
        }
    }
}
