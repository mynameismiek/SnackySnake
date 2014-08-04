using System;
using System.Collections.Generic;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// The game layer.
    /// </summary>
    public class GameLayer : CCLayerColor
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
                var layer = new GameLayer();
                scene.AddChild(layer);

                return scene;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.Layers.GameLayer"/> class.
        /// </summary>
        private GameLayer()
        {
            // TODO: remove this once its further along.
            var title = new CCLabelTTF("Game Layer", "MarkerFelt", 22)
            {
                Position = CCDirector.SharedDirector.WinSize.Center,
                Color = new CCColor3B(XNA.Color.GreenYellow)
            };

            AddChild(title);

            // layer background
            Color = new CCColor3B(new XNA.Color(202, 165, 128)); // sandy
            Opacity = 255;
        }
    }
}

