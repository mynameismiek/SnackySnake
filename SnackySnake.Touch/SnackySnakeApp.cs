using System;
using Cocos2D;
using Microsoft.Xna.Framework;
using SnackySnake.Touch.Layers;

namespace SnackySnake.Touch
{
    /// <summary>
    /// Snacky snake application.
    /// </summary>
    public class SnackySnakeApp : CCApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.SnackySnakeApp"/> class.
        /// </summary>
        /// <param name="game">Snacky Snake Game.</param>
        /// <param name="graphics">Graphics.</param>
        public SnackySnakeApp(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            s_pSharedApplication = this;
            CCDrawManager.InitializeDisplay(game, graphics, DisplayOrientation.LandscapeRight);
        }

        #region App Life Cycle

        /// <remarks>>
        /// Sets up the shared director for the snacky snake game.
        /// </remarks>
        public override bool ApplicationDidFinishLaunching()
        {
            var director = CCDirector.SharedDirector;
            director.SetOpenGlView();
            director.Projection = CCDirectorProjection.Projection2D;
            director.DisplayStats = false;
            var scene = MainMenuLayer.Scene;
            director.RunWithScene(scene);

            return true;
        }

        /// <remarks>>
        /// Have the shared director pause the game.
        /// </remarks>
        public override void ApplicationDidEnterBackground()
        {
            CCDirector.SharedDirector.Pause();
        }

        /// <remarks>
        /// Have the shared director resume the game.
        /// </remarks>
        public override void ApplicationWillEnterForeground()
        {
            CCDirector.SharedDirector.Resume();
        }

        #endregion
    }
}

