using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asteroids;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Asteroids.GameObjectClasses;

namespace Maze
{
    /// <summary>
    /// Класс, визуализирующий алгоритм Эйлера для построения лабиринта.
    /// Дочерний класс класса <see cref="Scene"/>
    /// </summary>
    class MazeScene : Scene
    {
        #region Fields
        Stack<Cell> cellStack;
        int MWidth = 51;
        int MHeight = 51;
        Cell startCell;
        Cell currentCell;
        Cell nCell;
        Cell Exit;
        bool IsGenerated = false;
        #endregion

        #region Public methods
        /// <summary>
        /// Метод инициализации лабиринта.
        /// </summary>
        /// <param name="form">Форма, внутри которой реализована графика</param>
        public override void Init(Form form)
        {
            cellStack = new Stack<Cell>();
            _form = form;

            base.Init(_form);

            timer.Interval = 12;
            #region Button add
            //Добавление кнопки выхода
            Button toMenu = new Button();
            toMenu.Size = new Size(190, 32);
            toMenu.Location = new Point(Width - 200, 10);
            toMenu.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            toMenu.Name = "exitGame";
            toMenu.TabIndex = 0;
            toMenu.Text = "Выход";
            toMenu.Click += new EventHandler(toMenu_Click);
            toMenu.BackColor = Color.FromArgb(120, 0, 0, 0);
            _form.Controls.Add(toMenu);
            #endregion
        }

        /// <summary>
        /// Метод первоначальной загрузки игры. 
        /// </summary>
        public override void Load()
        {
            base.Load();

            for (int i = 0; i < MWidth; i++)
                for (int j = 0; j < MHeight; j++)
                    if ((i % 2 != 0 && j % 2 != 0) && (i < MHeight - 1 && j < MWidth - 1))
                        objs.Add(new Cell(i, j, false, false, Buffer.Graphics, new Size(Width, Height)));
                    else
                        objs.Add(new Cell(i, j, true, true, Buffer.Graphics, new Size(Width, Height)));
            startCell = objs.Where(o => o is Cell).Where(m => (m as Cell).X == 1 && (m as Cell).Y == 1).FirstOrDefault() as Cell;
            currentCell = startCell;
            currentCell.IsVisited = true;
        }
        
        /// <summary>
        /// Метод обновления параметров объектов лабиринта.
        /// </summary>
        public override void Update()
        {
            if (!IsGenerated)
                if ((objs.Where(o => o is Cell).Count(m => !(m as Cell).IsVisited) > 0))
                {
                    List<BaseObject> nc = objs.Where(o => o is Cell).Where(m => (((m as Cell).X == currentCell.X && (m as Cell).Y == currentCell.Y - 2) || ((m as Cell).X == currentCell.X + 2 && (m as Cell).Y == currentCell.Y) || ((m as Cell).X == currentCell.X && (m as Cell).Y == currentCell.Y + 2) || ((m as Cell).X == currentCell.X - 2 && (m as Cell).Y == currentCell.Y)) && !(m as Cell).IsWall && !(m as Cell).IsVisited).ToList().ToList();
                    if (nc.Count > 0)
                    {
                        cellStack.Push(currentCell);
                        nCell = nc.OrderBy(arg => Guid.NewGuid()).First() as Cell;
                        nCell.IsVisited = true;
                        removeWall(currentCell, nCell);
                        currentCell = nCell;

                    }
                    else if (cellStack.Count > 0)
                    {
                        nCell = cellStack.Pop();
                        nCell.IsVisited = true;
                        currentCell = nCell;
                    }
                    else
                    {
                        nCell = objs.Where(m => m is Cell).Where(m => !(m as Cell).IsVisited).OrderBy(arg => Guid.NewGuid()).First() as Cell;
                        nCell.IsVisited = true;
                        currentCell = nCell;
                    }
                }
                else
                {

                    Cell enter = objs.Where(o => o is Cell).Where(m => !(m as Cell).IsWall).OrderBy(arg => Guid.NewGuid()).First() as Cell;
                    enter.IsEnter = true;

                    Exit = objs.Where(o => o is Cell).Where(m => !(m as Cell).IsWall && !(m as Cell).IsEnter).OrderBy(arg => Guid.NewGuid()).First() as Cell;
                    Exit.IsExit = true;
                    currentCell = enter;

                    cellStack.Clear();
                    IsGenerated = true;
                }
            else
            {
                if (!(currentCell.X == Exit.X && currentCell.Y == Exit.Y))
                {
                    List<BaseObject> nc = objs.Where(o => o is Cell).Where(m => (((m as Cell).X == currentCell.X && (m as Cell).Y == currentCell.Y - 1) || ((m as Cell).X == currentCell.X + 1 && (m as Cell).Y == currentCell.Y) || ((m as Cell).X == currentCell.X && (m as Cell).Y == currentCell.Y + 1) || ((m as Cell).X == currentCell.X - 1 && (m as Cell).Y == currentCell.Y)) && !(m as Cell).IsWall && !(m as Cell).IsFinding).ToList().ToList();
                    if (nc.Count > 0)
                    {
                        cellStack.Push(currentCell);
                        nCell = nc.OrderBy(arg => Guid.NewGuid()).First() as Cell;
                        nCell.IsFinding = true;
                        currentCell = nCell;
                    }

                    else if (cellStack.Count > 0)
                    {
                        nCell = cellStack.Pop();
                        nCell.IsFinding = true;
                        currentCell = nCell;
                    }
                    else
                    {
                        timer.Stop();
                        Buffer.Graphics.DrawString("Выхода нет.", new Font(FontFamily.GenericSansSerif, 20, FontStyle.Underline), Brushes.White, Width-250, 100);
                        Buffer.Render();
                    }
                }
                else
                {
                    timer.Stop();
                    Buffer.Graphics.DrawString("Выход найден.", new Font(FontFamily.GenericSansSerif, 20, FontStyle.Underline), Brushes.White, Width - 250, 100);
                    Buffer.Render();
                }
            }
            base.Update();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Удаление стены между двумя ячейками.
        /// </summary>
        /// <param name="first">Первая ячейка</param>
        /// <param name="second">Вторая ячейка</param>
        private void removeWall(Cell first, Cell second)
        {
            int xDiff = second.X - first.X;
            int yDiff = second.Y - first.Y;
            int addX, addY;

            addX = (xDiff != 0) ? (xDiff / Math.Abs(xDiff)) : 0;
            addY = (yDiff != 0) ? (yDiff / Math.Abs(yDiff)) : 0;

            Cell cell = objs.Where(o => o is Cell).Where(m => ((m as Cell).X == first.X + addX) && ((m as Cell).Y == first.Y + addY)).First() as Cell;
            cell.IsWall = false;
            cell.IsVisited = true;
        }


        
        /// <summary>
        /// Обработчик события нажатия кнопки выхода.
        /// </summary>
        private void toMenu_Click(object sender, EventArgs e)
        {
            _form.Close();
        }
        #endregion
    }
}
