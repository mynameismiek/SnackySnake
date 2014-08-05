using System;
using System.Collections.Generic;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;
using SnackySnake.Touch.Models;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// The game layer.
    /// </summary>
    public class GameLayer : CCLayerColor
    {
        private const int SquareSizeHD = 32;
        private readonly int NumSquaresWide;
        private readonly int NumSquaresTall;
        private readonly float X_Offset;
        private readonly float Y_Offset;

        private List<CCSprite> _borders;
        private Snake _snake;
        private CCSprite _apple;
        private int _maxScore;
        private int _score;
        private PauseLayer _pauseLayer = null;

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
            // figure out the board size
            var screenSize = CCDirector.SharedDirector.VisibleSize;

            // TODO: add support for non-retina iPhones
            // TOOD: add support for iPads
            NumSquaresWide = (int)Math.Floor(screenSize.Width / SquareSizeHD);
            NumSquaresTall = (int)Math.Floor(screenSize.Height / SquareSizeHD);

            // figure out the x and y offsets in case we can't divide the screen up evenly
            X_Offset = (screenSize.Width - (NumSquaresWide * SquareSizeHD)) / 2f; 
            Y_Offset = (screenSize.Height - (NumSquaresTall * SquareSizeHD)) / 2f;

            // layer background
            Color = new CCColor3B(new XNA.Color(202, 165, 128)); // sandy
            Opacity = 255;

            // add borders
            AddBorders();
            Reset();
        }

        /// <summary>
        /// Reset the game.
        /// </summary>
        public void Reset()
        {
            if (_pauseLayer != null)
            {
                _pauseLayer.RemoveFromParent();
            }
            _score = 0;
            TouchEnabled = true;
            AddChild(new Snake());

            _apple = GetApple();
            AddChild(_apple);

            var goLabel = new CCLabelTTF("GO!", "arial", 64f)
            {
                Position = CCDirector.SharedDirector.WinSize.Center,
                Color = new CCColor3B(XNA.Color.White)
            };
            AddChild(goLabel);
            CCFadeOut fade = new CCFadeOut(1.5f);
            goLabel.RunAction(fade);
            ResumeSchedulerAndActions();
        }

        /// <summary>
        /// Resume the game.
        /// </summary>
        public void Resume()
        {
            _pauseLayer.RemoveFromParent();
            TouchEnabled = true;
            ResumeSchedulerAndActions();
        }

        /// <summary>
        /// Adds the borders to the scene.
        /// </summary>
        private void AddBorders()
        {
            _borders = new List<CCSprite>();
            _maxScore = 0;

            for (int y = 0; y < NumSquaresTall; y++)
            {
                for (int x = 0; x < NumSquaresWide; x++)
                {
                    // skip (1,1) through (NumSquaresWide - 1, NumSquaresTall - 1)
                    if (x > 0 && 
                        y > 0 &&
                        x < NumSquaresWide - 1 &&
                        y < NumSquaresTall - 1)
                    {
                        _maxScore++;
                        continue;
                    }

                    var border = new CCSprite(new CCSize(SquareSizeHD, SquareSizeHD));
                    border.Name = String.Format("__border__({0},{1})", x, y);
                    border.Color = new CCColor3B(XNA.Color.Orange);
                    border.AnchorPoint = new CCPoint(0f, 0f);
                    border.Opacity = 255;
                    border.Position = new CCPoint(x * SquareSizeHD + X_Offset, 
                                                  y * SquareSizeHD + Y_Offset);

                    AddChild(border);
                    _borders.Add(border);
                }
            }
        }

        /// <summary>
        /// Gets a new apple.
        /// </summary>
        /// <returns>The apple.</returns>
        private CCSprite GetApple()
        {

            var sprite = new CCSprite();
            sprite.InitWithFile("Images/Apple-hd.png");
            // TODO: position randomly that isnt a wall or a snake part.
            //   1 <= x <= NumSquaresWide - 1
            //   1 <= y <= NumSquaresTall - 1
            sprite.Position = CCDirector.SharedDirector.WinSize.Center;
  
            return sprite;
        }

        /// <summary>
        /// Update the layer for the specified delta time.
        /// </summary>
        /// <param name="dt">Delta Time.</param>
        public override void Update(float dt)
        {
            base.Update(dt);

            // move snake


            CheckCollisions();
        }

        public override void TouchesEnded(List<CCTouch> touches)
        {
            base.TouchesEnded(touches);

            if (touches.Count > 1)
            {
                HandlePause();
            }
            else 
            {
                // figure out where the snake head is in relation to the touch
                // and schedule the snake head to go in that direction when

            }
        }

        /// <summary>
        /// Check for collisions between the snake head, snake body, borders, an apple.
        /// </summary>
        private void CheckCollisions()
        {
            // check that the snake head is not colliding with any borders
            foreach (var b in _borders)
            {
//                var hit = b.BoundingBox.IntersectsRect();
//                if (hit) // game over bro
//                {
//                    HandleGameOver();
//                }
            }

            // check that the snake head is not colliding with any snake body parts
//            foreach (var bodypart in )
//            {
//                var hit = bodyPart.BoundingBox.IntersectsRect();
//                if (hit) // game over bro
//                {
//                    HandleGameOver();
//                }
//            }

            // check that the snake is eating the apple
//            var appleHit = _apple.BoundingBox.IntersectsRect();
//            if (appleHit)
//            {
//                HandleAppleHit();
//            }
        }

        /// <summary>
        /// Handles when the snake head touches an apple.
        /// </summary>
        private void HandleAppleHit()
        {
            _apple.RemoveFromParent();
            _score++;

            if (_score == _maxScore) // player has reached the killscreen!
            {
                HandleGameOver();
            }
            else 
            {
                _apple = GetApple();
                AddChild(_apple);
            }
        }

        /// <summary>
        /// Handles when the game is paused.
        /// </summary>
        private void HandlePause()
        {
            TouchEnabled = false;
            PauseSchedulerAndActions();
            _pauseLayer = new PauseLayer(this);
            _pauseLayer.Position = new CCPoint(0f, 0f);
            AddChild(_pauseLayer);
        }

        /// <summary>
        /// Loads the game over layer when the player has run into his/herself or the borders.
        /// </summary>
        private void HandleGameOver()
        {
            // set the score and max score and let the game over layer figure out what to do with them
            GameOverLayer.MaxScore = _maxScore;
            GameOverLayer.Score = _score;

        }
    }
}

