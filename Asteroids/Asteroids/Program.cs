using System;
using System.Windows.Forms;
namespace Asteroids
{
    class Program
    {
        static ExceptionHelper exp;
        static void Main(string[] args)
        {
            try
            {
                exp = new ExceptionHelper();
                Form form = new Form();
                form.Width = 800;
                form.Height = 600;
                Game.Init(form);
                form.Show();
                Application.Run(form);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                exp.PutMessage("Main", ex);
            }
        }
    }
}