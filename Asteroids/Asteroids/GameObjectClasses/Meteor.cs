using Asteroids.Interfaces;
using System;
using System.Drawing;

namespace Asteroids.GameObjectClasses
{
    /// <summary>
    /// Класс, описывающий астероид и реализующий его отрисовку и перемещение.
    /// Дочерний класс класса <see cref="ImageObject"/>
    /// </summary>
    class Meteor : ImageObject, IDestroyable, IQuantitative
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее значение угла, на которое осуществляется поворот астероида.
        /// </summary>
        protected float Angle;

        /// <summary>
        /// Поле, хранящее значение угла отрисовки астероида.
        /// </summary>
        protected float DAngle;

        /// <summary>
        /// Поле, хранящее количество жизней астероида.
        /// </summary>
        public int Health { get; protected set; } = 30;

        /// <summary>
        /// Поле, хранящее максимальное количество жизней астероида.
        /// </summary>
        public int MaxHealth { get; protected set; } = 30;

        /// <summary>
        /// Количество очков, которые получает игрок за уничтожение астероида.
        /// </summary>
        public int Points { get; protected set; } = 50;

        /// <summary>
        /// Событие, возникающее при взаимодействии астероида с другими объектами. 
        /// </summary>
        public event MessageEventHandler EnemyMessage;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Meteor.
        /// </summary>
        /// <param name="pos">Координаты объекта на экране</param>
        /// <param name="dir">Направление и скорость движения по осям X и Y</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="image">Изображение, использующееся для отрисовки объекта</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        /// <param name="isCollisionEnabled">Нужна ли обработка коллизии объекту</param>
        /// <param name="dAngle">Угол, на который астероид поворачивается за один цикл Update</param>
        public Meteor(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, float dAngle) : base(pos, dir, size, image, graphic, screenSize)
        {
            DAngle = dAngle;
            Angle = 0; //Начальное значение угла отрисовки изображения
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Метод отрисовки астероида.
        /// </summary>
        public override void Draw()
        {
            //Рассчет центра вращения
            int x = Pos.X + Size.Width / 2;
            int y = Pos.Y + Size.Height / 2;
            double _angle = Angle * Math.PI / 180; //перевод градусов в радианы

            //Эффект вращения реализуется путем отрисовки изображения в параллелограмме, наклоненного в нужную сторону на определенный градус
            //ниже реализовар рассчет координат вершин параллелограмма 
            double x1 = x + ((-Size.Width / 2) * Math.Cos(_angle) - (-Size.Height / 2) * Math.Sin(_angle)); //Координаты верхней правой вершины
            double y1 = y + ((-Size.Width / 2) * Math.Sin(_angle) + (-Size.Height / 2) * Math.Cos(_angle));

            double x2 = x + ((Size.Width / 2) * Math.Cos(_angle) - (-Size.Height / 2) * Math.Sin(_angle)); //Координаты верхнего левой вершины
            double y2 = y + ((Size.Width / 2) * Math.Sin(_angle) + (-Size.Height / 2) * Math.Cos(_angle));

            double x3 = x + ((Size.Width / 2) * Math.Cos(_angle) - (Size.Height / 2) * Math.Sin(_angle)); //Координаты нижней правой вершины
            double y3 = y + ((Size.Width / 2) * Math.Sin(_angle) + (Size.Height / 2) * Math.Cos(_angle));

            Point[] destPoints = { new Point((int)x2, (int)y2), new Point((int)x1, (int)y1), new Point((int)x3, (int)y3) };
            Graphic.DrawImage(Image, destPoints);

            // TODO: разобраться почему способ, приведенный ниже дает глюк
            //
            //Game.Buffer.Graphics.TranslateTransform(Pos.X+Size.Width/2, Pos.Y+Size.Height/2);
            //Game.Buffer.Graphics.RotateTransform(Angle);
            //Game.Buffer.Graphics.DrawImage(ObjectImage, new Rectangle(0, 0, Size.Width, Size.Height));
            //Game.Buffer.Graphics.RotateTransform(-Angle);
            //Game.Buffer.Graphics.TranslateTransform(-(Pos.X + Size.Width / 2), -(Pos.Y + Size.Height / 2));
            //
        }

        /// <summary>
        /// Метод обновления параметров объекта.
        /// </summary>        public override void Update()
        {
            //Движение объекта вдоль координат X и Y
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;

            if (Pos.X < -Size.Width || Pos.Y < -Size.Height || Pos.Y > ScreenSize.Height + Size.Height)
                //Если объект доходит до края экрана, то генерируется событие 
                EnemyMessage?.Invoke(this, new MessageEventArgs("Астероид не был уничтожен, и теперь направляется в сторону планеты.", -2, MessageEventArgs.EventTypeEnum.OutOfScreen));

            //Поворот объекта на заданный угол
            Angle += DAngle;
            if (Angle >= 360) //Если угол больше или равен 360 градусов, то он выставляется в 0
                Angle = 0;
        }

        /// <summary>
        /// Метод, описывающий получение урона.
        /// </summary>
        /// <param name="damage">Количество жизней, которые следует отнять у объекта</param>
        public void GetDamage(int damage)
        {
            System.Media.SystemSounds.Asterisk.Play();
            Health -= damage;
            EnemyMessage?.Invoke(this, new MessageEventArgs("Астероид поврежден.", damage, MessageEventArgs.EventTypeEnum.GotDamage));
            if (Health <= 0)
                Die();
        }

        /// <summary>
        /// Метод, описывающий лечение объекта.
        /// </summary>
        /// <param name="health">количество здоровья, на которое следует увеличить жизни объекта.</param>
        public void GetHealed(int health)
        {
            //TODO: Так как астероид не лечится, есть смысл вынести лечение в отдельный интерфейс
        }

        /// <summary>
        /// Метод, описывающий смерть объекта.
        /// </summary>
        public void Die()
        {
            System.Media.SystemSounds.Exclamation.Play();
            EnemyMessage?.Invoke(this, new MessageEventArgs("Астероид уничтожен!", Points, MessageEventArgs.EventTypeEnum.Killed));
        }
        #endregion    }
}
