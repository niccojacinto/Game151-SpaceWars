#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace SpaceWars {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        //A Game1 object that acts a singleton.
        //It can be accessed by other classes and exposes
        //the current active screen.
        public static Game1 game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (game = new Game1())
                game.Run();
        }//static void Main()
    }//public static class Program
#endif
}//namespace SpaceWars {
