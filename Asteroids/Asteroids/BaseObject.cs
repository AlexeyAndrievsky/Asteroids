using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Абстрактный класс, описывающий базовый игровой объект и реализующий его отрисовку и перемещение.
    /// </summary>
    abstract class BaseObject:ICollision
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее координаты объекта.
        /// </summary>
        protected Point Pos;
        public Point pos
        {
            get { return Pos; }
            protected set { Pos = value; } 
        }

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

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);
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
        /// <param name="isCollisionEnabled">Нужна ли обработка коллизии объекту</param>
        public BaseObject(Point pos, Point dir, Size size, Graphics graphic, Size screenSize)
        {
            Pos = pos;
            Dir = dir;
            if (Dir.X < -100 || Dir.X > 100)
                throw new GameObjectException("Скорость по X слишком большая");
            if (Dir.Y < -100 || Dir.Y > 100)
                throw new GameObjectException("Скорость по Y слишком большая");
            if (size.Width < 0)
                throw new GameObjectException("Ширина объекта не может быть меньше нуля");
            if (size.Height < 0)
                throw new GameObjectException("Высота объекта не может быть меньше нуля");
            Size = size;
            Graphic = graphic;
            ScreenSize = screenSize;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки объекта.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>
        public abstract void Update();
        #endregion
    }
}
