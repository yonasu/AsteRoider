using System;

namespace AsteRoider
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AsteRoiderGame game = new AsteRoiderGame())
            {
                game.Run();
            }
        }
    }
#endif
}

