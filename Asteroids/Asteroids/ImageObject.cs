using System;
using System.Drawing;

namespace Asteroids
{
    class ImageObject : BaseObject
    {
        protected Image Image;

        public ImageObject(Point pos, Point dir, Size size, Image image) : base(pos, dir, size)
        {
            Image = image;
        }        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0)
                Pos.X = Game.Width;
        }

        public void Update(int offsetX)
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X <= offsetX)
                Pos.X = Game.Width;
        }
    }
}
