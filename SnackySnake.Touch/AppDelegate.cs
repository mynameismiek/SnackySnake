using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SnackySnake.Touch
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        SnackySnakeGame game;

        /// <remarks>>
        /// Create the Snacky Snake game and run it.
        /// </remarks>
        public override void FinishedLaunching(UIApplication application)
        {
            game = new SnackySnakeGame();
            game.Run();
        }
    }
}

