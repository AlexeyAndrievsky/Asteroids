using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс для отрисовки текста под произвольным углом и перемещения текста по экрану.
    /// Дочерний класс класса <see cref="BaseObject"/>
    /// </summary>
    class MovingText : BaseObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее угол отрисовки текста.
        /// </summary>
        protected float Angle;

        /// <summary>
        /// Поле, хранящее объекты, используемый для рисования текста.
        /// </summary>
        protected Brush Brsh;

        /// <summary>
        /// Поле, хранящее выводимый на экран текст.
        /// </summary>
        protected string Text;

        /// <summary>
        /// Поле, хранящее фонт текста.
        /// </summary>
        protected Font Fnt;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса MovingText.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="angle">Угол отрисовки текста</param>
        /// <param name="brush">Кисточка</param>
        /// <param name="font">Фонт текста</param>
        /// <param name="text">Выводимый текст</param>
        public MovingText(Point pos, Point dir, Size size, Graphics graphic, Size screenSize, float angle, Brush brush, Font font, string text) : base(pos, dir, size, graphic, screenSize)
        {
            //TODO: Слишком много параметров,неудобно. Думаю, стоит переписать
            Brsh = brush;
            Text = text;
            Angle = angle;
            Fnt = font;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Метод отрисовки текста.
        /// </summary>
        public override void Draw()
        {
            Graphic.TranslateTransform(Pos.X, Pos.Y); //Переносим точку рисования по заданным координатам
            Graphic.RotateTransform(Angle); //Поворачиваем на заданный угол
            Graphic.DrawString(Text, Fnt, Brsh, 0, 0); //Выводим текст
            Graphic.RotateTransform(-Angle); //Возвращаем угол в исходное положение
            Graphic.TranslateTransform(-Pos.X, -Pos.Y); //Возвращаем координаты в исходное положение
        }

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>
        public override void Update()
        {
            //Движение текста по осям X и Y
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;

            //Если текст доходит до края экрана, то он начинает двигаться в противоположном направлении
            if (Pos.X < 0)
                Dir.X = -Dir.X;
            if (Pos.X > ScreenSize.Width)
                Dir.X = -Dir.X;
            if (Pos.Y < 0)
                Dir.Y = -Dir.Y;
            if (Pos.Y > ScreenSize.Height)
                Dir.Y = -Dir.Y;
        }
        #endregion
    }
}
