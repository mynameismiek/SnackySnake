using System;
using Cocos2D;
using System.Collections.Generic;
using SnackySnake.Touch.Utilities;

namespace SnackySnake.Touch.Models
{
    /// <summary>
    /// The snake! This is what the player plays as. It moves around and eats things, mainly apples.
    /// </summary>
    public class Snake : CCNode
    {
        enum Direction
        {
            North,
            South,
            East,
            West
        }

        private const float SPEED = 1 / 5f;
        private readonly CCSize _offsets;
        private readonly int _squareSize;

        private Direction currentDirection;
        private Direction nextDirection;
        private bool shouldGrow;
        private float _moveTick;

        private CCSprite _headSprite;
        private CCSprite _bodySprite;
        private CCSprite _curveSprite;
        private CCSprite _tailSprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnackySnake.Touch.Models.Snake"/> class.
        /// </summary>
        /// <param name="offsets">Offsets.</param>
        /// <param name="squareSize">Square size.</param>
        public Snake(CCSize offsets, int squareSize)
        {
            _offsets = offsets;
            _squareSize = squareSize;
            LoadContent();
        }

        /// <summary>
        /// Loads the content for the snake.
        /// </summary>
        private void LoadContent()
        {
            _headSprite = new CCSprite();
            _headSprite.InitWithFile("Images/Head-hd.png");

            _tailSprite = new CCSprite();
            _tailSprite.InitWithFile("Images/Tail-hd.png");

            _bodySprite = new CCSprite();
            _bodySprite.InitWithFile("Images/Body-hd.png");

            _curveSprite = new CCSprite();
            _curveSprite.InitWithFile("Images/BodyTurn-hd.png");
        }

        /// <summary>
        /// Resets this snake to its default state.
        /// </summary>
        public void Reset()
        {
            currentDirection = Direction.West;
            nextDirection = Direction.West;
            shouldGrow = false;
            _moveTick = 0f;

            if (Children != null)
            {
                Children.Clear();
            }

            var size = CCDirector.SharedDirector.VisibleSize;
            // TODO: I must be too tired because this works, but doesnt make any sense as to why
            var x = (((size.Width - (_offsets.Width * 2f)) * 2f) / 3f) - (_offsets.Width / 2f); 
            var y = size.Center.Y + _offsets.Height;

            // Add the head
            _headSprite.Position = new CCPoint(x, y);
            _headSprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(_headSprite);

            // add the body
            x += _squareSize;
            _bodySprite.Position = new CCPoint(x, y);
            _bodySprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(_bodySprite);

            // add the tail
            x += _squareSize;
            _tailSprite.Position = new CCPoint(x, y);
            _tailSprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(_tailSprite);

            FigureOutRotations();
        }

        /// <summary>
        /// Tell the snake to grow by one body segment.
        /// </summary>
        public void Grow()
        {
            shouldGrow = true;
        }

        /// <summary>
        /// Gets the (corrected) boudning box for the head.
        /// </summary>
        /// <returns>The head bounding box.</returns>
        public CCRect GetHeadBox()
        {
            return CollisionUtils.GetCorrectedBoundingBox((CCSprite)Children[0]);
        }

        /// <summary>
        /// Gets the (corrected) bounding boxes for the body and tail.
        /// </summary>
        /// <returns>The body and tail bounding boxes.</returns>
        public List<CCRect> GetBodyAndTailBoundingBoxes()
        {
            var boxes = new List<CCRect>();

            for (int i = 1; i < Children.Count; i++)
            {
                boxes.Add(CollisionUtils.GetCorrectedBoundingBox((CCSprite)Children[i]));
            }

            return boxes;
        }

        #region Turn Methods

        /// <summary>
        /// Tells the snake to turn north.
        /// </summary>
        public void TurnNorth()
        {
            if (currentDirection != Direction.South)
            {
                nextDirection = Direction.North;
            }
        }

        /// <summary>
        /// Tells the snake to turn south.
        /// </summary>
        public void TurnSouth()
        {
            if (currentDirection != Direction.North)
            {
                nextDirection = Direction.South;
            }
        }

        /// <summary>
        /// Tells the snake to turn east.
        /// </summary>
        public void TurnEast()
        {
            if (currentDirection != Direction.West)
            {
                nextDirection = Direction.East;
            }
        }

        /// <summary>
        /// Tells the snake to turn west.
        /// </summary>
        public void TurnWest()
        {
            if (currentDirection != Direction.East)
            {
                nextDirection = Direction.West;
            }
        }

        #endregion

        /// <remarks>>
        /// See if the snake needs to move and move it if it does.
        /// </remarks>
        public override void Update(float dt)
        {
            base.Update(dt);

            _moveTick += dt;

            if (_moveTick > SPEED)
            {
                _moveTick = 0f;
                currentDirection = nextDirection;
                Move();
                FigureOutRotations();
            }
        }

