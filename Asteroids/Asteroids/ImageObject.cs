using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс, реализующий движущийся объект с изображением.
    /// Дочерний класс класса <see cref="BaseObject"/>
    /// </summary>
    class ImageObject : BaseObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее изображение.
        /// </summary>
        public Image Image { get; }
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
        public ImageObject(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize) : base(pos, dir, size, graphic, screenSize)
        {
            Image = image;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки объекта.
        /// </summary>
        public override void Draw()
        {
            Graphic.DrawImage(Image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }

        /// <summary>
        /// Метод обновления параметров объекта.
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
        }
        #endregion
    }
}
