using System;
using System.Drawing;

namespace Asteroids
{
    class ThroughtSpace : Star
    {
        private Random rand;

        public ThroughtSpace(Point pos, Point dir, Size size, Image image, Random rand, Graphics graphic, Size screenSize) : base(pos, dir, size, image, graphic, screenSize)
        {
            this.rand = rand;
        }        public override void Draw()
        {
            base.Draw();
        }        public override void Update()
        {
            if (Pos.X < ScreenSize.Width / 2 && Pos.Y < ScreenSize.Height / 2)
            {
                Pos.X = Pos.X - GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y - GetKoeff().Y * Dir.Y;
            }
            else if (Pos.X > ScreenSize.Width / 2 && Pos.Y < ScreenSize.Height / 2)
            {
                Pos.X = Pos.X + GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y - GetKoeff().Y * Dir.Y;
            }
            else if (Pos.X < ScreenSize.Width / 2 && Pos.Y > ScreenSize.Height / 2)
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

            if (Size.Width > 10 || Pos.X > ScreenSize.Width || Pos.X < 0 || Pos.Y > ScreenSize.Height || Pos.Y < 0)
            {
                Size.Width = 1;
                Pos.X = rand.Next(0, ScreenSize.Width);
                Pos.Y = rand.Next(0, ScreenSize.Height);
            }

        }

        private Point GetKoeff()
        {
            return new Point(Math.Abs(ScreenSize.Width / 2 - Pos.X) / 50, Math.Abs(ScreenSize.Height / 2 - Pos.Y) / 50);
        }
    }
}
