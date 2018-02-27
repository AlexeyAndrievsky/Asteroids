using System.Drawing;

namespace Asteroids
{
    class Player : ImageObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее количество жизней игрока.
        /// </summary>
        private int Health { get; }

        /// <summary>
        /// Поле, хранящее текущую максимальную скорость корабля игрока.
        /// </summary>
        protected int MaxVelocity { get; }

        /// <summary>
        /// Флаг движения
        /// </summary>
        protected bool MovingFlag;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса ImageObject.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="health">Количество жизней</param>
        /// <param name="maxVelocity">Максимальная скорость</param>
        public Player(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, int health, int maxVelocity) : base(pos, dir, size, image, graphic, screenSize)
        {
            Health = health;
            MaxVelocity = maxVelocity;
            MovingFlag = false;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки объекта.
        /// </summary>
        public override void Draw()
        {
            base.Draw();
        }

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>
        public override void Update()
        {
            //Если объект доходит до края экрана, он останавливается 
            if (Pos.X > 0 && Pos.X < ScreenSize.Width - Image.Width && Pos.Y > 0 && Pos.Y < ScreenSize.Height - Image.Height)
            {
                Pos.X = Pos.X + Dir.X;
                Pos.Y = Pos.Y + Dir.Y;
            }

            if (Pos.X <= 0)
            {
                Pos.X = 1;
                Dir.X = 0;
            }

            if (Pos.Y <=0)
            {
                Pos.Y = 1;
                Dir.Y = 0;
            }

            if (Pos.X >= ScreenSize.Width - Image.Width)
            {
                Pos.Y = ScreenSize.Width - Image.Width - 1;
                Dir.Y = 0;
            }

            if (Pos.Y >= ScreenSize.Height - Image.Height)
            {
                Pos.Y = ScreenSize.Height - Image.Height - 1;
                Dir.Y = 0;
            }

            if (!MovingFlag) //инерция
            {
                if (Dir.Y != 0)
                    if (Dir.Y < 0)
                        Dir.Y++;
                    else
                        Dir.Y--;
            }
        }

        /// <summary>
        /// Ускорение при движении вниз
        /// </summary>
        public void MoveDown()
        {
            MovingFlag = true;
            if (Dir.Y < MaxVelocity)
                Dir.Y++;
        }

        /// <summary>
        /// Ускорение при движении вверх
        /// </summary>
        public void MoveUp()
        {
            MovingFlag = true;
            if (Dir.Y > -MaxVelocity)
                Dir.Y--;
        }

        /// <summary>
        /// Остановка движения.
        /// </summary>
        public void StopMoving()
        {
            MovingFlag = false;
        }

        #endregion

    }
}
