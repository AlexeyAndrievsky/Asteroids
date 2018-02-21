using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Random rand;
        static ExceptionHelper exp;
        static Form _form;
        private static bool Flag;

        static Game()
        {
            exp = new ExceptionHelper();
        }

        public static List<BaseObject> _objs;

        public static void Load()
        {
            try
            {
                string resfile = @"..\..\res\res.xml";
                if (File.Exists(resfile))
                {
                    XmlLoader loader = new XmlLoader(resfile);
                    loader.Load();

                    _objs = new List<BaseObject>();
                    _objs.Add(new Background(new Point(0, 0), new Point(1, 0), new Size(Width, Height), Image.FromFile(@"..\..\res\img\background.jpeg"), Buffer.Graphics, new Size(Width, Height)));

                    rand = new Random();

                    for (int i = 0; i < 15; i++)
                        _objs.Add(new Star(new Point(rand.Next(Width + 10, Width + 650), rand.Next(0, Height)), new Point(rand.Next(15, 30), 0), new Size(rand.Next(3, 10), 0), loader.ResList.Where(t => t.resType == "star").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, Buffer.Graphics, new Size(Width, Height)));

                    for (int i = 0; i < 15; i++)
                        _objs.Add(new Meteor(new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), new Point(rand.Next(2, 9), 0), new Size(rand.Next(50, 110), rand.Next(50, 110)), loader.ResList.Where(t => t.resType == "asteroid").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, Buffer.Graphics, new Size(Width, Height), rand.Next(-10, 10)));
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
        public static void Init(Form form)
        {
            try
            {

                _form = form;
                _form.FormClosing += new FormClosingEventHandler(game_Closing);
                Flag = true;
                Graphics g;
                _context = BufferedGraphicsManager.Current;
                g = _form.CreateGraphics();
                Width = _form.Width;
                Height = _form.Height;

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

                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

                Load();
                Timer timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Init()", ex);
            }
        }

        private static void toMenu_Click(object sender, EventArgs e)
        {
            _form.Close();
        }

        private static void game_Closing(object sender, FormClosingEventArgs e)
        {
            Flag = false;
        }

        public static void Draw()
        {
            try
            {
                if (Flag)
                {
                    Buffer.Graphics.Clear(Color.Black);

                    foreach (BaseObject obj in _objs)
                        obj.Draw();

                    Buffer.Render();
                }
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Draw()", ex);
            }
        }

        public static void Update()
        {
            try
            {
                foreach (BaseObject obj in _objs)
                    obj.Update();
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Update()", ex);
            }

        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Flag)
                {
                    Draw();
                    Update();
                }
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Timer_Tick()", ex);
            }
        }    }}
