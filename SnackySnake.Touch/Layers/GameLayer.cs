using System;
using System.Collections.Generic;
using Cocos2D;
using XNA = Microsoft.Xna.Framework;
using SnackySnake.Touch.Models;
using SnackySnake.Touch.Utilities;
using CocosDenshion;

namespace SnackySnake.Touch.Layers
{
    /// <summary>
    /// The game layer.
    /// </summary>
    public class GameLayer : CCLayerColor
    {
        #region Class Constants and Variables

        private const int SquareSizeHD = 32;
        private const string TIMER_STRING = "Time:  {0:00}:{1:00}";
        private const string APPLE_STRING = "Apples: {0} of {1}";
        private readonly int NumSquaresWide;
        private readonly int NumSquaresTall;
        private readonly float X_Offset;
        private readonly float Y_Offset;

        // cache the corners of the screen
        private readonly CCPoint _bottomLeftCorner;
        private readonly CCPoint _topLeftCorner;
        private readonly CCPoint _topRightCorner;
        private readonly CCPoint _bottomRightCorner;

        private List<CCSprite> _borders;
        private Snake _snake;
        private CCSprite _apple;
        private int _maxApples;
        private int _eatenApples;
        private PauseLayer _pauseLayer = null;
        private GameOverLayer _gameOverLayer = null;
        private float _elapsedTime;
        private CCLabelTTF _timeLabel;
        private CCLabelTTF _appleLabel;
        private bool _isPaused;

        #endregion

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

            // cache the corners
            _bottomLeftCorner = new CCPoint(0f, 0f);
            _topLeftCorner = new CCPoint(0f, screenSize.Height);
            _topRightCorner = new CCPoint(screenSize.Width, screenSize.Height);
            _bottomRightCorner = new CCPoint(screenSize.Width, 0f);

            // layer background
            Color = new CCColor3B(new XNA.Color(202, 165, 128)); // sandy
            Opacity = 255;

            // add borders
            AddBorders();

            // add timer label
            _timeLabel = new CCLabelTTF()
            {
                AnchorPoint = new CCPoint(0f, 0f),
                Color = new CCColor3B(XNA.Color.White),
                FontName = "MarkerFelt",
                FontSize = 22f,
                HorizontalAlignment = CCTextAlignment.Left,
                Position = new CCPoint(14f, -6f),
                Text = String.Format(TIMER_STRING, 0, 0)
            };
            AddChild(_timeLabel);

            // add apple label
            _appleLabel = new CCLabelTTF()
            {
                AnchorPoint = new CCPoint(1f, 0f),
                Color = new CCColor3B(XNA.Color.White),
                FontName = "MarkerFelt",
                FontSize = 22f,
                HorizontalAlignment = CCTextAlignment.Right,
                Position = new CCPoint(screenSize.Width - 14f, -6f),
                Text = String.Format(APPLE_STRING, 0, _maxApples)
            };
            AddChild(_appleLabel);

            // add the player snake
            _snake = new Snake(new CCSize(X_Offset, Y_Offset), SquareSizeHD);
            AddChild(_snake);

            // load music
            CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic("Sounds/Foodcomputer_Short-Stop (Disconnected-mix).mp3");

            Reset();
        }

        /// <summary>
        /// Reset the game to the initial state.
        /// </summary>
        public void Reset()
        {
            TouchEnabled = true;

            if (_pauseLayer != null)
            {
                _pauseLayer.RemoveFromParentAndCleanup(true);
            }

            if (_gameOverLayer != null)
            {
                _gameOverLayer.RemoveFromParentAndCleanup(true);
            }

            _snake.Reset();
            Scores.Reset();

            if (_apple != null)
            {
                _apple.RemoveFromParentAndCleanup(true);
            }
            _apple = GetAnApple();
            AddChild(_apple);

            // label telling the player its time to get going!
            var goLabel = new CCLabelTTF("GO!", "arial", 64f)
            {
                Position = CCDirector.SharedDirector.WinSize.Center,
                Color = new CCColor3B(XNA.Color.White)
            };
            AddChild(goLabel);
            var goFade = new CCFadeOut(1.5f);
            var goRemove = new CCCallFuncN(node => node.RemoveFromParentAndCleanup(true));
            var goSeq = new CCSequence(goFade, goRemove); 
            goLabel.RunAction(goSeq);

            _elapsedTime = 0f;
            _eatenApples = 0;

            ScheduleUpdate();
            ResumeSchedulerAndActions();
            CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic("Sounds/Foodcomputer_Short-Stop (Disconnected-mix).mp3", true);
            _isPaused = false;
        }

