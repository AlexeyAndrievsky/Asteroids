using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Asteroids.GameObjectClasses;
using Asteroids.ResourcesLoaderClasses;
using Asteroids.Interfaces;

namespace Asteroids
{
    /// <summary>
    /// Класс реализующий основную логику игры.
    /// Дочерний класс класса <see cref="Scene"/>
    /// </summary>
    class Game : Scene
    {
        #region Fields
        /// <summary>
        /// Объект под управлением игроком
        /// </summary>
        public Player PlayerObj { get; private set; }

        /// <summary>
        /// Список объектов, которые необходимо удалить.
        /// </summary>
        private List<BaseObject> ObjectsToRemove;

        /// <summary>
        /// Список объектов, которые необходимо добавить.
        /// </summary>
        private List<BaseObject> ObjectsToAdd;

        /// <summary>
        /// Хранилище загруженных ресурсов.
        /// </summary>
        private XmlLoader loader;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Game.
        /// </summary>
        public Game() : base()
        {
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Метод загрузки игры.
        /// </summary>
        public override void Load()
        {
            try
            {
                string resfile = @"..\..\res\res.xml"; //XML файл с данными о ресурсах игры
                if (File.Exists(resfile)) //Проверка наличия заданного файла
                {
                    base.Load();

                    loader = new XmlLoader(resfile);
                    loader.Load(); //Загрузка ресурсов игры

                    objs.Add(new Background(new Point(0, 0), new Point(-1, 0), new Size(Width, Height),
                        Image.FromFile(@"..\..\res\img\background.jpeg"), Buffer.Graphics, new Size(Width, Height))); //Создание заднего фона

                    for (int i = 0; i < 100; i++) //Добавление в список игровых объектов звезд. Положение, размер и скорость движения звезды выбираются случайным образом в некотором диапазоне
                        objs.Add(new Star(
                            new Point(rand.Next(10, Width + 650), rand.Next(0, Height)), //Координаты звезды
                            new Point(rand.Next(-30, -15), 0), //Направление и скорость движения звезды
                            new Size(rand.Next(3, 10), 0), //Размеры звезды
                            loader.ResList.Where(t => t.resType == "star").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, //Изображение каждой звезды выбирается случайным образом из загруженных ресурсов
                            Buffer.Graphics,
                            new Size(Width, Height),
                            rand.Next(-2, 2) //Пульсация звезды
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
                    objs.Add(PlayerObj);
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
        public override void Init(Form form)
        {
            try
            {
                StreamWriter sw = new StreamWriter(@"log.txt", false);
                sw.WriteLine("ИГРА НАЧАЛАСЬ.");
                sw.Close();

                _form = form;
                base.Init(_form);
                _form.KeyPreview = true;
                _form.FormClosing += new FormClosingEventHandler(game_Closing); //Добавление обработчика события на закрывание формы
                _form.KeyDown += new KeyEventHandler(game_KeyPressing); //Добавление обработчика события на нажатие клавиш
                _form.KeyUp += new KeyEventHandler(game_KeyUp); //Добавление обработчика события "отжатие" клавиш
                ObjectsToRemove = new List<BaseObject>();
                ObjectsToAdd = new List<BaseObject>();

                PlayerObj.EventMessage += MessageArrive;

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

            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Init()", ex);
            }
        }

        /// <summary>
        /// Метод обновления параметров игровых объектов.
        /// </summary>
        public override void Update()
        {
            try
            {
                GenerateObjects(); //Вызод метода добавления объектов в игру

                //TODO: Если реализовать это все через for, а не foreach, можно попробовать обойтись одним циклом.
                foreach (BaseObject objr in ObjectsToRemove)
                {
                    objs.Remove(objr); //Удаление объектов, помеченных на удаление
                }
                foreach (BaseObject obja in ObjectsToAdd)
                {
                    objs.Add(obja); //Добавление объектов
                }
                ObjectsToRemove.Clear();
                ObjectsToAdd.Clear();
                foreach (var obj in objs) //Цикл перебора всех игровых объектов
                {
                    obj?.Update(); //Обновление параметров объектов
                    if (obj is Enemy)
                        if (rand.Next(0, 100) >= 80)
                        {
                            Bullet nb = (obj as Enemy).Shoot(); //Враг стреляет в момент, выбранный случайным образом
                            if (nb != null)
                                ObjectsToAdd.Add(nb);
                        }

                    if (obj is IDestroyable && obj != PlayerObj) //Проверка коллизии игрока с врагами и астероидами
                        if (obj.Collision(PlayerObj))
                        {
                            (obj as IDestroyable).Die();
                            PlayerObj.GetDamage(rand.Next(5, 20));
                        }

                    if (obj is RemontComplect) //Проверка коллизии с аптечкой
                        if (obj.Collision(PlayerObj))
                        {
                            ObjectsToRemove.Add(obj);
                            PlayerObj.GetHealed((obj as RemontComplect).Health);
                        }

                    if (obj is Bullet)
                    {
                        if ((obj as Bullet).IsPlayerBullet) //Проверка коллизии пуль игрока с врагами и астероидами
                        {
                            foreach (var en in objs.Where(o => o is IDestroyable && !(o is Player)).ToList())
                                if (obj.Collision(en))
                                {
                                    ObjectsToRemove.Add(obj);
                                    (en as IDestroyable).GetDamage((obj as Bullet).Damage);
                                }
                        }
                        else //Проверка коллизии пуль врага с игроком
                        {
                            if (obj.Collision(PlayerObj))
                            {
                                if (obj.Collision(PlayerObj))
                                {
                                    ObjectsToRemove.Add(obj);
                                    PlayerObj.GetDamage((obj as Bullet).Damage);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Update()", ex);
            }

        }
        #endregion
        #region Private methods
        /// <summary>
        /// Обработчик события нажатия кнопки возврата в меню.
        /// </summary>
        private void toMenu_Click(object sender, EventArgs e)
        {
            _form.Close(); //Закрытие текущей формы
        }

        /// <summary>
        /// Обработчик события, возникающего при закрытии формы.
        /// </summary>
        private void game_Closing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }

        /// <summary>
        /// Обработчик события отпускания клавиш
        /// </summary>
        private void game_KeyUp(object sender, KeyEventArgs e)
        {
            PlayerObj.StopMoving();
        }

        /// <summary>
        /// Обработчик события нажатия клавиш
        /// </summary>
        private void game_KeyPressing(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                PlayerObj.MoveUp();
            if (e.KeyCode == Keys.S)
                PlayerObj.MoveDown();
            if (e.KeyCode == Keys.D)
                PlayerObj.MoveForward();
            if (e.KeyCode == Keys.A)
                PlayerObj.MoveBack();
            if (e.KeyCode == Keys.ControlKey)
            {
                Bullet nb = PlayerObj.Shoot();
                if (nb != null)
                    objs.Add(nb);
            }
        }

        /// <summary>
        /// Метод, вызываемый при возникновении внутриигрового события.
        /// </summary>
        private void MessageArrive(object sender, MessageEventArgs e)
        {
            
            switch (e.EventType)
            {
                case MessageEventArgs.EventTypeEnum.GotDamage: //Получение урона
                    {
                        Log($"{e.Text} Нанесено {e.IntParam} урона.");
                    }
                    break;
                case MessageEventArgs.EventTypeEnum.Healed: //Лечение
                    {
                        Log($"{e.Text} Получено {e.IntParam} очков жизни.");
                    }
                    break;
                case MessageEventArgs.EventTypeEnum.Killed: //Уничтожение
                    {
                        if (sender is Player) //Если уничтожен игрок
                        {
                            timer.Stop();
                            string text = $"Конец игры: {e.Text} Вы заработали {e.IntParam} очков.";
                            Buffer.Graphics.DrawString(text, new Font(FontFamily.GenericSansSerif, 20, FontStyle.Underline), Brushes.White, 200, 100);
                            Buffer.Render();
                            Log(text);
                        }
                        else //Если уничтожен враг или астероид
                        {
                            PlayerObj.AddPoints(e.IntParam);
                            Log($"{e.Text} Получено {e.IntParam} очков.");
                            ObjectsToRemove.Add(sender as BaseObject);
                        }
                    }
                    break;
                case MessageEventArgs.EventTypeEnum.OutOfScreen: //Вылет за пределы экрана
                    {
                        ObjectsToRemove.Add(sender as BaseObject);
                        if (!(sender is Bullet) && !(sender is RemontComplect))
                            Log($"{e.Text} Потеряно {e.IntParam} очков.");
                    }
                    break;
            }
   
        }

        /// <summary>
        /// Метод, выводящий лог игры в консоль и в файл.
        /// </summary>
        /// <param name="s">Текст лога</param>
        private void Log(string s)
        {
            Console.WriteLine(s);
            StreamWriter writer = new StreamWriter("log.txt",true);
            writer.WriteLine(s);
            writer.Close();
        }

        /// <summary>
        /// Метод добавление нв игру объектов.
        /// </summary>
        private void GenerateObjects()
        {
            int rnd = rand.Next(0, 1000);
            if (rnd > 900)
            {
                Meteor ast = new Meteor(
                        new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), //Координаты астероида
                        new Point(rand.Next(-9, -2), 0), //Направление и скорость движения астероида
                        new Size(rand.Next(50, 110), rand.Next(50, 110)), //Размеры астероида
                        loader.ResList.Where(t => t.resType == "asteroid").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, //Изображение каждого астероида выбирается случайным образом из загруженных ресурсов
                        Buffer.Graphics,
                        new Size(Width, Height),
                        rand.Next(-10, 10));
                ast.EnemyMessage += MessageArrive;
                objs.Add(ast);
            }
            else if (rnd < 10)
            {
                RemontComplect heal = new RemontComplect(
                        new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), //Координаты аптечки
                        new Point(rand.Next(-9, -2), 0), //Направление и скорость движения аптечки
                        new Size(35, 35), //Размеры аптечки
                        Image.FromFile(@"..\..\res\img\remkomplect.png"),
                        Buffer.Graphics,
                        new Size(Width, Height),
                        rand.Next(10,60)); //Количество жизней, восполняемых ремкомплектом
                heal.RemontComplectMessage += MessageArrive;
                objs.Add(heal);
            }
            else if (rnd > 890)
            {
                Enemy en = new Enemy(
                        new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), //Координаты врага
                        new Point(rand.Next(-5, -2), 0), //Направление и скорость движения врага
                        new Size(216, 303), //Размеры врага
                        Image.FromFile(@"..\..\res\img\ship2.png"),
                        Buffer.Graphics,
                        new Size(Width, Height),
                        rand.Next(60, 90), //Количество жизней врага
                        rand);
                en.EventMessage += MessageArrive;
                objs.Add(en);
            }

        }



        #endregion    }}
