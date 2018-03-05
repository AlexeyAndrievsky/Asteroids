using Asteroids.Interfaces;
using System;
using System.Drawing;

namespace Asteroids.GameObjectClasses
{
    /// <summary>
    /// Класс, описывающий врага.
    /// </summary>
    class Enemy : ImageObject, IShoot, IDestroyable, IQuantitative
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее количество жизней врага.
        /// </summary>
        public int Health { get; protected set; }

        /// <summary>
        /// Поле, хранящее максимальное количество жизней врага.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Поле, реализующее значение задержки на перезарядку оружия.
        /// </summary>
        public int Cooldown { get; protected set; } = 10;

        /// <summary>
        /// Поле, реализующее флаг перезарядки оружия.
        /// </summary>
        public bool CooldownFlag { get; protected set; } = false;

        /// <summary>
        /// Поле, хранящее значение счетчика перезарятки оружия.
        /// </summary>
        public int CooldownCounter { get; protected set; } = 0;

        /// <summary>
        /// Количество очков, которые получает игрок за уничтожение врага.
        /// </summary>
        public int Points { get; protected set; } = 250;

        /// <summary>
        /// Событие, порожденное врагом. 
        /// </summary>
        public event MessageEventHandler EventMessage;

        /// <summary>
        /// Поле, хранящее генератор псевдослучайных чисел. 
        /// </summary>
        private Random Rand;
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
        public Enemy(Point pos, Point dir, Size size, Image image, Graphics graphic, Size screenSize, int health, Random rand) : base(pos, dir, size, image, graphic, screenSize)
        {
            Health = MaxHealth = health;
            Rand = rand;
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
                Pos.X = Pos.X + Dir.X;
                Pos.Y = Pos.Y + Dir.Y;

            if (Pos.X < -Size.Width)
                //Если объект доходит до края экрана, то генерируется событие 
                EventMessage?.Invoke(this, new MessageEventArgs("Корабль не был уничтожен, и теперь планета в опасности.", -10, MessageEventArgs.EventTypeEnum.OutOfScreen));

            if (Pos.Y < 0 || Pos.Y > ScreenSize.Height) //Если объект доходит до края экрана по оси Y, он меняет направление движения по оси Y на противоположное
                Dir.Y *= -1;
            else
                Dir.Y = Rand.Next(-10, 10); //Направление и скорость движения по оси Y выбирается случайным образом

            

            if (CooldownCounter == 0 && CooldownFlag) // перезарядка оружия (CooldownFlag=true значит идет перезарядка)
            {
                CooldownFlag = false;
            }
            else if (CooldownCounter > 0 && CooldownFlag)
            {
                CooldownCounter--;
            }
        }

        /// <summary>
        /// Метод, реализующий выстрел врага
        /// </summary>
        public Bullet Shoot()
        {
            if (!CooldownFlag)
            {
                CooldownFlag = true;
                CooldownCounter = Cooldown;
                return new Bullet(
                    new Point(pos.X + 20, pos.Y + Size.Height / 2), //Координаты, откуда вылетает пуля относительно корабля
                    new Point(-98, 0), //Направление и скорость полета пули
                    new Size(89, 13), //Размер пули
                    Image.FromFile(@"..\..\res\img\laserred.png"), //Изображение пули
                    Graphic,
                    ScreenSize,
                    5, //Количество урона, который наносит пуля
                    false //Пуля выпущенна игроком
                    );
            }
            else return null;
        }

        /// <summary>
        /// Метод, описывающий получение урона.
        /// </summary>
        /// <param name="damage">Количество жизней, которые следует отнять у объекта</param>
        public void GetDamage(int damage)
        {
            System.Media.SystemSounds.Asterisk.Play();
            Health -= damage;
            EventMessage?.Invoke(this, new MessageEventArgs("Игрок нанес врагу повреждения.", damage, MessageEventArgs.EventTypeEnum.GotDamage));
            if (Health <= 0)
                Die();
        }

        /// <summary>
        /// Метод, описывающий лечение объекта.
        /// </summary>
        /// <param name="health">количество здоровья, на которое следует увеличить жизни объекта.</param>
        public void GetHealed(int health)
        {
            if (Health < MaxHealth)
            {
                System.Media.SystemSounds.Beep.Play();
                Health += health;
            }
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
                EventMessage?.Invoke(this, new MessageEventArgs("Прочность корабля врага полностью восстановлена.", health, MessageEventArgs.EventTypeEnum.Healed));
            }
            else
            {
                EventMessage?.Invoke(this, new MessageEventArgs("Корабль врага отремантирован...", health, MessageEventArgs.EventTypeEnum.Healed));
            }
        }

        /// <summary>
        /// Метод, описывающий смерть врага.
        /// </summary>
        public void Die()
        {
            System.Media.SystemSounds.Exclamation.Play();
            EventMessage?.Invoke(this, new MessageEventArgs("Враг уничтожен!", Points, MessageEventArgs.EventTypeEnum.Killed));
        }

        #endregion
    }
}
