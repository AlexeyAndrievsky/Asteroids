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
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Width = Screen.PrimaryScreen.Bounds.Width;
                form.Height = Screen.PrimaryScreen.Bounds.Height;
                SplashScreen.Init(form);
                form.Show();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                exp.PutMessage("Main", ex);
            }
        }
    }
}