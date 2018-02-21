using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Asteroids
{
    static class SplashScreen
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Random rand;
        static ExceptionHelper exp;
        public static List<BaseObject> _objs;
        static Form _form;

        static SplashScreen()
        {
            exp = new ExceptionHelper();
        }

        public static void Init(Form form)
        {
            try
            {
                _form = form;

                Graphics g;
                _context = BufferedGraphicsManager.Current;
                g = form.CreateGraphics();
                Width = form.Width;
                Height = form.Height;

                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

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

                Button records = new Button();
                records.Size = new Size(200, 32);
                records.Location = new Point(Width / 2 - startGame.Width / 2, Height / 2 - 10);
                records.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                records.Name = "records";
                records.TabIndex = 0;
                records.Text = "Рекорды";
                records.Click += new EventHandler(startGame_Click);
                records.BackColor = Color.FromArgb(120, 0, 0, 0);
                _form.Controls.Add(records);

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

                Load();
                Timer timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Init()", ex);
            }
        }

        private static void quit_Click(object sender, EventArgs e)
        {
            _form.Close();
        }

        private static void startGame_Click(object sender, EventArgs e)
        {
            _form.Visible = false;

            Form gameForm = new Form();
            gameForm.StartPosition = FormStartPosition.CenterScreen;
            gameForm.FormBorderStyle = FormBorderStyle.None;
            gameForm.Width = Screen.PrimaryScreen.Bounds.Width;
            gameForm.Height = Screen.PrimaryScreen.Bounds.Height;
            Game.Init(gameForm);
            gameForm.Show();
            gameForm.FormClosed += new FormClosedEventHandler(game_Closed);
        }

        private static void game_Closed(object sender, FormClosedEventArgs e)
        {
            _form.Visible = true;
        }

        public static void Load()
        {
            try
            {
                rand = new Random();
                _objs = new List<BaseObject>();
                Image img = Image.FromFile(@"..\..\res\img\bluestar.png");
                Image ttl = Image.FromFile(@"..\..\res\img\title.png");
                _objs.Add(new MovingText(new Point(0, Height - 100), new Point(10, 0), new Size(0, 0), Buffer.Graphics, new Size(Width, Height), 0F, Brushes.DarkGreen, new Font("Arial", 12), "© Alexey.B.Andrievsky ~ 2018"));
                _objs.Add(new MovingText(new Point(Width, Height - 60), new Point(-10, 0), new Size(0, 0), Buffer.Graphics, new Size(Width, Height), 180F, Brushes.DarkGreen, new Font("Arial", 12), "© Alexey.B.Andrievsky ~ 2018"));
                _objs.Add(new ImageObject(new Point(Width / 2 - 350, 300), new Point(0, 0), new Size(700, 100), ttl, Buffer.Graphics, new Size(Width, Height)));
                for (int i = 0; i < 150; i++)
                    _objs.Add(new ThroughtSpace(new Point(rand.Next(0, Width), rand.Next(0, Height)), new Point(5, 5), new Size(1, 1), img, rand, Buffer.Graphics, new Size(Width, Height)));
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Load()", ex);
            }
        }
        public static void Draw()
        {
            try
            {
                Buffer.Graphics.Clear(Color.Black);

                foreach (BaseObject obj in _objs)
                    obj.Draw();

                Buffer.Render();
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Draw()", ex);
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
                exp.PutMessage("SplashScreen.Update()", ex);
            }

        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                Draw();
                Update();
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Timer_Tick()", ex);
            }
        }    }}
