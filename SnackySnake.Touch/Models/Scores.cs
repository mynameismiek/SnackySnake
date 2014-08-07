using System;

namespace SnackySnake.Touch.Models
{
    /// <summary>
    /// Player scores.
    /// </summary>
    public static class Scores
    {
        /// <summary>
        /// Gets or sets the maximum number of apples.
        /// </summary>
        /// <value>The max number of apples.</value>
        public static int MaxApples { get; set; }

        /// <summary>
        /// Gets or sets the number of apples eaten by the player.
        /// </summary>
        /// <value>The eaten apples.</value>
        public static int EatenApples { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        /// <value>The elapsed time.</value>
        public static float Time { get; set; }

        /// <summary>
        /// Initializes this instance of the <see cref="SnackySnake.Touch.Models.Scores"/> class.
        /// </summary>
        static Scores()
        {
            Reset();
        }

        /// <summary>
        /// Reset the scores;
        /// </summary>
        public static void Reset()
        {
            MaxApples = 0;
            EatenApples = 0;
            Time = 0f;
        }
    }
}

