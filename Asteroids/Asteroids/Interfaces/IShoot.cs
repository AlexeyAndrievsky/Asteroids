using Asteroids.GameObjectClasses;

namespace Asteroids.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий стрельбу объектов.
    /// </summary>
    interface IShoot
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее значение задержки на перезарядку оружия.
        /// </summary>
        int Cooldown { get; }

        /// <summary>
        /// Флаг перезарядки оружия.
        /// </summary>
        bool CooldownFlag { get; }

        /// <summary>
        /// Счетчик перезарядки оружия.
        /// </summary>
        int CooldownCounter { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, описывающий стрельбу.
        /// </summary>
        /// <returns>Объект класса <see cref="Bullet"/> который должен быть добавлен в список игровых объектов</returns>
        Bullet Shoot();
        #endregion
    }

}

