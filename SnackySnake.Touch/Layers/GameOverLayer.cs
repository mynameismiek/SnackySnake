using System;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// Game over layer.
    /// </summary>
    public class GameOverLayer : CCLayerColor
    {
        /// <summary>
        /// Gets or sets the max score.
        /// </summary>
        /// <value>The max score.</value>
        public static int MaxScore { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// Note: Set me before getting the scene!
        /// </summary>
        /// <value>The score.</value>
        public static int Score { get; set; }

        public GameOverLayer()
        {
            var screenSize = CCDirector.SharedDirector.WinSize;

            // show "game over"
            var gameOverLabel = new CCLabel("GAME OVER", "arial", 48f);
            gameOverLabel.Position = new CCPoint(screenSize.Center.X, screenSize.Height / 3f);
            gameOverLabel.Color = new CCColor3B(XNA.Color.White);
            AddChild(gameOverLabel);

            // show "score"
            // show restart button
            // show main menu button

            Color = new CCColor3B(XNA.Color.Black);
            Opacity = 128;
        }
    }
}

