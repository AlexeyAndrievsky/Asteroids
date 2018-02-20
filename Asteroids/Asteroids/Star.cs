using System;
using System.Drawing;

namespace Asteroids
{
    class Star : ImageObject
    {
        public Star(Point pos, Point dir, Size size, Image image) : base(pos, dir, size, image)
        {
        }        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(ObjectImage, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Width));
        }        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }    }
}
