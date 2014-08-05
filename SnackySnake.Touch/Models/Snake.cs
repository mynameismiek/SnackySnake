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

        private CCSprite HEAD;
        private CCSprite TAIL;
        private CCSprite BODY;
        private CCSprite BODY_TURN;

        private Direction currentDirection;
        private List<CCPoint> bodySegments;

        public Snake()
        {
            LoadContent();
            bodySegments = new List<CCPoint>();
        }

        /// <summary>
        /// Loads the content for the snake.
        /// </summary>
        private void LoadContent()
        {
            HEAD = new CCSprite();
            HEAD.InitWithFile("Images/Head-hd.png");

            TAIL = new CCSprite();
            TAIL.InitWithFile("Images/Tail-hd.png");

            BODY = new CCSprite();
            BODY.InitWithFile("Images/Body-hd.png");

            BODY_TURN = new CCSprite();
            BODY_TURN.InitWithFile("Images/BodyTurn-hd.png");
        }

        public void Reset()
        {
            bodySegments.Clear();
        }

        public void Grow()
        {
            // add a body segment where the current head is
            //bodySegments.Insert(1, )
        }
    }
}

