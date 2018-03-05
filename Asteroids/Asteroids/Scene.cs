using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Asteroids.GameObjectClasses;

namespace Asteroids
{
    /// <summary>
    /// Базовый класс, описывающий "сцену" игры 
    /// </summary>
    public class Scene
    {
        #region Fields
        /// <summary>
        /// Контекст для создания графических буферов.
        /// </summary>
        protected BufferedGraphicsContext _context;

        /// <summary>
        /// Поле, хранящее объект - генератор псевдослучайных чисел.
        /// </summary>
        protected Random rand;

        /// <summary>
        /// Поле, хранящее объект класса <see cref="ExceptionHelper"/> для вывода исключений в консоль.
        /// </summary>
        public ExceptionHelper exp;

        /// <summary>
        /// Поле, хранящее объект - форму.
        /// </summary>
        protected Form _form;

        /// <summary>
        /// Графическии буфер для двойной буферизации.
        /// </summary>
        protected BufferedGraphics Buffer;

        /// <summary>
        /// Список объектов <see cref="BaseObject"/> для хранения и перебора игровых объектов, отображаемых на экране.
        /// </summary>
        public List<BaseObject> objs;

        /// <summary>
        /// Игровой таймер
        /// </summary>
        protected Timer timer;

        /// <summary>
        /// Ширина игровой области.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота игровой области.
        /// </summary>
        public int Height { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Game.
        /// </summary>
        public Scene()
        {
            exp = new ExceptionHelper();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод загрузки игры.
        /// </summary>
        public virtual void Load()
        {
            objs = new List<BaseObject>();
        }

        /// <summary>
        /// Метод инициализации игры.
        /// </summary>
        /// <param name="form">Форма, внутри которой реализована графика</param>
        public virtual void Init(Form form)
        {
            try
            {
                _form = form;
                rand = new Random(); //Инициализациея генератора псевдослучайных чисел

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

                Load(); //Вызов метода загрузки игры

                //Инициализация и запуск игрового таймера
                timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                exp.PutMessage("Scene.Init(): " + form.Width + "x" + form.Height + " Заданы некорректные значения размера экрана", ex);
            }
            catch (Exception ex)
            {
                exp.PutMessage("Scene.Init()", ex);
            }
        }

        /// <summary>
        /// Метод отрисовки игрового экрана.
        /// </summary>
        public virtual void Draw()
        {
            try
            {
                Buffer.Graphics.Clear(Color.Black); //Очистка области

                foreach (BaseObject obj in objs)
                    obj?.Draw(); //Перебор всех игровых объектов в цикле и вызов их метода отрисовки
                Buffer.Render();
            }
            catch (Exception ex)
            {
                exp.PutMessage("Scene.Draw()", ex);
            }
        }

        /// <summary>
        /// Метод обновления параметров игровых объектов.
        /// </summary>
        public virtual void Update()
        {
            try
            {
                foreach (BaseObject obj in objs)
                    obj?.Update(); //Перебор всех игровых объектов в цикле и вызов их метода обновления

            }
            catch (Exception ex)
            {
                exp.PutMessage("Scene.Update()", ex);
            }

        }
        #endregion

        #region Private methods
        /// <summary>
        /// Оброботчик события срабатывания игрового таймера.
        /// </summary>
        protected virtual void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                Draw(); //Вызов метода отрисовки игрового экрана
                Update(); //Вызов метода обновления параметров игровых объектов
            }
            catch (Exception ex)
            {
                exp.PutMessage("Scene.Timer_Tick()", ex);
            }
        }
        #endregion

    }
}
