using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FlappyBird
{
    /// <summary>
    /// Ana pencere. Zamanlayici ile oyunu ilerletir, GDI+ ile cizer
    /// ve klavye/fare girdisini oyun motoruna iletir.
    /// </summary>
    public class MainForm : Form
    {
        private const int ClientWidth = 400;
        private const int ClientHeight = 600;
        private const int TimerInterval = 16;
        private const int GrassHeight = 20;

        private static readonly Color SkyTop = Color.FromArgb(78, 192, 230);
        private static readonly Color SkyBottom = Color.FromArgb(200, 240, 250);
        private static readonly Color PipeColor = Color.FromArgb(94, 178, 52);
        private static readonly Color PipeBorder = Color.FromArgb(40, 90, 30);
        private static readonly Color GrassColor = Color.FromArgb(111, 190, 70);
        private static readonly Color DirtColor = Color.FromArgb(222, 184, 135);

        private readonly GameEngine _engine;
        private readonly System.Windows.Forms.Timer _timer;

        public MainForm()
        {
            Text = "Flappy Bird";
            ClientSize = new Size(ClientWidth, ClientHeight);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            KeyPreview = true;

            _engine = new GameEngine(ClientWidth, ClientHeight);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = TimerInterval;
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _engine.Update();
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode != Keys.Space)
            {
                return;
            }

            if (_engine.State == GameState.GameOver)
            {
                _engine.Restart();
            }
            else
            {
                _engine.Jump();
            }

            e.SuppressKeyPress = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                _engine.Jump();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawSky(g);
            DrawPipes(g);
            DrawGround(g);
            DrawBird(g);
            DrawScore(g);
            DrawMessages(g);
        }

        private void DrawSky(Graphics g)
        {
            using var brush = new LinearGradientBrush(
                ClientRectangle, SkyTop, SkyBottom, LinearGradientMode.Vertical);
            g.FillRectangle(brush, ClientRectangle);
        }

        private void DrawPipes(Graphics g)
        {
            using var fill = new SolidBrush(PipeColor);
            using var border = new Pen(PipeBorder, 3f);

            foreach (Pipe pipe in _engine.Pipes)
            {
                Rectangle top = pipe.GetTopRectangle();
                Rectangle bottom = pipe.GetBottomRectangle(_engine.GroundY);

                g.FillRectangle(fill, top);
                g.DrawRectangle(border, top);
                g.FillRectangle(fill, bottom);
                g.DrawRectangle(border, bottom);
            }
        }

        private void DrawGround(Graphics g)
        {
            int groundY = _engine.GroundY;

            using var grass = new SolidBrush(GrassColor);
            using var dirt = new SolidBrush(DirtColor);

            g.FillRectangle(grass, 0, groundY, ClientWidth, GrassHeight);
            g.FillRectangle(dirt, 0, groundY + GrassHeight, ClientWidth,
                ClientHeight - groundY - GrassHeight);
        }

        private void DrawBird(Graphics g)
        {
            Bird bird = _engine.Bird;

            // Kusun dusus hizina gore egimi (yukari giderken yukari, duserken asagi bakar)
            float angle = Math.Clamp(bird.Velocity * 3f, -25f, 70f);

            GraphicsState saved = g.Save();
            g.TranslateTransform(bird.X, bird.Y);
            g.RotateTransform(angle);

            int halfW = Bird.Width / 2;
            int halfH = Bird.Height / 2;

            // Govde
            var body = new Rectangle(-halfW, -halfH, Bird.Width, Bird.Height);
            using (var bodyBrush = new SolidBrush(Color.Gold))
            {
                g.FillEllipse(bodyBrush, body);
            }
            g.DrawEllipse(Pens.Black, body);

            // Kanat
            var wing = new Rectangle(-halfW + 4, -2, 14, 9);
            using (var wingBrush = new SolidBrush(Color.Orange))
            {
                g.FillEllipse(wingBrush, wing);
            }
            g.DrawEllipse(Pens.Black, wing);

            // Goz
            var eye = new Rectangle(4, -9, 9, 9);
            g.FillEllipse(Brushes.White, eye);
            g.DrawEllipse(Pens.Black, eye);
            g.FillEllipse(Brushes.Black, new Rectangle(8, -6, 4, 4));

            // Gaga
            Point[] beak =
            {
                new Point(halfW - 2, -3),
                new Point(halfW + 8, 1),
                new Point(halfW - 2, 5)
            };
            g.FillPolygon(Brushes.OrangeRed, beak);
            g.DrawPolygon(Pens.Black, beak);

            g.Restore(saved);
        }

        private void DrawScore(Graphics g)
        {
            using var font = new Font("Arial", 32f, FontStyle.Bold);
            DrawCentered(g, _engine.Score.ToString(), font, 40f);
        }

        private void DrawMessages(Graphics g)
        {
            using var titleFont = new Font("Arial", 16f, FontStyle.Bold);
            using var textFont = new Font("Arial", 11f, FontStyle.Regular);

            switch (_engine.State)
            {
                case GameState.Ready:
                    DrawCentered(g, "Başlamak için SPACE", titleFont, 220f);
                    DrawCentered(g, "Zıplamak için SPACE veya sol tık", textFont, 255f);
                    break;

                case GameState.GameOver:
                    DrawCentered(g, "Game Over - Tekrar için SPACE", titleFont, 220f);
                    DrawCentered(g, $"Skor: {_engine.Score}   En iyi: {_engine.BestScore}",
                        textFont, 255f);
                    break;
            }
        }

        /// <summary>Metni yatayda ortalar; okunurluk icin golge ile cizer.</summary>
        private void DrawCentered(Graphics g, string text, Font font, float y)
        {
            SizeF size = g.MeasureString(text, font);
            float x = (ClientWidth - size.Width) / 2f;

            g.DrawString(text, font, Brushes.Black, x + 2f, y + 2f);
            g.DrawString(text, font, Brushes.White, x, y);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
