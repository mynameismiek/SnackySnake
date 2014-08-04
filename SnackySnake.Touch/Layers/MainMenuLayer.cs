using System;
using System.Collections.Generic;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;

namespace SnackySnake.Touch.Layers
{
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

            var title = new CCLabelTTF("Tap to start snacking!", "MarkerFelt", 22)
            {
                Position = CCDirector.SharedDirector.WinSize.Center,
                Color = new CCColor3B(XNA.Color.White)
            };

            AddChild(title);

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

