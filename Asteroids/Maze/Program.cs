using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Width = Screen.PrimaryScreen.Bounds.Width;
            form.Height = Screen.PrimaryScreen.Bounds.Height;
            MazeScene maze = new MazeScene();
            maze.Init(form);
            form.Show();
            Application.Run(form);
        }
    }
}
