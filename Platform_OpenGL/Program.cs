using System;
using Dolanan;

namespace Platform_OpenGL
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting OpenGL");
            using (var game = new Dolanan.GameMin())
                game.Run();
        }
    }
}
