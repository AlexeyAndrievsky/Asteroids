using System;
using System.Drawing;

namespace Asteroids
{
    class MovingText : BaseObject
    {
        protected float Angle;
        protected Brush Brsh;
        protected string Text;
        protected Font Fnt;

        public MovingText(Point pos, Point dir, Size size, Graphics graphic, Size screenSize, float angle, Brush brush, Font font, string text):base(pos, dir, size, graphic, screenSize)
        {
            Brsh = brush;
            Text = text;
            Angle = angle;
            Fnt = font;
        }

        public override void Draw()
        {
            Graphic.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
            Graphic.TranslateTransform(Pos.X, Pos.Y);
            Graphic.RotateTransform(Angle);
            Graphic.DrawString(Text, Fnt, Brsh, 0, 0);
            Graphic.RotateTransform(-Angle);
            Graphic.TranslateTransform(-Pos.X, -Pos.Y);
        }
        public override void Update()
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
