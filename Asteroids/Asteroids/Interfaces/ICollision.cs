using System.Drawing;

namespace Interfaces.Asteroids
{
    /// <summary>
    /// Интерфейс обработки коллизий
    /// </summary>
    public interface ICollision
    {
        #region fields
        /// <summary>
        /// Поле, хранящее коллайдер в виде прямоугольника.
        /// </summary>
        Rectangle Rect { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Метод проверки произошла ли коллизия с объектом obj.
        /// </summary>
        /// <param name="obj">Объект, на пересечение с которым необходима проверка</param>
        /// <returns>Если true - коллизия произошла, false - коллизии не было</returns>
        bool Collision(ICollision obj);
        #endregion
    }
}
