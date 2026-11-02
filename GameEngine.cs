using System.Drawing;

namespace FlappyBird
{
    public enum GameState
    {
        Ready,
        Playing,
        GameOver
    }

    /// <summary>
    /// Oyun mantigini yonetir: kus, borular, skor, durum ve carpisma kontrolleri.
    /// Cizim islemi burada yapilmaz.
    /// </summary>
    public class GameEngine
    {
        private const float PipeSpeed = 3f;
        private const int PipeSpacing = 220;
        private const int PipeGapHeight = 150;
        private const int PipeMargin = 60;
        private const int GroundHeight = 80;

        private readonly Random _random = new Random();
        private readonly List<Pipe> _pipes = new List<Pipe>();

        public GameEngine(int width, int height)
        {
            Width = width;
            Height = height;
            Bird = new Bird(width / 4f, height / 2f);
            Restart();
        }

        public int Width { get; }

        public int Height { get; }

        /// <summary>Zeminin ust kenarinin Y koordinati.</summary>
        public int GroundY => Height - GroundHeight;

        public Bird Bird { get; }

        public IReadOnlyList<Pipe> Pipes => _pipes;

        public int Score { get; private set; }

        public int BestScore { get; private set; }

        public GameState State { get; private set; }

        /// <summary>Oyunu baslangic durumuna dondurur.</summary>
        public void Restart()
        {
            Bird.Reset();
            _pipes.Clear();
            Score = 0;
            State = GameState.Ready;
        }

        /// <summary>
        /// Zipla komutu. Hazir durumunda oyunu baslatir, oynanirken kusu ziplatir.
        /// Oyun bittiyse etkisizdir.
        /// </summary>
        public void Jump()
        {
            if (State == GameState.Ready)
            {
                State = GameState.Playing;
            }

            if (State == GameState.Playing)
            {
                Bird.Jump();
            }
        }

        /// <summary>Her zamanlayici karesinde bir kez cagrilir.</summary>
        public void Update()
        {
            if (State != GameState.Playing)
            {
                return;
            }

            Bird.Update();

            foreach (Pipe pipe in _pipes)
            {
                pipe.Update(PipeSpeed);
            }

            _pipes.RemoveAll(pipe => pipe.IsOffScreen);

            SpawnPipeIfNeeded();
            UpdateScore();
            CheckCollisions();
        }

        private void SpawnPipeIfNeeded()
        {
            bool needsPipe = _pipes.Count == 0
                || _pipes[_pipes.Count - 1].X < Width - PipeSpacing;

            if (!needsPipe)
            {
                return;
            }

            int minTop = PipeMargin;
            int maxTop = GroundY - PipeGapHeight - PipeMargin;
            int gapTop = _random.Next(minTop, maxTop + 1);

            _pipes.Add(new Pipe(Width, gapTop, PipeGapHeight));
        }

        private void UpdateScore()
        {
            foreach (Pipe pipe in _pipes)
            {
                if (pipe.TryMarkPassed(Bird.X))
                {
                    Score++;
                }
            }
        }

        private void CheckCollisions()
        {
            Rectangle birdBounds = Bird.Bounds;

            if (birdBounds.Top < 0 || birdBounds.Bottom >= GroundY)
            {
                EndGame();
                return;
            }

            foreach (Pipe pipe in _pipes)
            {
                if (pipe.CollidesWith(birdBounds, GroundY))
                {
                    EndGame();
                    return;
                }
            }
        }

        private void EndGame()
        {
            State = GameState.GameOver;
            BestScore = Math.Max(BestScore, Score);
        }
    }
}
