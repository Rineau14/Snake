using Raylib_cs;
using System.Numerics;
using static Snake.Level;

namespace Snake
{
    class GameManager
    {
        public int SCREENWIDTH = 1920;
        public int SCREENHEIGHT = 950;
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
        Level level = new Level();
        Snake snake;
        SecondSnake? secondSnake;
        AISnake? aiSnake;
        Apple? apple;

        bool menuLoaded = false;
        bool isPaused = false;
        bool victory = false;
        bool gameOver = false;

        int record = 50;
        int speedUP = 10;
        int remainingSpeedUp;
        int appleObjectif = 20;
        int remainingApples;

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
                    SceneInit();
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
                    if (level.currentLevel != levelName.mapEditor)
                    {
                        currentScene = sceneType.game;
                        SceneInit();
                    }
                    else
                        currentScene = sceneType.mapEditor;
                }
            }

            // Editeur de map
            if (currentScene == sceneType.mapEditor)
            {
                level.MapEditor(cells);
                SceneInit();
                if (level.currentStep == mapEditorStep.ready)
                {
                    currentScene = sceneType.game;
                    SceneInit();
                }
            }

            // En jeu
            if (currentScene == sceneType.game)
            {
                // Gère pause/victoire/gameover
                if (Raylib.IsKeyPressed(KeyboardKey.P))
                {
                    isPaused = !isPaused;
                }
                if (((isPaused) || (victory) || (gameOver)) && (Raylib.IsKeyPressed(KeyboardKey.R)))
                {
                    currentScene = sceneType.menu;
                    level.currentLevel = Level.levelName.none;
                    level.Menu();
                    SceneInit();
                    return;
                }

                // Déroulement d'une partie
                if ((!isPaused) && (!victory) && (!gameOver))
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

                        // Test pendant débug (non utilisé)
                        if (level.currentLevel == Level.levelName.vsAI)
                        {
                            //if (Raylib.IsKeyDown(KeyboardKey.K))
                            //{
                                //aiSnake.coordinates = aiSnake.Pathfinding(cells, walls, snakeCells, secondSnakeCells, apple, aiSnake.coordinates);
       
                                //aiSnake.UpdatePosition(cellSize, OFFSETX, OFFSETY);
                                //aiSnake.isGrowing = false;
                                //CheckAppleCollision(1);
                            //}
                            
                            //aiSnake.coordinates = aiMove.coordinates;
                            //aiSnake.UpdatePosition(cellSize, OFFSETX, OFFSETY);
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
                                return;
                            }
                        }
                    }
                }
            }
        }

        
        public void SceneInit(int x = 0, int y = 0) // Se lance à chaque changement de grille
        {
            // Réinitialise toutes les données
            cells.Clear(); walls.Clear(); choices.Clear(); snakes.Clear(); snakeCells.Clear(); secondSnakeCells.Clear();
            apple = null; timer = 0; timeDelay = 2; remainingSpeedUp = speedUP; remainingApples = appleObjectif;
            isPaused = false; victory = false; gameOver = false;

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
                    if (level.grid[l, c] == 1)
                        walls.Add(cell);
                    if (level.grid[l, c] == 2)
                        choices.Add(cell);
                }
            }
            foreach (GridCell choice in choices)
                choice.reposition(cellSize);

            // Initialise les snakes qui doivent être présent
            snake = new Snake(new Vector2(nbColumns / 2 + x, nbRows / 2 + y), cellSize, OFFSETX, OFFSETY);
            snakes.Add(snake);
            if (level.currentLevel == Level.levelName.vsPlayer)
            {
                secondSnake = new SecondSnake(new Vector2(17, 10), cellSize, OFFSETX, OFFSETY);
                snakes.Add(secondSnake);
            }
            if (level.currentLevel == Level.levelName.vsAI)
            {
                aiSnake = new AISnake(new Vector2(4, 4), cellSize, OFFSETX, OFFSETY);
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
                        x = 1;
                        level.currentStory = Level.storyName.B;
                        level.LevelB();
                    }
                    else if (level.currentStory == Level.storyName.B)
                    {
                        x = 1;
                        y = 1;
                        level.currentStory = Level.storyName.C;
                        level.LevelC();
                    }
                    else if (level.currentStory == Level.storyName.C)
                    {
                        level.currentStory = Level.storyName.D;
                        level.LevelD();
                    }
                    else if (level.currentStory == Level.storyName.D)
                    {
                        level.currentStory = Level.storyName.E;
                        level.LevelE();
                    }
                    else if (level.currentStory == Level.storyName.E)
                    {
                        victory = true;
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

                if (applePosValid == true)
                    break;
            }
        }

        public void Draw()
        {
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

            //if (aiSnake is not null)
            //    aiSnake.draw();

            //if (aiSnake is not null)
            //    aiSnake.drawclose();

            if (apple is not null)
                apple.Draw();



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
                Raylib.DrawText("GAMEOVER", SCREENWIDTH / 2 - 230, cells[0].posY - 130, 80, Color.Red);
                messageTimer += 0.1f;
                if (messageTimer > messageTimeDelay)
                {
                    messageTimer = 0;
                    showMessage = !showMessage;
                }
                if (showMessage)
                    Raylib.DrawText("Press 'R' to return to the menu", SCREENWIDTH/2 - 400, cells[cells.Count - 1].posY + 100, 50, Color.Red);
            }

            // Affiche texte mode libre
            if ((level.currentLevel == Level.levelName.free) || (level.currentLevel == Level.levelName.freeWall))
            {
                //Raylib.DrawText("time " + timeDelay, SCREENWIDTH / 2 - 100, cells[0].posY - 150, 40, Color.Red);
                Raylib.DrawText(" Score : " + snake.score, cells[0].posX - 300, cells[0].posY, 40, Color.Red);
                Raylib.DrawText(" Record : " + record, cells[nbColumns-1].posX+100, cells[nbColumns-1].posY, 40, Color.Red);
                // nombre colonnes !!!!!! sans les murs
            }
            
            // Affiche texte mode niveaux
            if (level.currentLevel == Level.levelName.story)
                Raylib.DrawText("Pommes restantes : " + remainingApples, cells[nbColumns - 1].posX + 100, cells[nbColumns - 1].posY, 40, Color.Red);

            // Affiche texte mode éditeur
            if (level.currentStep == mapEditorStep.sizeSet)
            {
                Raylib.DrawText("Selectionner le nombre de colonnes avec haut/bas et le nombre de rangées avec gauche/droite.", 450, 0, 20, Color.Red);
            }
        }
    }
}
    
