using System;
using Cocos2D;
using System.Collections.Generic;

namespace SnackySnake.Touch.Models
{
    /// <summary>
    /// The snake! This is what the player plays as. It moves around and eats things, mainly apples.
    /// </summary>
    public class Snake : CCDrawNode
    {
        enum Direction
        {
            North,
            South,
            East,
            West
        }

        private CCSprite _headSprite;
        private CCSprite _tailSprite;
        private CCSprite _bodySprite;
        private CCSprite _turnSprite;

        private Direction currentDirection;
        private Direction nextDirection;
        private List<CCPoint> bodySegments;

        public CCSprite Head
        {
            get 
            {
                // head pos + head size
                return _headSprite;
            }
        }

        public Snake()
        {
            LoadContent();
            bodySegments = new List<CCPoint>();
            Reset();
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

            _turnSprite = new CCSprite();
            _turnSprite.InitWithFile("Images/BodyTurn-hd.png");
        }

        /// <summary>
        /// Resets this snake to its default state.
        /// </summary>
        public void Reset()
        {
            currentDirection = Direction.West;
            nextDirection = Direction.West;
            bodySegments.Clear();
            bodySegments.Add(new CCPoint()); // head
            bodySegments.Add(new CCPoint()); // body
            bodySegments.Add(new CCPoint()); // tail
        }

        public void Grow()
        {
            // add a body segment where the current head is
            //bodySegments.Insert(1, )
        }


        public override void Update(float dt)
        {
            base.Update(dt);


        }
    }
}

