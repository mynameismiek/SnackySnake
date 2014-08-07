using System;
using System.Collections.Generic;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// The main menu layer.
    /// </summary>
    public class MainMenuLayer : CCLayerColor
    {
        /// <summary>
        /// Gets the game start scene for this layer.
        /// </summary>
        /// <value>The scene.</value>
        public static CCScene Scene
        {
            get 
            {
                var scene = new CCScene();
                var layer = new MainMenuLayer();
                scene.AddChild(layer);

                return scene;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.Layers.MainMenuLayer"/> class.
        /// </summary>
        private MainMenuLayer()
        {
            TouchEnabled = true;
            var screenSize = CCDirector.SharedDirector.WinSize;

            var title = new CCLabelTTF("Snacky Snake", "arial", 64f)
            {
                Position = screenSize.Center,
                Color = new CCColor3B(XNA.Color.White)
            };
            AddChild(title);

            var directions = new CCLabelTTF("Tap anywhere to Play!", "MarkerFelt", 22f)
            {
                Position = new CCPoint(screenSize.Center.X, screenSize.Height / 3f),
                Color = new CCColor3B(XNA.Color.LightGray)
            };
            AddChild(directions);

            var bymiekLabel = new CCLabelTTF("by Michael Hope", "MarkerFelt", 22f)
            {
                AnchorPoint = new CCPoint(0f, 0f),
                Position = new CCPoint(4f, 0f),
                Color = new CCColor3B(XNA.Color.LightGray)
            };
            AddChild(bymiekLabel);

            // layer background
            Color = new CCColor3B(XNA.Color.CornflowerBlue); // for old times sake
            Opacity = 255;
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

