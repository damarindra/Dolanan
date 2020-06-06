using System;

namespace DolananSample
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TopDown())
                game.Run();
        }
    }
}
