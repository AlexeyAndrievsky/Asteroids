using System;
using System.Windows.Forms;

namespace Asteroids
{
    /// <summary>
    /// Класс вывода сообщения об исключениях в консоль.
    /// </summary>
    public class ExceptionHelper
    {
        #region Public methods
        /// <summary>
        /// Метод, выводящий исключение в консоль.
        /// </summary>
        /// <param name="AddInfo">Строка с дополнительной информацией об исключении</param>
        /// <param name="ex">Объект, хранящий информацию об исключении</param>
        public void PutMessage(string AddInfo, Exception ex)
        {
            Console.WriteLine("Error occurred in " + AddInfo + ": " + ex.Message + " " + ex.TargetSite);
            Console.ReadLine(); //Запрос нажатия клавиши чтобы сообщения не пролистывались
            Environment.Exit(0);
        }
        #endregion
    }
}
