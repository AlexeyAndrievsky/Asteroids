using System;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс для создания элементов, образующих эффек движения сквозь космос.
    /// Дочерний класс класса <see cref="Star"/>
    /// </summary>
    class ThroughtSpace : Star
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее генератор псевдослучайных чисел. 
        /// </summary>
        private Random rand;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса ThroughtSpace.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="rand">Генератор псевдослучайных чисел</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="pulse">Значение, определяющее пульсацию элементов</param>
        public ThroughtSpace(Point pos, Point dir, Size size, Image image, Random rand, Graphics graphic, Size screenSize, int? pulse) : base(pos, dir, size, image, graphic, screenSize, pulse)
        {
            this.rand = rand;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки элемента.
        /// </summary>
        public override void Draw()
        {
            base.Draw();
        }

        /// <summary>
        /// Метод обновления параметров элемента.
        /// </summary>
        public override void Update()
        {
            //ЗD эффект достигается зависимостью скорости движения элемента от расстояния элемента от центра экрана:
            //чем дальше элемент от центра экрана, тем быстрее он движется;
            //а так же благодаря увеличению элемента при "приближении к камере"
            if (Pos.X < ScreenSize.Width / 2 && Pos.Y < ScreenSize.Height / 2) //Если элемент левее и выше центра экрана, то он движется влево и наверх
            {
                Pos.X = Pos.X - GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y - GetKoeff().Y * Dir.Y;
            }
            else if (Pos.X >= ScreenSize.Width / 2 && Pos.Y < ScreenSize.Height / 2) //Если элемент правее и выше центра экрана, то он движется вправо и наверх
            {
                Pos.X = Pos.X + GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y - GetKoeff().Y * Dir.Y;
            }
            else if (Pos.X < ScreenSize.Width / 2 && Pos.Y >= ScreenSize.Height / 2) //Если элемент левее и ниже центра экрана, то он движется влево и вниз
            {
                Pos.X = Pos.X - GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y + GetKoeff().Y * Dir.Y;
            }
            else //Если элемент левее и выше центра экрана, то он движется вправо и вниз
            {
                Pos.X = Pos.X + GetKoeff().X * Dir.X;
                Pos.Y = Pos.Y + GetKoeff().Y * Dir.Y;
            }
            Size.Width += 1; //Увеличение размера элемента

            //проверка: при выходе элемента за пределы экрана, а так же при достижении определенного размера элемента,
            //размер элемента сбрасывается на минимум, а координаты выставляются случайным образом в пределах экрана
            if (Size.Width > 10 || Pos.X > ScreenSize.Width || Pos.X < 0 || Pos.Y > ScreenSize.Height || Pos.Y < 0)
            {
                Size.Width = 1;
                Pos.X = rand.Next(0, ScreenSize.Width);
                Pos.Y = rand.Next(0, ScreenSize.Height);
            }

        }
        #endregion

        #region Private methods
        /// <summary>
        /// Метод для рассчета коэффициента скорости движения элемента, в зависимости от положения на экране.
        /// </summary>
        /// <returns>Коэффициент скорости для осей X и Y</returns>
        private Point GetKoeff()
        {
            return new Point(Math.Abs(ScreenSize.Width / 2 - Pos.X) / 50, Math.Abs(ScreenSize.Height / 2 - Pos.Y) / 50);
        }
        #endregion
    }
}
