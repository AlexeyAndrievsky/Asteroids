using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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

        static SplashScreen()
        {
            exp = new ExceptionHelper();
        }

        public static List<BaseObject> _objs;

        public static void Load()
        {
            try
            {
                rand = new Random();
                _objs = new List<BaseObject>();
                for (int i = 0; i < 100; i++)
                    _objs.Add(ThroughtSpaceGenerator(Image.FromFile(@"..\..\res\img\whitestar.png"), 5));
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.Load()", ex);
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
                exp.PutMessage("SplashScreen.Init()", ex);
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
        }        public static ThroughtSpace ThroughtSpaceGenerator(Image image, int speed)
        {
            try
            {
                return new ThroughtSpace(new Point(rand.Next(0, Width), rand.Next(0, Height)), new Point(speed, speed), new Size(1, 1), image, rand);
            }
            catch (Exception ex)
            {
                exp.PutMessage("SplashScreen.ThroughtSpaceGenerator()", ex);
                return null;
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
                exp.PutMessage("SplashScreen.DrawString()", ex);
            }
        }
    }
}
