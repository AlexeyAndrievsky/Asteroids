using System;
using System.Drawing;

namespace Asteroids
{
    class ImageObject : BaseObject
    {
        protected Image Image;

        public ImageObject(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize) : base(pos, dir, size, graphic, screenSize)
        {
            Image = image;
        }        public override void Draw()
        {
            Graphic.DrawImage(Image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0)
                Pos.X = ScreenSize.Width;
        }

        public void Update(int offsetX)
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X <= offsetX)
                Pos.X = ScreenSize.Width;
        }
    }
}
