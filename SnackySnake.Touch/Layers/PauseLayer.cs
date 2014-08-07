using System;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// The pause menu. 
    /// </summary>
    public class PauseLayer : CCMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.Layers.PauseLayer"/> class.
        /// </summary>
        /// <param name="game">Game.</param>
        public PauseLayer(GameLayer game)
        {
            TouchEnabled = true;
            var screenSize = CCDirector.SharedDirector.WinSize;
            Color = new CCColor3B(XNA.Color.Black);

            // PAUSE!
            var pauseLabel = new CCMenuItemLabel(new CCLabel("PAUSE", "arial", 48f));
            pauseLabel.Position = new CCPoint(screenSize.Center.X, screenSize.Height * 2f / 3f);
            pauseLabel.Color = new CCColor3B(XNA.Color.White);
            AddChild(pauseLabel);       

            // show main menu button
            var mainMenuBtn = new CCMenuItemImage("Images/Exit-normal-hd.png", "Images/Exit-pressed-hd.png", delegate
            {
                CCDirector.SharedDirector.ReplaceScene(MainMenuLayer.Scene);
            });
            mainMenuBtn.Position = screenSize.Center;
            AddChild(mainMenuBtn);

            // show reset button
            var resetBtn = new CCMenuItemImage("Images/Reset-hd.png", "Images/Reset-hd.png", delegate
            {
                game.Reset();
            });
            resetBtn.Position = new CCPoint(screenSize.Width / 3f, 
                                            screenSize.Height / 3f);
            resetBtn.AnchorPoint = new CCPoint(1f, 1f);
            AddChild(resetBtn);

            // show resume button
            var resumeBtn = new CCMenuItemImage("Images/Play-hd.png", "Images/Play-hd.png", delegate
            {
                game.Resume();
            });
            resumeBtn.Position = new CCPoint(screenSize.Width * 2f / 3f,
                                             screenSize.Height / 3f);
            resumeBtn.AnchorPoint = new CCPoint(0f, 1f);
            AddChild(resumeBtn);
        }
    }
}

