using System;
using System.Drawing;

namespace Asteroids
{
    class Background: ImageObject
    {
        public Background(Point pos, Point dir, Size size, Image image) : base(pos, dir, size, image)
        {
        }
        public override void Draw()
        {
            
        }        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}
