using System;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;
using SnackySnake.Touch.Models;
using System.Collections.Generic;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// Game over layer.
    /// </summary>
    public class GameOverLayer : CCLayerColor
    {
        /// <summary>
        /// Gets the game scene for this layer.
        /// </summary>
        /// <value>The scene.</value>
        public static CCScene Scene
        {
            get 
            {
                var scene = new CCScene();
                var layer = new GameOverLayer();
                scene.AddChild(layer);

                return scene;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.Layers.GameOverLayer"/> class.
        /// </summary>
        public GameOverLayer()
        {
            TouchEnabled = true;
            var screenSize = CCDirector.SharedDirector.WinSize;
            var didWin = (Scores.EatenApples == Scores.MaxApples);

            // show "game over"
            var title = didWin ? "CCONGRATULATIONS!" : "GAME OVER";
            var gameOverLabel = new CCLabel(title, "arial", 48f)
            {
                Position = new CCPoint(screenSize.Center.X, screenSize.Height * 2f / 3f),
                Color = new CCColor3B(XNA.Color.White)
            };
            AddChild(gameOverLabel);

            // show scores
            int min = (int)(Scores.Time / 60f);
            int sec = (int)(Scores.Time % 60f);
            var scoreLabel = new CCLabel(String.Format("You ate {0} of {1} apples in {2:00}:{3:00}", Scores.EatenApples, Scores.MaxApples, min, sec), 
                "MarkerFelt", 
                22f)
            {
                Position = screenSize.Center,
                Color = new CCColor3B(XNA.Color.White)
            };
            AddChild(scoreLabel);

            Color = didWin ? new CCColor3B(XNA.Color.DarkGreen) : new CCColor3B(XNA.Color.DarkRed);
            Opacity = 128;
        }

        /// <remarks>>
        /// Transition to the Game Scene.
        /// </remarks>
        public override void TouchesEnded(List<CCTouch> touches)
        {
            base.TouchesEnded(touches);

            CCDirector.SharedDirector.ReplaceScene(GameLayer.Scene);
        }
    }
}

