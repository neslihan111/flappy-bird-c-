using System.Drawing;

namespace FlappyBird
{
    /// <summary>
    /// Ekranda sagdan sola kayan bir boru cifti (ust ve alt boru).
    /// Aradaki bosluk GapTop ile baslar ve GapHeight kadar surer.
    /// </summary>
    public class Pipe
    {
        public const int Width = 70;

        public Pipe(float x, int gapTop, int gapHeight)
        {
            X = x;
            GapTop = gapTop;
            GapHeight = gapHeight;
        }

        public float X { get; private set; }

        public int GapTop { get; }

        public int GapHeight { get; }

        public int GapBottom => GapTop + GapHeight;

        /// <summary>Kusun bu boruyu gecip gecmedigini belirtir.</summary>
        public bool Passed { get; private set; }

        public bool IsOffScreen => X + Width < 0;

        public void Update(float speed)
        {
            X -= speed;
        }

        public Rectangle GetTopRectangle()
        {
            return new Rectangle((int)Math.Round(X), 0, Width, GapTop);
        }

        public Rectangle GetBottomRectangle(int groundY)
        {
            return new Rectangle((int)Math.Round(X), GapBottom, Width, groundY - GapBottom);
        }

        public bool CollidesWith(Rectangle birdBounds, int groundY)
        {
            return birdBounds.IntersectsWith(GetTopRectangle())
                || birdBounds.IntersectsWith(GetBottomRectangle(groundY));
        }

        /// <summary>
        /// Kus boruyu ilk kez gectiginde true dondurur ve boruyu gecilmis olarak isaretler.
        /// </summary>
        public bool TryMarkPassed(float birdX)
        {
            if (Passed || X + Width >= birdX)
            {
                return false;
            }

            Passed = true;
            return true;
        }
    }
}
