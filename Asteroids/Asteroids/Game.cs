using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { get; set; }
        public static int Height { get; set; }

        static Game()
        {
        }

        public static List<BaseObject> _objs;

        public static void Load()
        {
            _objs = new List<BaseObject>();
            _objs.Add(new Background(new Point(0, 0), new Point(1, 0), new Size(Width, Height), Image.FromFile(@"..\..\res\img\background.jpeg")));
            for (int i = 0; i < 1; i++)
                _objs.Add(new Meteor(new Point(100, 100), new Point(2, 0), new Size(121, 107), Image.FromFile(@"..\..\res\img\ast1.png"),3));
            int cnt = _objs.Count;
            for (int i = cnt; i < cnt + 15; i++)
                _objs.Add(new Star(new Point(600, i * 20), new Point(i, 0), new Size(10, 10), Image.FromFile(@"..\..\res\img\bluestar.png")));
        }
        public static void Init(Form form)
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

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject obj in _objs)
                obj.Draw();
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
           
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}
