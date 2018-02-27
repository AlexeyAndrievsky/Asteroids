using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Интерфейс обработки коллизий
    /// </summary>
    interface ICollision
    {
        #region fields
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
        #endregion
    }
}