        /// <summary>
        /// Resume the game.
        /// </summary>
        public void Resume()
        {
            _pauseLayer.RemoveFromParent();
            TouchEnabled = true;
            ScheduleUpdate();
            ResumeSchedulerAndActions();
            CCSimpleAudioEngine.SharedEngine.ResumeBackgroundMusic();
            _isPaused = false;
        }

        /// <summary>
        /// Adds the borders to the scene.
        /// </summary>
        private void AddBorders()
        {
            _borders = new List<CCSprite>();
            _maxApples = 0;

#if DEBUG
            bool alternateColor = false;
#endif

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
                        _maxApples++;
#if DEBUG // checkerboard pattern to make it easier to figure out where things are
                        var debugGridCell = new CCSprite(new CCSize(SquareSizeHD, SquareSizeHD))
                        {
                            Color = alternateColor ? new CCColor3B(XNA.Color.AliceBlue) : new CCColor3B(XNA.Color.Beige), 
                            AnchorPoint = new CCPoint(0f, 0f),
                            Opacity = 255,
                            Position = new CCPoint(x * SquareSizeHD + X_Offset, 
                                                   y * SquareSizeHD + Y_Offset)
                        };
                        AddChild(debugGridCell);
                        alternateColor = !alternateColor;
#endif
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

            _maxApples -= 3; // we have 3 snake body segments to start out
        }

        /// <summary>
        /// Gets a new apple.
        /// </summary>
        /// <returns>The apple.</returns>
        private CCSprite GetAnApple()
        {
            var sprite = new CCSprite();
            sprite.InitWithFile("Images/Apple-hd.png");
            sprite.AnchorPoint = new CCPoint(0f, 0f);
            var rand = new Random();
            float x, y;

            do
            {
                x = (float)rand.Next(1, NumSquaresWide) * SquareSizeHD + X_Offset;
                y = (float)rand.Next(1, NumSquaresTall) * SquareSizeHD + Y_Offset;
                sprite.Position = new CCPoint(x, y);
            } while (!IsAppleSpawnPositionOK(sprite));
  
            return sprite;
        }

        /// <summary>
        /// Checks to see if the sprite is colliding with any borders or snake body segments.
        /// 
        /// Note: this gets more inefficient the longer the snake gets, its good enough for now though
        /// </summary>
        /// <returns><c>true</c> if the sprite position is OK; otherwise, <c>false</c>.</returns>
        /// <param name="sprite">Sprite.</param>
        private bool IsAppleSpawnPositionOK(CCSprite sprite)
        {
            var isOK = true;
            var spriteRect = CollisionUtils.GetCorrectedBoundingBoxBL(sprite);
            bool hit;

            // make SURE that apple isnt colliding with the borders
            foreach (var b in _borders)
            {
                var bRect = CollisionUtils.GetCorrectedBoundingBoxBL(b);
                hit = spriteRect.IntersectsRect(bRect);
                if (hit) 
                {
                    isOK = false;
                    break;
                }
            }

            // check the snake head
            hit = spriteRect.IntersectsRect(_snake.GetHeadBox());
            if (hit)
            {
                isOK = false;
            }
            else // check the rest of the snake body
            {
                var bodySegments = _snake.GetBodyAndTailBoundingBoxes();
                foreach (var bodyPart in bodySegments)
                {
                    hit = spriteRect.IntersectsRect(bodyPart);
                    if (hit)
                    {
                        isOK = false;
                        break;
                    }
                }
            }

            return isOK;
        }

