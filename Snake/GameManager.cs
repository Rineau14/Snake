using Raylib_cs;
using System.Numerics;
using static Snake.Level;

namespace Snake
{
    class GameManager
    {
        int SCREENWIDTH = 1920;
        int SCREENHEIGHT = 950;
        int OFFSETX;
        int OFFSETY;
        int nbColumns;
        int nbRows;
        const int cellSize = 30;

        // Liste des objets
        List<GridCell> cells = new List<GridCell>();
        List<GridCell> walls = new List<GridCell>();
        List<GridCell> choices = new List<GridCell>();
        List<Snake> snakes = new List<Snake>();
        List<SnakeCell> snakeCells = new List<SnakeCell>();
        List<SnakeCell> secondSnakeCells = new List<SnakeCell>();
        Level? level = new Level();
        Snake? snake;
        SecondSnake? secondSnake;
        AISnake? aiSnake;
        Apple? apple;

        bool menuLoaded = false;
        bool isPaused = false;
        bool victory = false;
        bool gameOver = false;
        bool draw = false;

        bool recordBeaten = false;
        bool levelModeBeaten = false;
        bool aiBeaten = false;

        int record = 30;
        int speedUP = 7;
        int remainingSpeedUp;
        int appleObjectif = 20;
        int remainingApples;

        int posXPlayer;

        float timer = 0f;
        double timeDelay = 5;

        float messageTimer = 0;
        int messageTimeDelay = 3;
        bool showMessage = true;
        
        public enum sceneType {menu, game, mapEditor}
        public sceneType currentScene = sceneType.menu;

        
        public void Update()
        {
            // Menu
            if (currentScene == sceneType.menu)
            {
                // Chargement du menu uniquement au lancement du jeu
                level.Menu();
                if (menuLoaded == false)
                {
                    SceneInit(level.grid.GetLength(1)/2, level.grid.GetLength(0)/2);
                    menuLoaded = true;
                }
                
                // Déplacement du joueur dans le menu
                if (Raylib.IsKeyPressed(KeyboardKey.Left) || Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.Right) || Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    Vector2 currentSnakeCoord = snake.coordinates;
                    snake.SnakeDirection();
                    snake.UpdateCoordinates(nbColumns, nbRows);
                    if (!Collision.CheckLoosingCollision(snake, walls, snakeCells, secondSnakeCells))
                        snake.UpdatePosition(cellSize, OFFSETX, OFFSETY);
                    else
                        snake.coordinates = currentSnakeCoord;

                    snake.SnakeNeutral();
                }

                // Choix du mode de jeu
                if (Collision.CheckChoicesCollision(snake, choices, level))
                {
                    if ((level.currentLevel == levelName.vsPlayer) || (level.currentLevel == levelName.vsAI))
                    {
                        currentScene = sceneType.game;
                        SceneInit(1, level.grid.GetLength(0) / 2);
                    }
                    else if (level.currentLevel != levelName.mapEditor)
                    {
                        currentScene = sceneType.game;
                        SceneInit(level.grid.GetLength(1) / 2, level.grid.GetLength(0) / 2);
                    }
                    else
                        currentScene = sceneType.mapEditor;
                }
            }

            // Editeur de map
            if (currentScene == sceneType.mapEditor)
            {
                level.MapEditor(cells);
                
                if ((level.vsPlayer) || (level.vsAI))
                {
                    posXPlayer = 1;
                }
                else
                {
                    posXPlayer = level.grid.GetLength(1) / 2;
                }
                if (level.currentStep == mapEditorStep.ready)
                {
                    currentScene = sceneType.game;
                    level.vsPlayer = false; // peut-être appeler une méthode dans Level
                    level.vsAI = false;
                }
                SceneInit(posXPlayer, level.grid.GetLength(0) / 2);
            }

