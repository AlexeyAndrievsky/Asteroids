using System.Drawing;

namespace Asteroids.GameObjectClasses
{
    class Bullet : ImageObject
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее значение количества урона, наносимого пулей.
        /// </summary>
        public int Damage { get; }

        /// <summary>
        /// Поле, хранящее значение, которое определяет выпущена ли пуля игроком (true если пулю выпустил игрок)
        /// </summary>
        public bool IsPlayerBullet { get; }

        /// <summary>
        /// Событие, возникающее при взаимодействии пули с другими объектами. 
        /// </summary>
        public event MessageEventHandler BulletMessage;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Bullet.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="damage">Количество урона, которое наносит пуля</param>
        /// <param name="isPlayerBullet">Выпущена ли пуля игроком (true если пулю выпустил игрок)</param>
        public Bullet(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, int damage, bool isPlayerBullet) : base(pos, dir, size, image, graphic, screenSize)
        {
            Damage = damage;
            IsPlayerBullet = isPlayerBullet;
        }
        #endregion

        #region Methods
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            //Если объект доходит до края экрана, то генерируется событие 
            if (Pos.X < -3 * Size.Width || Pos.X > ScreenSize.Width + 3 * Size.Width || Pos.Y < -3 * Size.Height || Pos.Y > ScreenSize.Height + 3 * Size.Height)
                BulletMessage?.Invoke(this, new MessageEventArgs("Пуля за пределами экрана.", 0, MessageEventArgs.EventTypeEnum.OutOfScreen));
        }
        #endregion
    }
}
 