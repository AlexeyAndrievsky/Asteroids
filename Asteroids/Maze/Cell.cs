using Asteroids.GameObjectClasses;
using System.Drawing;
using System;

namespace Maze
{
    /// <summary>
    /// Класс, описывающий ячейку лабиринта.
    /// Дочерний класс класса <see cref="BaseObject"/>
    /// </summary>
    class Cell : BaseObject
    {
        #region Fields
        /// <summary>
        /// Позиция ячейки в массиве ячеек по X.
        /// </summary>
        public int X;

        /// <summary>
        /// Позиция ячейки в массиве ячеек по Y.
        /// </summary>
        public int Y;

        /// <summary>
        /// Является ли ячейка стеной.
        /// </summary>
        public bool IsWall;

        /// <summary>
        /// Посещена ли ячейка в процессе генерации лабиринта.
        /// </summary>
        public bool IsVisited;

        /// <summary>
        /// Цвет ячейки.
        /// </summary>
        protected Brush CellBrush;

        /// <summary>
        /// Является ли ячейка входом.
        /// </summary>
        public bool IsEnter;

        /// <summary>
        /// Является ли ячейка выходом.
        /// </summary>
        public bool IsExit;

        /// <summary>
        /// Посещена ли ячейка в процессе нахождения пути в лабиринте.
        /// </summary>
        public bool IsFinding;
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса Cell
        /// </summary>
        /// <param name="x">Позиция ячейки в массиве ячеек по X.</param>
        /// <param name="y">Позиция ячейки в массиве ячеек по Y.</param>
        /// <param name="isWall">Является ли ячейка стеной.</param>
        /// <param name="isVisited">Посещена ли ячейка в процессе генерации лабиринта.</param>
        /// <param name="graphic">Поверхность рисования</param>
        /// <param name="screenSize">Размеры области рисования</param>
        public Cell(int x, int y, bool isWall, bool isVisited, Graphics graphic, Size screenSize) : base(new Point(x * 20, y * 20), new Point(0, 0), new Size(20, 20), graphic, screenSize)
        {
            X = x;
            Y = y;
            IsVisited = isVisited;
            IsWall = isWall;
            CellBrush = new SolidBrush(Color.Gray);
            IsEnter = false;
            IsExit = false;
            IsFinding = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод отрисовки ячейки.
        /// </summary>
        public override void Draw()
        {
            Rectangle rect = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Graphic.FillRectangle(CellBrush, rect);
        }

        /// <summary>
        /// Метод обновления параметров ячейки.
        /// </summary>
        public override void Update()
        {
            //Выбирается цвет ячейки в зависимости от ее типа.
            if (IsWall)
                CellBrush = new SolidBrush(Color.DarkGray);
            else if (IsVisited)
                CellBrush = new SolidBrush(Color.White);
            else
                CellBrush = new SolidBrush(Color.LightGray);

            if (!IsWall)
            {
                if (IsEnter)
                    CellBrush = new SolidBrush(Color.DarkGreen);
                if (IsExit)
                    CellBrush = new SolidBrush(Color.DarkRed);
                if (IsFinding)
                    CellBrush = new SolidBrush(Color.LightGreen);
            }
        }
        #endregion
    }


}
 