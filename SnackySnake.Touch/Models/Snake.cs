using System;
using Cocos2D;
using System.Collections.Generic;

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

        public const float _speed = 1 / 60f;
        private readonly int _squareSize;

        private Direction currentDirection;
        private Direction nextDirection;
        private List<CCPoint> bodySegments;
        private bool shouldGrow;
        private float _moveTick;

        public CCSprite Head { get; private set; }
        public CCSprite Body { get; private set; }
        public CCSprite Curve { get; private set; }
        public CCSprite Tail { get; private set; }

        public Snake(int squareSize)
        {
            _squareSize = squareSize;
            LoadContent();
            bodySegments = new List<CCPoint>();
            Reset();
        }

        /// <summary>
        /// Loads the content for the snake.
        /// </summary>
        private void LoadContent()
        {
            Head = new CCSprite();
            Head.InitWithFile("Images/Head-hd.png");

            Tail = new CCSprite();
            Tail.InitWithFile("Images/Tail-hd.png");

            Body = new CCSprite();
            Body.InitWithFile("Images/Body-hd.png");

            Curve = new CCSprite();
            Curve.InitWithFile("Images/BodyTurn-hd.png");
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
            bodySegments.Clear();
            bodySegments.Add(new CCPoint(25, 9)); // head
            bodySegments.Add(new CCPoint(26, 9)); // body
            bodySegments.Add(new CCPoint(27, 9)); // tail
        }

        public void Grow()
        {
            shouldGrow = true;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            _moveTick += dt;

            if (_moveTick > _speed)
            {
                _moveTick = 0f;
                currentDirection = nextDirection;
                Move();
            }
        }

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

        /// <summary>
        /// Move the body segments of the snake. 
        /// </summary>
        private void Move()
        {
            var cachedPos = bodySegments[0]; // start off as the head position

            switch (currentDirection)
            {
                case Direction.North:
                    bodySegments[0] = new CCPoint(cachedPos.X, cachedPos.Y + 1);
                    break;
                case Direction.South:
                    bodySegments[0] = new CCPoint(cachedPos.X, cachedPos.Y - 1);
                    break;
                case Direction.East:
                    bodySegments[0] = new CCPoint(cachedPos.X + 1, cachedPos.Y);
                    break;
                case Direction.West:
                    bodySegments[0] = new CCPoint(cachedPos.X - 1, cachedPos.Y);
                    break;
                default:
                    throw new ArgumentException("Invalid direction!");
            }

            // check to see if the snake should grow
            if (shouldGrow)
            {
                // insert a segment behind the head for the new growth... ew...
                bodySegments.Insert(1, cachedPos);
                shouldGrow = false;

                return; // no need to move the rest of the snake
            }

            // move the rest of the snake
            for (int i = 1; i < bodySegments.Count; i++)
            {
                var tempPos = bodySegments[i];
                bodySegments[i] = cachedPos;
                cachedPos = tempPos;
            }
        }
    }
}

