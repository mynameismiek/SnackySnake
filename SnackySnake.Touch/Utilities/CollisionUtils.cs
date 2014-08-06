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
        /// Gets the corrected bounding box for the sprite.
        /// </summary>
        /// <returns>The corrected bounding box.</returns>
        /// <param name="sprite">Sprite.</param>
        public static CCRect GetCorrectedBoundingBox(CCSprite sprite)
        {
            var spriteSize = sprite.ContentSize;
            return new CCRect(sprite.Position.X + 1,  // if they touch on the borders of the rects it counts as a collision,
                              sprite.Position.Y + 1,  // so shift 1 up here
                              spriteSize.Width - 2,   // and strink by 1 in each direction (N,S,E,W)
                              spriteSize.Height - 2); // down here so its 30x30 and centered
        }
    }
}

