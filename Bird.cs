using System.Drawing;

namespace FlappyBird
{
    /// <summary>
    /// Oyuncunun kontrol ettigi kusu temsil eder.
    /// X ve Y kusun merkez koordinatlaridir.
    /// </summary>
    public class Bird
    {
        public const int Width = 34;
        public const int Height = 24;

        private const float Gravity = 0.45f;
        private const float JumpVelocity = -7.5f;
        private const float MaxFallSpeed = 12f;

        private readonly float _startY;

        public Bird(float x, float y)
        {
            X = x;
            _startY = y;
            Y = y;
        }

        public float X { get; }

        public float Y { get; private set; }

        public float Velocity { get; private set; }

        public Rectangle Bounds =>
            new Rectangle(
                (int)Math.Round(X - Width / 2f),
                (int)Math.Round(Y - Height / 2f),
                Width,
                Height);

        public void Jump()
        {
            Velocity = JumpVelocity;
        }

        public void Update()
        {
            Velocity += Gravity;
            if (Velocity > MaxFallSpeed)
            {
                Velocity = MaxFallSpeed;
            }

            Y += Velocity;
        }

        public void Reset()
        {
            Y = _startY;
            Velocity = 0f;
        }
    }
}
