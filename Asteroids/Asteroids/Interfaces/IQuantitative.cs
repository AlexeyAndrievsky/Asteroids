namespace Asteroids.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий объекты, за уничтожение которых дают очки.
    /// </summary>
    interface IQuantitative
    {
        #region Fields
        /// <summary>
        /// Количество очков, которые игрок получает за уничтожение объекта.
        /// </summary>
        int Points { get; }
        #endregion
    }
}
