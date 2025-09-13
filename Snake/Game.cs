using Raylib_cs;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Snake
{
    class Game
    {
        static void Main(string[] args)
        {
            Raylib.InitAudioDevice();
            Music music = Raylib.LoadMusicStream("musics/music");
            Raylib.PlayMusicStream(music);
            int SCREENWIDTH = 1920;
            int SCREENHEIGHT = 950;
            
            Raylib.InitWindow(SCREENWIDTH, SCREENHEIGHT, "SNAKE");
            Raylib.SetTargetFPS(60);

            GameManager scene = new GameManager();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.UpdateMusicStream(music);
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                scene.Update();
                scene.Draw();

                Raylib.EndDrawing();
                if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    Raylib.UnloadMusicStream(music);
                    Raylib.CloseAudioDevice();
                    Raylib.CloseWindow();
                }
            }
        }
    }
}
    

