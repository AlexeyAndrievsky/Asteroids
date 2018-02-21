using System;
using System.Drawing;

namespace Asteroids
{
    class ThroughtSpace : Star
    {
        private Random rand;

        public ThroughtSpace(Point pos, Point dir, Size size, Image image, Random rand) : base(pos, dir, size, image)
        {
            this.rand = rand;
        }        public override void Draw()
        {
            SplashScreen.Buffer.Graphics.DrawImage(Image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Width));
        }        public override void Update()
        {
            if (Pos.X < SplashScreen.Width / 2 && Pos.Y < SplashScreen.Height / 2)
            {
                Pos.X = Pos.X - GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y - GetKoeff().Y * Dir.Y;
            }
            else if (Pos.X > SplashScreen.Width / 2 && Pos.Y < SplashScreen.Height / 2)
            {
                Pos.X = Pos.X + GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y - GetKoeff().Y * Dir.Y;
            }
            else if (Pos.X < SplashScreen.Width / 2 && Pos.Y > SplashScreen.Height / 2)
            {
                Pos.X = Pos.X - GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y + GetKoeff().Y * Dir.Y;
            }
            else
            {
                Pos.X = Pos.X + GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y + GetKoeff().Y * Dir.Y;
            }
            Size.Width += 1;

            if (Size.Width > 10 || Pos.X > SplashScreen.Width || Pos.X < 0 || Pos.Y > SplashScreen.Height || Pos.Y < 0)
            {
                Size.Width = 1;
                Pos.X = rand.Next(0, SplashScreen.Width);
                Pos.Y = rand.Next(0, SplashScreen.Height);
            }

        }

        private Point GetKoeff()
        {
            return new Point(Math.Abs(SplashScreen.Width / 2 - Pos.X) / 50, Math.Abs(SplashScreen.Height / 2 - Pos.Y) / 50);
        }
    }
}