        /// <summary>
        /// Move the body segments of the snake. 
        /// </summary>
        private void Move()
        {
            var cachedPos = new CCPoint(Children[0].PositionX, Children[0].PositionY); // start off as the head position

            switch (currentDirection)
            {
                case Direction.North:
                    Children[0].Position = new CCPoint(cachedPos.X, cachedPos.Y + _squareSize);
                    break;
                case Direction.South:
                    Children[0].Position = new CCPoint(cachedPos.X, cachedPos.Y - _squareSize);
                    break;
                case Direction.East:
                    Children[0].Position = new CCPoint(cachedPos.X + _squareSize, cachedPos.Y);
                    break;
                case Direction.West:
                    Children[0].Position = new CCPoint(cachedPos.X - _squareSize, cachedPos.Y);
                    break;
                default:
                    throw new ArgumentException("Invalid direction!");
            }

            // check to see if the snake should grow
            if (shouldGrow)
            {
                // insert a segment behind the head for the new growth... ew...
                var newBodySeg = new CCSprite(_bodySprite.Texture, _bodySprite.TextureRect)
                {
                    Position = cachedPos
                };
                Children.Insert(1, newBodySeg);
                shouldGrow = false;

                return; // no need to move the rest of the snake
            }

            // move the rest of the snake
            for (int i = 1; i < Children.Count; i++)
            {
                var tempPos = Children[i].Position;
                Children[i].Position = cachedPos;
                cachedPos = tempPos;
            }
        }

        /// <summary>
        /// Figures the out rotations of the different body segments.
        /// </summary>
        private void FigureOutRotations()
        {
            FigureOutBodySegmentRotationsAndSprites();
            FigureOutTailRotation();
            FigureOutHeadRotation();
        }

        /// <summary>
        /// Figures out the head rotation.
        /// </summary>
        private void FigureOutHeadRotation()
        {
            var head = (CCSprite)Children.First();
            var headPos = head.Position;
            var firstBodyPos = Children[1].Position;
            int rot;

            // NOTE: the head image is pointing up and these need to change if I fix it to be pointing to 0 on the unit circle instead of pi/2
            if (headPos.Y < firstBodyPos.Y)
            {
                rot = 180;
            }
            else if (headPos.X > firstBodyPos.X)
            {
                rot = 90;
            }
            else if (headPos.X < firstBodyPos.X)
            {
                rot = 270;
            }
            else
            {
                rot = 0;
            }

            head.Rotation = rot;
        }

        /// <summary>
        /// Figures out the rotations of the body segments and 
        /// if we need to swap it to a corner sprite or back to
        /// a normal body sprite.
        /// </summary>
        private void FigureOutBodySegmentRotationsAndSprites()
        {
            CCSprite currentBody;
            CCPoint prevPos, currentPos, nextPos;
            CCTexture2D tex;
            int rot;

            for (int i = 1; i < Children.Count - 1; i++)
            {
                currentBody = (CCSprite)Children[i];
                prevPos = Children[i - 1].Position;
                nextPos = Children[i + 1].Position;

                // NOTE: the body image is running north/south and these need to change if I fix it to be east/west or 0 on the unit circle
                // Y's are the same and its running east/west rotate 90
                if (prevPos.Y == nextPos.Y) 
                {
                    tex = _bodySprite.Texture;
                    rot = 90;
                }
                // X's are the same and its running north/south so no rotation
                else if (prevPos.X == nextPos.X)
                {
                    tex = _bodySprite.Texture;
                    rot = 0;
                }
                // this segment needs to be a corner piece
                else 
                {
                    tex = _curveSprite.Texture;
                    currentPos = currentBody.Position;

                    // NOTE: the bodyturn images is such that the left and top edges are the joining edges, if I fix that I need to change this
                    if (((currentPos.X > prevPos.X) && (currentPos.Y > nextPos.Y)) ||
                        ((currentPos.X > nextPos.X) && (currentPos.Y > prevPos.Y)))
                    {
                        rot = 270;
                    }
                    else if (((currentPos.X < prevPos.X) && (currentPos.Y > nextPos.Y)) ||
                             ((currentPos.X < nextPos.X) && (currentPos.Y > prevPos.Y)))
                    {
                        rot = 180;
                    }
                    else if (((currentPos.X < nextPos.X) && (currentPos.Y < prevPos.Y)) ||
                             ((currentPos.X < prevPos.X) && (currentPos.Y < nextPos.Y)))
                    {
                        rot = 90;
                    }
                    else 
                    {
                        rot = 0;
                    }
                }

                if (currentBody.Texture != tex)
                {
                    currentBody.Texture = tex;
                }

                currentBody.Rotation = rot;
            }
        }

        /// <summary>
        /// Figures out the tail rotation.
        /// </summary>
        private void FigureOutTailRotation()
        {
            var tail = (CCSprite)Children.Last();
            var tailPos = tail.Position;
            var lastBodyPos = Children[Children.Count - 2].Position;
            int rot;

            // NOTE: the tail image is pointing down and these need to change if I fix it to be pointing to 0 on the unit circle instead of 3*pi/2
            if (tailPos.Y < lastBodyPos.Y)
            {
                rot = 0;
            }
            else if (tailPos.X > lastBodyPos.X)
            {
                rot = 270;
            }
            else if (tailPos.X < lastBodyPos.X)
            {
                rot = 90;
            }
            else
            {
                rot = 180;
            }

            tail.Rotation = rot;
        }
    }
}

