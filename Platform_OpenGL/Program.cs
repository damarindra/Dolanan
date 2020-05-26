using System;
using CoreGame;

namespace Platform_OpenGL
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting OpenGL");
            using (var game = new CoreGame.GameClient())
                game.Run();
        }
    }
}
