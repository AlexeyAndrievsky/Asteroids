using System;
using System.Drawing;


namespace Asteroids
{
    class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected Size ScreenSize;
        protected Graphics Graphic;

        public BaseObject(Point pos, Point dir, Size size, Graphics graphic, Size screenSize)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            Graphic = graphic;
            ScreenSize = screenSize;
        }
        public virtual void Draw()
        {
            Graphic.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public virtual void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > ScreenSize.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > ScreenSize.Height) Dir.Y = -Dir.Y;
        }
    }
}
