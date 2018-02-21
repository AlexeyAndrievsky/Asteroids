using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс, описывающий базовый игровой объект и реализующий его отрисовку и перемещение.
    /// </summary>
    class BaseObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее координаты объекта.
        /// </summary>
        protected Point Pos;

        /// <summary>
        /// Поле, хранящее информацию о велечине смещения объекта по осям X и Y. 
        /// </summary>
        protected Point Dir;

        /// <summary>
        /// Поле, хранящее размер объекта.
        /// </summary>
        protected Size Size;

        /// <summary>
        /// Поле хранящее размеры области отрисовки.
        /// </summary>
        protected Size ScreenSize;

        /// <summary>
        /// Поверхность рисования.
        /// </summary>
        protected Graphics Graphic;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса BaseObject.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        public BaseObject(Point pos, Point dir, Size size, Graphics graphic, Size screenSize)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            Graphic = graphic;
            ScreenSize = screenSize;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки объекта.
        /// </summary>
        public virtual void Draw()
        {
            Graphic.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>
        public virtual void Update()
        {
            //Движение вдоль осей X и Y
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;

            //Если объект достигает края экрана, то он начинает двигаться в противоположную сторону
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > ScreenSize.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > ScreenSize.Height) Dir.Y = -Dir.Y;
        }
        #endregion
    }
}
