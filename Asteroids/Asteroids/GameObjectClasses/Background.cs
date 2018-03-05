using System.Collections.Generic;
using System.Drawing;

namespace Asteroids.GameObjectClasses
{
    /// <summary>
    /// Класс, реализующий заполнение фона фоновым изображением и обеспечивающий его циклическое пролистывание.
    /// Дочерний класс класса <see cref="ImageObject"/>
    /// </summary>
    class Background : ImageObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее список объектов <see cref="ImageObject"/>, из которых состоит фон.
        /// </summary>
        protected List<ImageObject> BackgroundList;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Background.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        public Background(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize) : base(pos, dir, size, image, graphic, screenSize)
        {
            BackgroundList = new List<ImageObject>();
            //Вложенный цикл формирует список объектов ImageObject таким образом, чтобы экран был полностью заполнен "мозайкой" из изображений
            for (int x = pos.X; x <= size.Width + image.Width; x += image.Width)
                for (int y = pos.Y; y < size.Height; y += image.Height)
                    BackgroundList.Add(new ImageObject(new Point(x, y), dir, image.Size, image, graphic, screenSize));
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки фона.
        /// </summary>
        public override void Draw()
        {
            foreach (ImageObject img in BackgroundList)
                img.Draw(); //Перебор в цикле всех элементов "мозайки", из которой состоит фон и отрисовка этих элементов
        }

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>
        public override void Update()
        {
            foreach (ImageObject img in BackgroundList)
                img.Update(); //Перебор в цикле всех элементов "мозайки" и вызов метода Update для каждого элемента
        }
        #endregion
    }
}