            // En jeu
            if (currentScene == sceneType.game)
            {
                //POUR DEBUG
                if ((level.currentLevel == levelName.free) && (Raylib.IsKeyPressed(KeyboardKey.U)))
                    recordBeaten = true;
                else if ((level.currentLevel == levelName.story) && (Raylib.IsKeyPressed(KeyboardKey.U)))
                    levelModeBeaten = true;
                else if ((level.currentLevel == levelName.vsAI) && (Raylib.IsKeyPressed(KeyboardKey.U)))
                    aiBeaten = true;

                // Gère pause/victoire/game over
                if (Raylib.IsKeyPressed(KeyboardKey.P))
                {
                    isPaused = !isPaused;
                }
                if (((isPaused) || (victory) || (gameOver) || (draw)) && (Raylib.IsKeyPressed(KeyboardKey.R)))
                {
                    if (((level.currentLevel == levelName.free) || (level.currentLevel == levelName.freeWall)) && (snake.score > record))
                    {
                        record = snake.score;
                        recordBeaten = true;
                    }
                        

                    currentScene = sceneType.menu;
                    level.currentLevel = Level.levelName.none;
                    level.Menu();
                    SceneInit(level.grid.GetLength(1) / 2, level.grid.GetLength(0) / 2);
                    return;
                }

                // Déroulement d'une partie
                if ((!isPaused) && (!victory) && (!gameOver) && (!draw))
                {
                    timer += 0.1f;
                    if (timer >= timeDelay)
                    {
                        // Gère l'agrandissement de la queue des 3 snakes possible
                        timer = 0;
                        if (snake.score > 0)
                        {
                            if (snake.isGrowing == false)
                                snakeCells.Remove(snakeCells[0]);                            
                            SnakeCell snakeCell = new SnakeCell(snake.coordinates, cellSize, OFFSETX, OFFSETY);
                            snakeCells.Add(snakeCell);
                        }
                        if ((secondSnake is not null) && (secondSnake.score > 0))
                        {
                            if (secondSnake.isGrowing == false)
                                secondSnakeCells.Remove(secondSnakeCells[0]);
                            SnakeCell secondSnakeCell = new SnakeCell(secondSnake.coordinates, cellSize, OFFSETX, OFFSETY);
                            secondSnakeCells.Add(secondSnakeCell);
                        }
                        if ((aiSnake is not null) && (aiSnake.score > 0))
                        {
                            if (aiSnake.isGrowing == false)
                                secondSnakeCells.Remove(secondSnakeCells[0]);
                            SnakeCell secondSnakeCell = new SnakeCell(aiSnake.coordinates, cellSize, OFFSETX, OFFSETY);
                            secondSnakeCells.Add(secondSnakeCell);
                        }
                        foreach (SnakeCell snakeCell in snakeCells)
                        {
                            //snakeCell.Reposition(snakeCell.bodySize); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        }

                        // Actualise les déplacements et les collisions des 3 snakes possible
                        for (int i = 0; i < snakes.Count; i++)
                        {
                            if (snakes[i] == aiSnake)
                            {
                                aiSnake.coordinates = aiSnake.Pathfinding(cells, walls, snakeCells, secondSnakeCells, apple, aiSnake.coordinates);
                            }
                            else
                            {
                                snakes[i].SnakeDirection();
                                snakes[i].UpdateCoordinates(nbColumns, nbRows);
                            }
                            snakes[i].UpdatePosition(cellSize, OFFSETX, OFFSETY);
                            snakes[i].isGrowing = false;
                            if (Collision.CheckAppleCollision(snakes[i], apple))
                            {
                                UpdateAppleTrigger();
                            }
                            if (Collision.CheckLoosingCollision(snakes[i], walls, snakeCells , secondSnakeCells))
                            {
                                gameOver = true;
                                if (i == 1)
                                    aiBeaten = true;
                                return;
                            }
                        }
                        if ((snakes.Count == 2) &&(snakes[0].coordinates == snakes[1].coordinates))
                        {
                            draw = true;
                        }
                    }
                }
            }
        }

