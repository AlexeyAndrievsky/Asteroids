using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс, описывающий звезду и реализующий ее отрисовку и перемещение.
    /// Дочерний класс класса <see cref="ImageObject"/>
    /// </summary>
    class Star : ImageObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее значение, определяющее пульсацию звезды.
        /// </summary>
        private int? Pulse;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Star.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="pulse">Значение, определяющее пульсацию звезды</param>
        public Star(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, int? pulse) : base(pos, dir, size, image, graphic, screenSize)
        {
            if (pulse.HasValue)
                Pulse = pulse;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки звезды.
        /// </summary>
        public override void Draw()
        {
            Graphic.DrawImage(Image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Width));
        }

        /// <summary>
        /// Метод обновления параметров звезды.
        /// </summary>
        public override void Update()
        {
            //Движение объекта вдоль координат X и Y
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            //Если объект полностью скрывается за пределами экрана, то он начинает движение с противоположной стороны экрана 
            if (Pos.X <= -Image.Width)
                Pos.X = ScreenSize.Width;
            if (Pos.Y <= -Image.Height)
                Pos.Y = ScreenSize.Height;
            if (Pos.X >= ScreenSize.Width + Image.Width)
                Pos.X = 0;
            if (Pos.Y > ScreenSize.Height + Image.Height)
                Pos.Y = 0;
            //Эффект пульсации достигается изменением размера звезды
            if (Pulse.HasValue) //Пульсация включается толбко если задано значение поля Pulse
            {
                Size.Width += Pulse.Value;
                if (Size.Width >= 10 || Size.Width <= 1)
                    Pulse = Pulse * -1; //Если размер звезды достигает максилального значения, то она уменьшается, если достигает минимального значения - увеличивается
            }
        }
        #endregion
    }
}