        /// <summary>
        /// Check for collisions between the snake head, snake body, borders, an apple.
        /// </summary>
        private void CheckCollisions()
        {
            var snakeRect = _snake.GetHeadBox();
            bool hit;

            // check if the snake head is not colliding with any borders
            foreach (var b in _borders)
            {
                var bRect = CollisionUtils.GetCorrectedBoundingBoxBL(b);
                hit = snakeRect.IntersectsRect(bRect);
                if (hit) // game over bro
                {
                    HandleGameOver();
                    return;
                }
            }

            // check if the snake head is not colliding with any snake body parts
            foreach (var bodypart in _snake.GetBodyAndTailBoundingBoxes())
            {
                hit = snakeRect.IntersectsRect(bodypart);
                if (hit) // game over bro
                {
                    HandleGameOver();
                    return;
                }
            }

            // check if the snake is eating the apple
            var appleRect = CollisionUtils.GetCorrectedBoundingBoxBL(_apple);
            var appleHit = snakeRect.IntersectsRect(appleRect);
            if (appleHit)
            {
                HandleAppleHit();
            }
        }

        #region "Event" Handlers

        /// <summary>
        /// Update the layer for the specified delta time.
        /// </summary>
        /// <param name="dt">Delta Time.</param>
        public override void Update(float dt)
        {
            if (_isPaused)
            {
                return;
            }

            base.Update(dt);

            _snake.Update(dt);

            CheckCollisions();

            _elapsedTime += dt;
            int min = (int)(_elapsedTime / 60f);
            int sec = (int)(_elapsedTime % 60f);
            _timeLabel.Text = String.Format(TIMER_STRING, min, sec);
            _appleLabel.Text = String.Format(APPLE_STRING, _eatenApples, _maxApples);
        }

        /// <remarks>>
        /// One touch - change snake direction
        /// More than one touch - pause the game
        /// </remarks>
        public override void TouchesEnded(List<CCTouch> touches)
        {
            base.TouchesEnded(touches);

            if (_isPaused)
            {
                return;
            }

            if (touches.Count > 1)
            {
                HandlePause();
            }
            else 
            {
                // figure out which quadrant of the screen the touch was in 
                // and schedule the snake to go in that direction

                var center = CCDirector.SharedDirector.WinSize.Center;
                var touchLoc = touches[0].Location;

                if (CollisionUtils.IsPointInTriangle(touchLoc, center, _topLeftCorner, _topRightCorner))
                {
                    _snake.TurnNorth();
                }
                else if (CollisionUtils.IsPointInTriangle(touchLoc, center, _topRightCorner, _bottomRightCorner))
                {
                    _snake.TurnEast();
                }
                else if (CollisionUtils.IsPointInTriangle(touchLoc, center, _bottomRightCorner, _bottomLeftCorner))
                {
                    _snake.TurnSouth();
                }
                else if (CollisionUtils.IsPointInTriangle(touchLoc, center, _bottomLeftCorner, _topLeftCorner))
                {
                    _snake.TurnWest();
                }
            }
        }

        /// <summary>
        /// Handles when the snake head touches an apple.
        /// </summary>
        private void HandleAppleHit()
        {
            CCSimpleAudioEngine.SharedEngine.PlayEffect("Sounds/nom-nom.mp3");
            _apple.RemoveFromParent();
            _eatenApples++;

            if (_eatenApples == _maxApples) // player has reached the killscreen!
            {
                HandleGameOver();
            }
            else 
            {
                _snake.Grow();
                _apple = GetAnApple();
                AddChild(_apple);
            }
        }

        /// <summary>
        /// Handles when the game is paused.
        /// </summary>
        private void HandlePause()
        {
            TouchEnabled = false;
            _isPaused = true;
            UnscheduleUpdate();
            PauseSchedulerAndActions();
            CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic();
            _pauseLayer = new PauseLayer(this);
            _pauseLayer.Position = new CCPoint(0f, 0f);
            AddChild(_pauseLayer);
        }

        /// <summary>
        /// Loads the game over layer when the player has run into his/herself or the borders.
        /// </summary>
        private void HandleGameOver()
        {
            TouchEnabled = false;
            _isPaused = true;
            UnscheduleUpdate();
            PauseSchedulerAndActions();
            CCSimpleAudioEngine.SharedEngine.StopBackgroundMusic();

            // set the score and max score and let the game over scene figure out what to do with them
            Scores.MaxApples = _maxApples;
            Scores.EatenApples = _eatenApples;
            Scores.Time = _elapsedTime;

            CCDirector.SharedDirector.PushScene(GameOverLayer.Scene);
        }

        #endregion
    }
}