        public void SceneInit(int x, int y) // Se lance à chaque changement de grille
        {
            // Réinitialise toutes les données

            cells.Clear(); walls.Clear(); choices.Clear(); snakes.Clear(); snakeCells.Clear(); secondSnakeCells.Clear();
            secondSnake = null; aiSnake = null;  apple = null; timer = 0; timeDelay = 2; remainingSpeedUp = speedUP; remainingApples = appleObjectif;
            isPaused = false; victory = false; gameOver = false; draw = false;

            // Consrtuction de la grille
            nbRows = level.grid.GetLength(0);
            nbColumns = level.grid.GetLength(1);
            OFFSETX = (SCREENWIDTH - (nbColumns * cellSize)) / 2;
            OFFSETY = (SCREENHEIGHT - (nbRows * cellSize)) / 2;
            for (int l = 0; l < nbRows; l++)
            {
                for (int c = 0; c < nbColumns; c++)
                {
                    GridCell cell = new GridCell(new Vector2(c, l), cellSize, OFFSETX, OFFSETY);
                    cells.Add(cell);
                    if (currentScene == sceneType.menu) // POUR UNLOCK
                    {
                        if (!recordBeaten)
                            level.grid[7, 4] = 1;
                        if (!levelModeBeaten)
                            level.grid[10, 7] = 1;
                        if (!aiBeaten)
                            level.grid[7, 10] = 1;
                    }
                    if (level.grid[l, c] == 1)
                        walls.Add(cell);
                    if (level.grid[l, c] == 2)
                        choices.Add(cell);
                }
            }
            foreach (GridCell choice in choices)
                choice.Reposition(cellSize);

            // Initialise les snakes qui doivent être présent
            snake = new Snake(new Vector2(x,y), cellSize, OFFSETX, OFFSETY);
            snakes.Add(snake);
            if ((level.currentLevel == Level.levelName.vsPlayer) || (level.vsPlayer == true))
            {
                secondSnake = new SecondSnake(new Vector2(nbColumns-2, nbRows/2), cellSize, OFFSETX, OFFSETY);
                snakes.Add(secondSnake);
            }
            if ((level.currentLevel == Level.levelName.vsAI) || (level.vsAI == true))
            {
                aiSnake = new AISnake(new Vector2(nbColumns - 2, nbRows / 2), cellSize, OFFSETX, OFFSETY);
                snakes.Add(aiSnake);
            }

            // génère la première pomme
            if (currentScene == sceneType.game)
            {
                apple = new Apple(nbColumns, nbRows);
                while (true)
                {
                    apple.CreateApplePositon(cellSize, OFFSETX, OFFSETY);
                    bool applePosValid = true;
                    foreach (GridCell wall in walls)
                    {
                        if (apple.coordinates == wall.coordinates)
                            applePosValid = false;
                    }
                    foreach (Snake snake in snakes)
                    {
                        if (apple.coordinates == snake.coordinates)
                            applePosValid = false;
                    }
                    if (applePosValid == true)
                        break;
                }
            }
        }
        void UpdateAppleTrigger()
        {
            // Gère augmentation vitesse (mode libre)
            if ((level.currentLevel == Level.levelName.free) || (level.currentLevel == Level.levelName.freeWall))
            {
                remainingSpeedUp--;
                if ((remainingSpeedUp <= 0) && (timeDelay >= 1))
                {
                    remainingSpeedUp = speedUP;
                    timeDelay -= 0.1f;
                }
            }

            // Gère diminution pommes restantes (mode niveaux)
            if (level.currentLevel == Level.levelName.story)
            {    
                remainingApples--;
                if (remainingApples <= 5)
                    timeDelay = 1;   
                else if (remainingApples <= 10)
                    timeDelay = 1.4f;
                else if (remainingApples <= 15)
                    timeDelay = 1.7f;
                    
                if (remainingApples <= 0)
                {
                    remainingApples = appleObjectif;
                    int x = 0;
                    int y = 0;
                    if (level.currentStory == Level.storyName.A)
                    {
                        x = 6;
                        y = 6;
                        level.currentStory = Level.storyName.B;
                        level.LevelB();
                    }
                    else if (level.currentStory == Level.storyName.B)
                    {
                        x = 6;
                        y = 6;
                        level.currentStory = Level.storyName.C;
                        level.LevelC();
                    }
                    else if (level.currentStory == Level.storyName.C)
                    {
                        x = 6;
                        y = 6;
                        level.currentStory = Level.storyName.D;
                        level.LevelD();
                    }
                    else if (level.currentStory == Level.storyName.D)
                    {
                        x = 5;
                        y = 5;
                        level.currentStory = Level.storyName.E;
                        level.LevelE();
                    }
                    else if (level.currentStory == Level.storyName.E)
                    {
                        victory = true;
                        levelModeBeaten = true;
                        return;                 
                    }
                    SceneInit(x, y);
                    return;
                }
            }

            // Création nouvelle pomme
            while (true)
            {
                apple.CreateApplePositon(cellSize, OFFSETX, OFFSETY);
                bool applePosValid = true;
                foreach (GridCell wall in walls)
                {
                    if (apple.coordinates == wall.coordinates)
                        applePosValid = false;
                }
                foreach (SnakeCell snakeCell in snakeCells)
                {
                    if (apple.coordinates == snakeCell.coordinates)
                        applePosValid = false;
                }
                foreach (SnakeCell secondSnakeCell in secondSnakeCells)
                {
                    if (apple.coordinates == secondSnakeCell.coordinates)
                        applePosValid = false;
                }
                foreach (Snake snake in snakes)
                {
                    if (apple.coordinates == snake.coordinates)
                        applePosValid = false;
                }

                if (applePosValid == true)
                    break;
            }
        }

