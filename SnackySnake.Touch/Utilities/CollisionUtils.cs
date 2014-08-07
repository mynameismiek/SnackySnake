using System;
using Cocos2D;

namespace SnackySnake.Touch.Utilities
{
    /// <summary>
    /// Utilities for helping with collisions.
    /// </summary>
    public static class CollisionUtils
    {
        /// <summary>
        /// Gets the corrected bounding box for the sprite with an 
        /// anchor point at the bottom left corner (0f,0f)
        /// </summary>
        /// <returns>The corrected bounding box.</returns>
        /// <param name="sprite">Sprite.</param>
        public static CCRect GetCorrectedBoundingBoxBL(CCSprite sprite)
        {
            var spriteSize = sprite.ContentSize;
            return new CCRect(sprite.Position.X + 1f,  // if they touch on the borders of the rects it counts as a collision,
                              sprite.Position.Y + 1f,  // so shift 1 up here
                              spriteSize.Width - 2f,   // and strink by 1 in each direction (N,S,E,W)
                              spriteSize.Height - 2f); // down here so its 30x30 and centered
        }

        /// <summary>
        /// Gets the corrected bounding box for the sprite with an
        /// anchor point at the center (0.5f, 0.5f)
        /// </summary>
        /// <returns>The corrected bounding box.</returns>
        /// <param name="sprite">Sprite.</param>
        public static CCRect GetCorrectedBoundingBoxCenter(CCSprite sprite)
        {
            var spriteSize = sprite.ContentSize;
            return new CCRect(sprite.Position.X - (spriteSize.Width / 2f) + 1f,
                              sprite.Position.Y - (spriteSize.Height / 2f) + 1f, 
                              spriteSize.Width - 2f,   
                              spriteSize.Height - 2f); 
        }

        /// <summary>
        /// Determines if the point is inside the triangle specified by the vertexes v1, v2, and v3.
        /// 
        /// Please wind clockwise.
        /// 
        /// Thanks, scgames @ http://www.gamedev.net/topic/295943-is-this-a-better-point-in-triangle-test-2d/
        /// </summary>
        /// <returns><c>true</c> if the point is in the triangle; otherwise, <c>false</c>.</returns>
        /// <param name="point">Point.</param>
        /// <param name="v1">Vertex 1.</param>
        /// <param name="v2">Vertex 2.</param>
        /// <param name="v3">Vertex 3.</param>
        public static bool IsPointInTriangle(CCPoint point, CCPoint v1, CCPoint v2, CCPoint v3)
        {
            bool b1, b2, b3;

            b1 = Sign(point, v1, v2) < 0.0f;
            b2 = Sign(point, v2, v3) < 0.0f;
            b3 = Sign(point, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        /// <summary>
        /// Figures out which side of the line created by p2 and p3 the point p1 is.
        /// 
        /// If it is greater than 0 its on the left.
        /// If it is less than 0 its on the right.
        /// 
        /// Again thanks, scgames @ http://www.gamedev.net/topic/295943-is-this-a-better-point-in-triangle-test-2d/
        /// </summary>
        /// <param name="p1">Point 1.</param>
        /// <param name="p2">Point 2.</param>
        /// <param name="p3">Point 3.</param>
        private static float Sign(CCPoint p1, CCPoint p2, CCPoint p3)
        {
            return ((p1.X - p3.X) * (p2.Y - p3.Y)) - ((p2.X - p3.X) * (p1.Y - p3.Y));
        }
    }
}

