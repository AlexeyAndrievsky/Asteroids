using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Random rand;

        static Game()
        {
        }

        public static List<BaseObject> _objs;

        public static void Load()
        {

            XmlLoader loader = new XmlLoader(@"..\..\res\res.xml");
            loader.Load();
            object a = loader.ResList;

            _objs = new List<BaseObject>();
            _objs.Add(new Background(new Point(0, 0), new Point(1, 0), new Size(Width, Height), Image.FromFile(@"..\..\res\img\background.jpeg")));

            rand = new Random();

            for (int i = 0; i < 15; i++)
                _objs.Add(new Star(new Point(rand.Next(Width + 10, Width + 650), rand.Next(0, Height)), new Point(rand.Next(15, 30), 0), new Size(rand.Next(3, 10), 0), loader.ResList.Where(t => t.resType == "star").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img));

            for (int i = 0; i < 15; i++)
                _objs.Add(new Meteor(new Point(rand.Next(Width + 100, Width + 650), rand.Next(0, Height)), new Point(rand.Next(2,9), 0), new Size(rand.Next(50, 110), rand.Next(50, 110)), loader.ResList.Where(t=>t.resType == "asteroid").OrderBy(arg => Guid.NewGuid()).Take(1).FirstOrDefault().img, rand.Next(-10,10)));

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
