using System;
using Microsoft.Xna.Framework;

namespace SnackySnake.Touch
{
    /// <summary>
    /// Snacky Snake game.
    /// </summary>
    public class SnackySnakeGame : Game 
    {
        readonly GraphicsDeviceManager _graphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.SnackySnakeGame"/> class.
        /// </summary>
        public SnackySnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = true;
            var ssApp = new SnackySnakeApp(this, _graphics);
            Components.Add(ssApp);
        }
    }
}

