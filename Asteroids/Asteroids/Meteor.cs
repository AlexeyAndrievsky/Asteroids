using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Asteroids
{
    class Meteor : ImageObject
    {
        protected float Angle;
        protected float DAngle;

        public Meteor(Point pos, Point dir, Size size, Image image, float dAngle) : base(pos, dir, size, image)
        {
            DAngle = dAngle;
            Angle = 0;
        }

        public override void Draw()
        {
            int x = Pos.X + Size.Width / 2;
            int y = Pos.Y + Size.Height / 2;
            double _angle = Angle * Math.PI / 180;

            double x1 = x + ((-Size.Width / 2) * Math.Cos(_angle) - (-Size.Height / 2) * Math.Sin(_angle));
            double y1 = y + ((-Size.Width / 2) * Math.Sin(_angle) + (-Size.Height / 2) * Math.Cos(_angle));

            double x2 = x + ((Size.Width / 2) * Math.Cos(_angle) - (-Size.Height / 2) * Math.Sin(_angle));
            double y2 = y + ((Size.Width / 2) * Math.Sin(_angle) + (-Size.Height / 2) * Math.Cos(_angle));

            double x3 = x + ((Size.Width / 2) * Math.Cos(_angle) - (Size.Height / 2) * Math.Sin(_angle));
            double y3 = y + ((Size.Width / 2) * Math.Sin(_angle) + (Size.Height / 2) * Math.Cos(_angle));

            Point[] destPoints = { new Point((int)x2, (int)y2), new Point((int)x1, (int)y1), new Point((int)x3, (int)y3) };
            Game.Buffer.Graphics.DrawImage(Image, destPoints);

            /* разобраться почему этот способ дает глюк
            Game.Buffer.Graphics.TranslateTransform(Pos.X+Size.Width/2, Pos.Y+Size.Height/2);
            Game.Buffer.Graphics.RotateTransform(Angle);
            Game.Buffer.Graphics.DrawImage(ObjectImage, new Rectangle(0, 0, Size.Width, Size.Height));
            Game.Buffer.Graphics.RotateTransform(-Angle);
            Game.Buffer.Graphics.TranslateTransform(-(Pos.X + Size.Width / 2), -(Pos.Y + Size.Height / 2));
            */
        }        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < -Size.Width) Pos.X = Game.Width + Size.Width;
            Angle += DAngle;
            if (Angle >= 360)
                Angle = 0;
        }    }
}
