using System.Drawing;

namespace Asteroids.GameObjectClasses
{
    /// <summary>
    /// Класс, описывающий аптечку для игрока.
    /// </summary>
    class RemontComplect : ImageObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее значение количества жизней, которое восполняет аптечка.
        /// </summary>
        public int Health { get; }

        /// <summary>
        /// Событие, возникающее при взаимодействии пули с другими объектами. 
        /// </summary>
        public event MessageEventHandler RemontComplectMessage;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса RemontComplect.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="health">Количество жизней, которое восполняет аптечка</param>
        public RemontComplect(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, int health) : base(pos, dir, size, image, graphic, screenSize)
        {
            Health = health;
        }
        #endregion

        #region Methods
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            //Если объект доходит до края экрана, то генерируется событие 
            if (Pos.X < -Size.Width || Pos.Y < -Size.Height || Pos.Y > ScreenSize.Height + Size.Height)
                RemontComplectMessage?.Invoke(this, new MessageEventArgs("Аптечка за пределами экрана.", 0, MessageEventArgs.EventTypeEnum.OutOfScreen));
        }
        #endregion
    }
}