        public void Draw()
        {
            //if (aiSnake is not null)
            //    aiSnake.draw();

            //if (aiSnake is not null)
            //    aiSnake.drawclose();



            // Affichage texte menu, pause/victoire/gameover
            if (currentScene == sceneType.menu)
            {
                Raylib.DrawText("Mode libre", SCREENWIDTH/2 - 130,100, 50, Color.Red);
                Raylib.DrawText("Sans limite", choices[0].posX-170, choices[0].posY-67, 30, Color.Red);
                Raylib.DrawText("Avec limite", choices[1].posX+30, choices[1].posY-67, 30, Color.Red);
                Raylib.DrawText("Mode niveaux", choices[2].posX - 430, SCREENHEIGHT/2-30, 50, Color.Red);
                Raylib.DrawText("Mode éditeur", choices[3].posX+120, SCREENHEIGHT / 2 - 30, 50, Color.Red);
                Raylib.DrawText("Mode versus", SCREENWIDTH / 2 - 160, 800, 50, Color.Red);
                Raylib.DrawText("Joueur vs joueur", choices[4].posX-270, choices[4].posY+55, 30, Color.Red);
                Raylib.DrawText("Joueur vs ordi", choices[5].posX+30, choices[5].posY+55, 30, Color.Red);
            }

            // Affiche texte mode niveaux
            if (level.currentLevel == Level.levelName.story)
            {
                if (remainingApples <= 5)
                {
                    Raylib.DrawRectangle(0, 0, SCREENWIDTH, SCREENHEIGHT, Color.DarkGray);
                }

                else if (remainingApples <= 10)
                {
                    Raylib.DrawRectangle(0, 0, SCREENWIDTH, SCREENHEIGHT, Color.Beige);
                }

                else if (remainingApples <= 15)
                {
                    Raylib.DrawRectangle(0, 0, SCREENWIDTH, SCREENHEIGHT, Color.RayWhite);
                }
                Raylib.DrawText("Pommes restantes : " + remainingApples, cells[nbColumns - 1].posX + 100, cells[nbColumns - 1].posY, 40, Color.Red);
            }
            if (isPaused)
            {
                Raylib.DrawText("PAUSE", SCREENWIDTH / 2 - 130, cells[0].posY - 130, 80, Color.Red);
                messageTimer += 0.1f;
                if (messageTimer > messageTimeDelay)
                {
                    messageTimer = 0;
                    showMessage = !showMessage;
                }
                if (showMessage)
                    Raylib.DrawText("Press 'P' to continue or press 'R' to return to the menu", 230 , cells[cells.Count-1].posY + 100, 50, Color.Red);
            }
            if (victory)
            {
                Raylib.DrawText("VICTOIRE", SCREENWIDTH / 2 - 200, cells[0].posY - 130, 80, Color.Red);
                messageTimer += 0.1f;
                if (messageTimer > messageTimeDelay)
                {
                    messageTimer = 0;
                    showMessage = !showMessage;
                }
                if (showMessage)
                    Raylib.DrawText("Press 'R' to return to the menu", SCREENWIDTH / 2 - 400, cells[cells.Count - 1].posY + 100, 50, Color.Red);
            }
            if (gameOver)
            {
                Raylib.DrawText("GAME OVER", SCREENWIDTH / 2 - 240, cells[0].posY - 130, 80, Color.Red);
                messageTimer += 0.1f;
                if (messageTimer > messageTimeDelay)
                {
                    messageTimer = 0;
                    showMessage = !showMessage;
                }
                if (showMessage)
                    Raylib.DrawText("Press 'R' to return to the menu", SCREENWIDTH/2 - 400, cells[cells.Count - 1].posY + 100, 50, Color.Red);
            }
            if (draw)
            {
                Raylib.DrawText("MATCH NUL", SCREENWIDTH / 2 - 230, cells[0].posY - 130, 80, Color.Red);
                messageTimer += 0.1f;
                if (messageTimer > messageTimeDelay)
                {
                    messageTimer = 0;
                    showMessage = !showMessage;
                }
                if (showMessage)
                    Raylib.DrawText("Press 'R' to return to the menu", SCREENWIDTH / 2 - 400, cells[cells.Count - 1].posY + 100, 50, Color.Red);
            }

            // Affiche texte mode libre
            if ((level.currentLevel == Level.levelName.free) || (level.currentLevel == Level.levelName.freeWall))
            {
                Raylib.DrawText(" Score : " + snake.score, cells[0].posX - 300, cells[0].posY, 40, Color.Red);
                Raylib.DrawText(" Record : " + record, cells[nbColumns-1].posX+100, cells[nbColumns-1].posY, 40, Color.Red);
            }

            // Affiche texte mode éditeur
            if (level.currentStep == mapEditorStep.sizeSet)
            {
                Raylib.DrawText("Selectionner le nombre de colonnes avec haut/bas et le nombre de rangées avec gauche/droite. Appuyer sur Entrer pour valider.", 280, 0, 20, Color.Red);
            }
            else if (level.currentStep == mapEditorStep.opponent)
            {
                Raylib.DrawText("Selectionner le type de partie. Haut pour le mode solo; gauche pour joueur VS joueur; droite pour joueur VS ordi. Appuyer sur Entrer pour valider.", 185, 0, 20, Color.Red);
                if ((!level.vsPlayer) && (!level.vsAI))
                    Raylib.DrawText("SOLO", SCREENWIDTH/2 - 50, 50, 40, Color.Red);
                else if (level.vsPlayer)
                    Raylib.DrawText("VS PLAYER", SCREENWIDTH / 2 - 100, 50, 40, Color.Red);
                else if (level.vsAI)
                    Raylib.DrawText("VS AI", SCREENWIDTH / 2 - 55, 50, 40, Color.Red);

            }
            else if (level.currentStep == mapEditorStep.wallSet)
            {
                Raylib.DrawText("Placer les murs en faisant un clic gauche avec la souris sur les cases souhaitées. Appuyer de nouveau pour effacer les murs. Appuyer sur Entrer pour valider.", 150, 0, 20, Color.Red);
            }

            // Affiche tous les types de cellules possibles
            foreach (GridCell gridCell in cells)
                gridCell.Draw();

            foreach (GridCell wall in walls)
                wall.DrawWall();

            foreach (GridCell choice in choices)
                choice.DrawChoice();

            foreach (Snake snake in snakes)
                snake.Draw();

            foreach (SnakeCell snakeCell in snakeCells)
                snakeCell.Draw();

            foreach (SnakeCell secondSnakeCell in secondSnakeCells)
                secondSnakeCell.DrawSecond();
            if (apple is not null)
                apple.Draw();
        }
    }
}
    
