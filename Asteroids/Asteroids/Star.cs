using System.Drawing;

namespace Asteroids
{
    class Star : ImageObject
    {
        public Star(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize) : base(pos, dir, size, image, graphic, screenSize)
        {
        }        public override void Draw()
        {
            Graphic.DrawImage(Image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Width));
        }        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0) Pos.X = ScreenSize.Width + Size.Width;
        }    }
}
