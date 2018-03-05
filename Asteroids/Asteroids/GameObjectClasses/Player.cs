using Asteroids.Interfaces;
using System.Drawing;

namespace Asteroids.GameObjectClasses
{
    /// <summary>
    /// Класс, описывающий игрока.
    /// </summary>
    class Player : ImageObject, IShoot, IDestroyable
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее количество жизней игрока.
        /// </summary>
        public int Health { get; protected set; }

        /// <summary>
        /// Поле, хранящее максимальное количество жизней игрока.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Поле, хранящее текущую максимальную скорость корабля игрока.
        /// </summary>
        protected int MaxVelocity { get; }

        /// <summary>
        /// Флаг движения по оси X
        /// </summary>
        protected bool MovingFlagX;

        /// <summary>
        /// Флаг движения по оси Y
        /// </summary>
        protected bool MovingFlagY;

        /// <summary>
        /// Поле, реализующее значение задержки на перезарядку оружия.
        /// </summary>
        public int Cooldown { get; protected set; } = 4;

        /// <summary>
        /// Поле, реализующее флаг перезарядки оружия.
        /// </summary>
        public bool CooldownFlag { get; protected set; } = false;

        /// <summary>
        /// Поле, хранящее значение счетчика перезарятки оружия.
        /// </summary>
        public int CooldownCounter { get; protected set; } = 0;

        /// <summary>
        /// Количество очков, набранных игроком.
        /// </summary>
        public int ScoredPoints { get; protected set; } = 0;

        /// <summary>
        /// Событие, порожденное игроком. 
        /// </summary>
        public event MessageEventHandler EventMessage;
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
            Health = MaxHealth = health;
            MaxVelocity = maxVelocity;
            MovingFlagX = false;
            MovingFlagY = false;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод отрисовки объекта.
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            Graphic.DrawString($"{Health}", new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold), Brushes.White, Pos.X, Pos.Y-25);
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

            if (Pos.Y <= 0)
            {
                Pos.Y = 1;
                Dir.Y = 0;
            }

            if (Pos.X >= ScreenSize.Width - Image.Width)
            {
                Pos.X = ScreenSize.Width - Image.Width - 1;
                Dir.X = 0;
            }

            if (Pos.Y >= ScreenSize.Height - Image.Height)
            {
                Pos.Y = ScreenSize.Height - Image.Height - 1;
                Dir.Y = 0;
            }

            if (!MovingFlagY) //инерция
            {
                if (Dir.Y != 0)
                    if (Dir.Y < 0)
                        Dir.Y++;
                    else
                        Dir.Y--;
            }

            if (!MovingFlagX)
            {
                if (Dir.X != 0)
                    if (Dir.X < 0)
                        Dir.X++;
                    else
                        Dir.X--;
            }

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
        /// Ускорение при движении вниз
        /// </summary>
        public void MoveDown()
        {
            MovingFlagY = true;
            if (Dir.Y < MaxVelocity)
                Dir.Y++;
        }

        /// <summary>
        /// Ускорение при движении вверх
        /// </summary>
        public void MoveUp()
        {
            MovingFlagY = true;
            if (Dir.Y > -MaxVelocity)
                Dir.Y--;
        }

        /// <summary>
        /// Ускорение при движении вперед
        /// </summary>
        public void MoveForward()
        {
            MovingFlagX = true;
            if (Dir.X < MaxVelocity)
                Dir.X++;
        }

        /// <summary>
        /// Ускорение при движении назад
        /// </summary>
        public void MoveBack()
        {
            MovingFlagX = true;
            if (Dir.X > -MaxVelocity)
                Dir.X--;
        }

        /// <summary>
        /// Остановка движения.
        /// </summary>
        public void StopMoving()
        {
            MovingFlagX = false;
            MovingFlagY = false;
        }

        /// <summary>
        /// Метод, реализующий выстрел игрока
        /// </summary>
        public Bullet Shoot()
        {
            if (!CooldownFlag)
            {
                CooldownFlag = true;
                CooldownCounter = Cooldown;
                return new Bullet(
                    new Point(pos.X + Size.Width - 20, pos.Y + Size.Height / 2), //Координаты, откуда вылетает пуля относительно корабля
                    new Point(98, 0), //Направление и скорость полета пули
                    new Size(89, 13), //Размер пули
                    Image.FromFile(@"..\..\res\img\lasergreen.png"), //Изображение пули
                    Graphic,
                    ScreenSize,
                    10, //Количество урона, который наносит пуля
                    true //Пуля выпущенна игроком
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
            EventMessage?.Invoke(this, new MessageEventArgs("Корабль получил повреждения.", damage, MessageEventArgs.EventTypeEnum.GotDamage));
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
                EventMessage?.Invoke(this, new MessageEventArgs("Прочность корабля полностью восстановлена.", health, MessageEventArgs.EventTypeEnum.Healed));
            }
            else
            {
                EventMessage?.Invoke(this, new MessageEventArgs("Корабль игрока отремантирован...", health, MessageEventArgs.EventTypeEnum.Healed));
            }
        }

        /// <summary>
        /// Метод, реализующий добавление очков, заработанных в игре.
        /// </summary>
        /// <param name="points"></param>
        public void AddPoints (int points)
        {
            ScoredPoints += points;
        }

        /// <summary>
        /// Метод, описывающий смерть объекта.
        /// </summary>
        public void Die()
        {
            System.Media.SystemSounds.Exclamation.Play();
            EventMessage?.Invoke(this, new MessageEventArgs("Корабль игрока уничтожен.", ScoredPoints, MessageEventArgs.EventTypeEnum.Killed));
        }
        
        #endregion

    }
}
