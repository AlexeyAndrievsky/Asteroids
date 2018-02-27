using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Asteroids
{
    /// <summary>
    /// Класс, реализующий меню игры.
    /// </summary>
    static class SplashScreen
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
        /// Ширина игровой области.
        /// </summary>
        public static int Width { get; set; }

        /// <summary>
        /// Высота игровой области.
        /// </summary>
        public static int Height { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса SplashScreen.
        /// </summary>
        static SplashScreen()
        {
            exp = new ExceptionHelper();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод загрузки главного меню.
        /// </summary>
        public static void Load()
        {
            try
            {
                rand = new Random(); //Инициализациея генератора псевдослучайных чисел
                _objs = new List<BaseObject>(); //Инициализациея списка игровых объектов

                Image img = Image.FromFile(@"..\..\res\img\bluestar.png"); //Загрузка изображения, используемого в эффекте "сквозь космос"
                Image ttl = Image.FromFile(@"..\..\res\img\title.png"); //Загрузка изображения заголовка игры

                #region Text adding
                _objs.Add(new MovingText( //Добавление плавающего текста в список отображаемых объектов
                    new Point(0, Height - 100), //Начальные координаты текста
                    new Point(10, 0), //Направление и скорость движение текста 
                    new Size(0, 0),
                    Buffer.Graphics,
                    new Size(Width, Height),
                    0F, //Угол отрисовки текста
                    Brushes.DarkGreen, //Цвет текста
                    new Font("Arial", 12), //Фонт текста
                    "© Alexey.B.Andrievsky ~ 2018" //Отображаемый текст
                    ));

                _objs.Add(new MovingText( //Добавление перевернутово плавающего текста в список отображаемых объектов
                    new Point(Width, Height - 60), //Начальные координаты текста
                    new Point(-10, 0), //Направление и скорость движение текста 
                    new Size(0, 0),
                    Buffer.Graphics,
                    new Size(Width, Height),
                    180F, //Угол отрисовки текста
                    Brushes.DarkGreen, //Цвет текста
                    new Font("Arial", 12), //Фонт текста
                    "© Alexey.B.Andrievsky ~ 2018" //Отображаемый текст
                    ));

                _objs.Add(new MovingText( //Добавление текста предупреждения в список отображаемых объектов
                    new Point(10, 10), //Начальные координаты текста
                    new Point(0, 0), //Направление и скорость движение текста 
                    new Size(0, 0),
                    Buffer.Graphics,
                    new Size(Width, Height),
                    0F, //Угол отрисовки текста
                    Brushes.DarkRed, //Цвет текста
                    new Font("Arial", 12), //Фонт текста
                    "WARNING: This video game may potentially trigger seizures for people with photosensitive epilepsy.Viewer discretion is advised." //Отображаемый текст
                    ));
                #endregion

                _objs.Add(new ImageObject( //Добавление заголовка в список отображаемых объектов
                    new Point(Width / 2 - 350, 300), //Координаты заголовка
                    new Point(0, 0), //Направление и скорость движения
                    new Size(700, 100), //Размер заголовка
                    ttl, //Загруженное изображение заголовка
                    Buffer.Graphics,
                    new Size(Width, Height)
                    ));

                for (int i = 0; i < 150; i++)
                    _objs.Add(new ThroughtSpace( //Добавление элементов, образующих эффект "сквозь космос" в список отображаемых объектов
                        new Point(rand.Next(0, Width), rand.Next(0, Height)), //Начальные координаты каждого элемента задаются случайным образом
                        new Point(5, 5), //Скорость движения элементов
                        new Size(1, 1), //Начальный размер элементов
                        img, //Загруженное изображение, используемое в эффекте
                        rand,
                        Buffer.Graphics,
                        new Size(Width, Height),
                        null //Пульсация элементов выключена
                        ));
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Load()", ex);
            }
        }

        /// <summary>
        /// Метод инициализации главного меню.
        /// </summary>
        /// <param name="form">Форма, внутри которой реализована графика</param>
        public static void Init(Form form)
        {
            try
            {
                _form = form;

                //Инициализация графики
                Graphics g;
                _context = BufferedGraphicsManager.Current;
                g = form.CreateGraphics();
                if (form.Width <= 0 || form.Height <= 0 || form.Width > 1920 || form.Height > 1080)
                {
                    throw new ArgumentOutOfRangeException();
                }
                Width = form.Width;
                Height = form.Height;
                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

                #region Buttons add
                //Добавление кнопки начала игры
                Button startGame = new Button();
                startGame.Size = new Size(200, 32);
                startGame.Location = new Point(Width / 2 - startGame.Width / 2, Height / 2 - 50);
                startGame.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                startGame.Name = "startGame";
                startGame.TabIndex = 0;
                startGame.Text = "Начать игру!";
                startGame.Click += new EventHandler(startGame_Click);
                startGame.BackColor = Color.FromArgb(120, 0, 0, 0);
                _form.Controls.Add(startGame);

                //Добавление кнопки, открывающей страницу рекордов
                Button records = new Button();
                records.Size = new Size(200, 32);
                records.Location = new Point(Width / 2 - startGame.Width / 2, Height / 2 - 10);
                records.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                records.Name = "records";
                records.TabIndex = 0;
                records.Text = "Рекорды";
                records.Click += new EventHandler(records_Click);
                records.BackColor = Color.FromArgb(120, 0, 0, 0);
                _form.Controls.Add(records);

                //Добавление кнопки выхода из игры
                Button quit = new Button();
                quit.Size = new Size(200, 32);
                quit.Location = new Point(Width / 2 - startGame.Width / 2, Height / 2 + 30);
                quit.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                quit.Name = "quit";
                quit.TabIndex = 0;
                quit.Text = "Выход";
                quit.Click += new EventHandler(quit_Click);
                quit.BackColor = Color.FromArgb(120, 0, 0, 0);
                _form.Controls.Add(quit);
                #endregion

                Load(); //Вызов метода загрузки игры

                //Инициализация и запуск игрового таймера
                Timer timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                exp.PutMessage("SplashScreen.Init(): " + form.Width + "x" + form.Height + " Заданы некорректные значения размера экрана", ex);
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Init()", ex);
            }
        }

        /// <summary>
        /// Метод отрисовки экрана меню.
        /// </summary>
        public static void Draw()
        {
            try
            {
                Buffer.Graphics.Clear(Color.Black); //Очистка области

                foreach (BaseObject obj in _objs)
                    obj.Draw(); //Перебор всех игровых объектов в цикле и вызов их метода отрисовки

                Buffer.Render();
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Draw()", ex);
            }
        }

        /// <summary>
        /// Метод обновления параметров объектов главного меню.
        /// </summary>
        public static void Update()
        {
            try
            {
                foreach (BaseObject obj in _objs)
                    obj.Update(); //Перебор всех объектов главного меню в цикле и вызов их метода обновления
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Update()", ex);
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
                Draw(); //Вызов метода отрисовки игрового экрана
                Update(); //Вызов метода обновления параметров игровых объектов
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Timer_Tick()", ex);
            }
        }

        /// <summary>
        /// Обработчик события, возникающего при закрытии формы, в которой реализована игра.
        /// </summary>
        private static void game_Closed(object sender, FormClosedEventArgs e)
        {
            _form.Visible = true; //При закрытии формы с игрой, форма главного меню становится видимой
        }

        #region Controls event handdlers
        /// <summary>
        /// Обработчик события нажатия кнопки выхода из игры.
        /// </summary>
        private static void quit_Click(object sender, EventArgs e)
        {
            _form.Close(); //Закрытие текущей формы
        }

        /// <summary>
        /// Обработчик события нажатия кнопки начала игры.
        /// </summary>
        private static void startGame_Click(object sender, EventArgs e)
        {
            _form.Visible = false; //Форма главного меню скрывается

            //Инициализация формы для игры
            Form gameForm = new Form();
            gameForm.StartPosition = FormStartPosition.CenterScreen;
            gameForm.FormBorderStyle = FormBorderStyle.None;
            gameForm.Width = Screen.PrimaryScreen.Bounds.Width;
            gameForm.Height = Screen.PrimaryScreen.Bounds.Height;
            Game.Init(gameForm);
            gameForm.Show();
            gameForm.FormClosed += new FormClosedEventHandler(game_Closed); //Добавление события, возникающего при закрытии формы с игрой
        }

        /// <summary>
        /// Обработчик события нажатия кнопки вызова таблицы рекордов.
        /// </summary>
        private static void records_Click(object sender, EventArgs e)
        {
            //TODO: разработать функционал, хранящий и отображающий рекорды
#if DEBUG
            throw new NotImplementedException();
#endif
        }




        #endregion

        #endregion
    }
}
