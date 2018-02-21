using System;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс, описывающий астероид и реализующий его отрисовку и перемещение.
    /// Дочерний класс класса <see cref="ImageObject"/>
    /// </summary>
    class Meteor : ImageObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее значение угла, на которое осуществляется поворот астероида.
        /// </summary>
        protected float Angle;

        /// <summary>
        /// Поле, хранящее значение угла отрисовки астероида.
        /// </summary>
        protected float DAngle;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Meteor.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="dAngle">Угол, на который астероид поворачивается за один цикл Update</param>
        public Meteor(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, float dAngle) : base(pos, dir, size, image, graphic, screenSize)
        {
            DAngle = dAngle;
            Angle = 0; //Начальное значение угла отрисовки изображения
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Метод отрисовки астероида
        /// </summary>
        public override void Draw()
        {
            //Рассчет центра вращения
            int x = Pos.X + Size.Width / 2; 
            int y = Pos.Y + Size.Height / 2;
            double _angle = Angle * Math.PI / 180; //перевод градусов в радианы

            //Эффект вращения реализуется путем отрисовки изображения в параллелограмме, наклоненного в нужную сторону на определенный градус
            //ниже реализовар рассчет координат вершин параллелограмма 
            double x1 = x + ((-Size.Width / 2) * Math.Cos(_angle) - (-Size.Height / 2) * Math.Sin(_angle)); //Координаты верхней правой вершины
            double y1 = y + ((-Size.Width / 2) * Math.Sin(_angle) + (-Size.Height / 2) * Math.Cos(_angle));

            double x2 = x + ((Size.Width / 2) * Math.Cos(_angle) - (-Size.Height / 2) * Math.Sin(_angle)); //Координаты верхнего левой вершины
            double y2 = y + ((Size.Width / 2) * Math.Sin(_angle) + (-Size.Height / 2) * Math.Cos(_angle));

            double x3 = x + ((Size.Width / 2) * Math.Cos(_angle) - (Size.Height / 2) * Math.Sin(_angle)); //Координаты нижней правой вершины
            double y3 = y + ((Size.Width / 2) * Math.Sin(_angle) + (Size.Height / 2) * Math.Cos(_angle));

            Point[] destPoints = { new Point((int)x2, (int)y2), new Point((int)x1, (int)y1), new Point((int)x3, (int)y3) };
            Graphic.DrawImage(Image, destPoints);

            // TODO: разобраться почему способ, приведенный ниже дает глюк
            /*
            Game.Buffer.Graphics.TranslateTransform(Pos.X+Size.Width/2, Pos.Y+Size.Height/2);
            Game.Buffer.Graphics.RotateTransform(Angle);
            Game.Buffer.Graphics.DrawImage(ObjectImage, new Rectangle(0, 0, Size.Width, Size.Height));
            Game.Buffer.Graphics.RotateTransform(-Angle);
            Game.Buffer.Graphics.TranslateTransform(-(Pos.X + Size.Width / 2), -(Pos.Y + Size.Height / 2));
            */
        }

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>        public override void Update()
        {
            //Движение объекта вдоль координат X и Y
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            
            //Если объект доходит до края экрана, то он начинает движение с противоположной стороны экрана 
            if (Pos.X < -Size.Width)
                Pos.X = ScreenSize.Width + Size.Width;
            if (Pos.X > ScreenSize.Width + Size.Width)
                Pos.X = -Size.Width;
            if (Pos.Y < -Size.Height)
                Pos.Y = ScreenSize.Height + Size.Height;
            if (Pos.Y > ScreenSize.Height + Size.Height )
                Pos.Y = -Size.Height;
            
            //Поаорот объекта на заданный угол
            Angle += DAngle;
            if (Angle >= 360) //Если угол больше или равен 360 градусов, то он выставляется в 0
                Angle = 0;
        }
        #endregion    }
}
