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

        static Game()
        {
            exp = new ExceptionHelper();
        }

        public static List<BaseObject> _objs;

        public static void Load()
        {
            try
            {
                string resfile = @"..\..\res\refs.xml";
                if (File.Exists(resfile))
                {
                    XmlLoader loader = new XmlLoader(resfile);
                    loader.Load();
                    object a = loader.ResList;

                    _objs = new List<BaseObject>();
                    _objs.Add(new Background(new Point(0, 0), new Point(1, 0), new Size(Width, Height), Image.FromFile(@"..\..\res\img\background.jpeg")));

                    rand = new Random();

                    for (int i = 0; i < 15; i++)
                        _objs.Add(new Star(new Point(rand.Next(Width + 10, Width + 650), rand.Next(0, Height)), new Point(rand.Next(15, 30), 0), new Size(rand.Next(3, 10), 0), loader.ResList.Where(t => t.resType == "star").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img));

                    for (int i = 0; i < 15; i++)
                        _objs.Add(new Meteor(new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), new Point(rand.Next(2, 9), 0), new Size(rand.Next(50, 110), rand.Next(50, 110)), loader.ResList.Where(t => t.resType == "asteroid").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, rand.Next(-10, 10)));
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
                Graphics g;
                _context = BufferedGraphicsManager.Current;
                g = form.CreateGraphics();
                Width = form.Width;
                Height = form.Height;

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
                Draw();
                Update();
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.Timer_Tick()", ex);
            }
        }        public static void DrawString(Graphics g, string s, int x, int y, int angle, Brush brush)
        {
            try
            {
                g.TranslateTransform(x, y);
                g.RotateTransform(angle);
                g.DrawString(s, new Font(new FontFamily("Arial"), 16), brush, 0, 0);
                g.RotateTransform(-angle);
                g.TranslateTransform(-x, -y);
            }
            catch (Exception ex)
            {
                exp.PutMessage("Game.DrawString()", ex);
            }
        }
    }
}
