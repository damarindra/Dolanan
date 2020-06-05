using System;
using Dolanan;

namespace Platform_WinDX
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting Win DX");
            using (var game = new Dolanan.GameMin())
                game.Run();
        }
    }
}
