using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Asteroids
{
    /// <summary>
    /// Класс реализующий основную логику игры.
    /// </summary>
    static class Game
    {
        #region Fields
        /// <summary>
        /// Контекст для создания графических буферов.
        /// </summary>
        private static BufferedGraphicsContext _context;

        /// <summary>
        /// Поле, хранящее объект - генератор псевдослучайных чисел.
        /// </summary>
        private static Random rand;

        /// <summary>
        /// Флаг остановки игрового таймера.
        /// </summary>
        private static bool Flag;

        /// <summary>
        /// Поле, хранящее объект класса <see cref="ExceptionHelper"/> для вывода исключений в консоль.
        /// </summary>
        private static ExceptionHelper exp;

        /// <summary>
        /// Поле, хранящее объект - форму.
        /// </summary>
        private static Form _form;

        /// <summary>
        /// Графическии буфер для двойной буферизации.
        /// </summary>
        private static BufferedGraphics Buffer;

        /// <summary>
        /// Список объектов <see cref="BaseObject"/> для хранения и перебора игровых объектов, отображаемых на экране.
        /// </summary>
        private static List<BaseObject> _objs;

        /// <summary>
        /// Список объектов <see cref="Meteor"/> для хранения и перебора игровых объектов, отображаемых на экране.
        /// </summary>
        private static List<Meteor> _meteors;

        /// <summary>
        /// Ширина игровой области.
        /// </summary>
        public static int Width { get; set; }

        /// <summary>
        /// Высота игровой области.
        /// </summary>
        public static int Height { get; set; }

        /// <summary>
        /// Объект под управлением игроком
        /// </summary>
        public static Player PlayerObj { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Game.
        /// </summary>
        static Game()
        {
            exp = new ExceptionHelper();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод загрузки игры.
        /// </summary>
        public static void Load()
        {
            try
            {
                string resfile = @"..\..\res\res.xml"; //XML файл с данными о ресурсах игры
                if (File.Exists(resfile)) //Проверка наличия заданного файла
                {
                    XmlLoader loader = new XmlLoader(resfile);
                    loader.Load(); //Загрузка ресурсов игры

                    _objs = new List<BaseObject>(); //Инициализациея списка игровых объектов
                    _meteors = new List<Meteor>();
                    rand = new Random(); //Инициализациея генератора псевдослучайных чисел

                    _objs.Add(new Background(new Point(0, 0), new Point(-1, 0), new Size(Width, Height),
                        Image.FromFile(@"..\..\res\img\background.jpeg"), Buffer.Graphics, new Size(Width, Height))); //Создание заднего фона

                    for (int i = 0; i < 100; i++) //Добавление в список игровых объектов звезд. Положение, размер и скорость движения звезды выбираются случайным образом в некотором диапазоне
                        _objs.Add(new Star(
                            new Point(rand.Next(10, Width + 650), rand.Next(0, Height)), //Координаты звезды
                            new Point(rand.Next(-30, -15), 0), //Направление и скорость движения звезды
                            new Size(rand.Next(3, 10), 0), //Размеры звезды
                            loader.ResList.Where(t => t.resType == "star").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, //Изображение каждой звезды выбирается случайным образом из загруженных ресурсов
                            Buffer.Graphics,
                            new Size(Width, Height),
                            rand.Next(-2, 2) //Пульсация звезды
                            ));

                    for (int i = 0; i < 22; i++) //Добавление в список игровых объектов астероидов. Положение, размер и скорость движения астероида выбираются случайным образом в некотором диапазоне
                        _meteors.Add(new Meteor(
                            new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), //Координаты астероида
                            new Point(rand.Next(-9, -2), 0), //Направление и скорость движения астероида
                            new Size(rand.Next(50, 110), rand.Next(50, 110)), //Размеры астероида
                            loader.ResList.Where(t => t.resType == "asteroid").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, //Изображение каждого астероида выбирается случайным образом из загруженных ресурсов
                            Buffer.Graphics,
                            new Size(Width, Height),
                            rand.Next(-10, 10) //Направление и скорость вращения астероидов
                            ));

                    PlayerObj = new Player(
                            new Point(10, Height / 2), //Начальные координаты
                            new Point(0, 0), //Направление и скорость движения
                            new Size(200, 50), //Размеры
                            Image.FromFile(@"..\..\res\img\ship1.png"),
                            Buffer.Graphics,
                            new Size(Width, Height),
                            100, //Здоровье
                            20 // Максимальная скорость
                            );
                    _objs.Add(PlayerObj);
                }
                else
                {
                    throw new FileNotFoundException();
                }

            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Load()", ex);
            }
        }
        /// <summary>
        /// Метод инициализации игры.
        /// </summary>
        /// <param name="form">Форма, внутри которой реализована графика</param>
        public static void Init(Form form)
        {
            try
            {
                _form = form;
                _form.KeyPreview = true;
                _form.FormClosing += new FormClosingEventHandler(game_Closing); //Добавление обработчика события на закрывание формы
                _form.KeyDown += new KeyEventHandler(game_KeyPressing); //Добавление обработчика события на нажатие клавиш
                _form.KeyUp += new KeyEventHandler(game_KeyUp); //Добавление обработчика события "отжатие" клавиш
                Flag = true; //Флаг таймера

                //Инициализация графики
                Graphics g;
                _context = BufferedGraphicsManager.Current;
                g = _form.CreateGraphics();
                if (form.Width <= 0 || form.Height <= 0 || form.Width > 1920 || form.Height > 1080)
                {
                    throw new ArgumentOutOfRangeException();
                }
                Width = _form.Width;
                Height = _form.Height;
                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

                #region Button add
                //Добавление кнопки выхода в меню
                Button toMenu = new Button();
                toMenu.Size = new Size(190, 32);
                toMenu.Location = new Point(Width - 200, 10);
                toMenu.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                toMenu.Name = "startGame";
                toMenu.TabIndex = 0;
                toMenu.Text = "Меню";
                toMenu.Click += new EventHandler(toMenu_Click);
                toMenu.BackColor = Color.FromArgb(120, 0, 0, 0);
                _form.Controls.Add(toMenu);
                #endregion

                Load(); //Вызов метода загрузки игры

                //Инициализация и запуск игрового таймера
                Timer timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                exp.PutMessage("Game.Init(): " + form.Width + "x" + form.Height + " Заданы некорректные значения размера экрана", ex);
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Init()", ex);
            }
        }

        /// <summary>
        /// Метод отрисовки игрового экрана.
        /// </summary>
        public static void Draw()
        {
            try
            {
                Buffer.Graphics.Clear(Color.Black); //Очистка области

                foreach (BaseObject obj in _objs)
                    obj.Draw(); //Перебор всех игровых объектов в цикле и вызов их метода отрисовки
                foreach (Meteor obj in _meteors)
                    obj.Draw(); //Перебор всех игровых объектов в цикле и вызов их метода отрисовки

                Buffer.Render();
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Draw()", ex);
            }
        }

        /// <summary>
        /// Метод обновления параметров игровых объектов.
        /// </summary>
        public static void Update()
        {
            try
            {
                List<Meteor> toRemove = new List<Meteor>();
                foreach (BaseObject obj in _objs)
                    obj.Update(); //Перебор всех игровых объектов в цикле и вызов их метода обновления
                foreach (Meteor obj in _meteors)
                {
                    obj.Update(); //Перебор всех игровых объектов в цикле и вызов их метода отрисовки
                    if (obj.Collision(PlayerObj))
                        toRemove.Add(obj);
                }
                foreach (Meteor obj in toRemove)
                    _meteors.Remove(obj);

            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Update()", ex);
            }

        }
        #endregion
        #region Private methods
        /// <summary>
        /// Оброботчик события срабатывания игрового таймера.
        /// </summary>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Flag) //Проверка флага игрового таймера
                {
                    Draw(); //Вызов метода отрисовки игрового экрана
                    Update(); //Вызов метода обновления параметров игровых объектов
                }
                else
                {
                    (sender as Timer).Stop(); //Остановка таймера
                }
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Timer_Tick()", ex);
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки возврата в меню.
        /// </summary>
        private static void toMenu_Click(object sender, EventArgs e)
        {
            _form.Close(); //Закрытие текущей формы
        }

        /// <summary>
        /// Обработчик события, возникающего при закрытии формы.
        /// </summary>
        private static void game_Closing(object sender, FormClosingEventArgs e)
        {
            Flag = false; //Выставление флага для остановки таймера
        }

        /// <summary>
        /// Обработчик события отпускания клавиш
        /// </summary>
        private static void game_KeyUp(object sender, KeyEventArgs e)
        {
            PlayerObj.StopMoving();
        }

        /// <summary>
        /// Обработчик события нажатия клавиш
        /// </summary>
        private static void game_KeyPressing(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                PlayerObj.MoveUp();
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                PlayerObj.MoveDown();
        }


        #endregion    }}
