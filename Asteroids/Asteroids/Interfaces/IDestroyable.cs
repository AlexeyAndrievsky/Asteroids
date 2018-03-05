using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Interfaces
{
    /// <summary>
    /// Интрефейс, описывающий поведение объекта, который может понести урон, вылечиться и умереть. 
    /// </summary>
    interface IDestroyable
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее текущее количество здоровья объекта.
        /// </summary>
        int Health { get; }

        /// <summary>
        /// Поле, хранящее максимальное количество здоровья объекта.
        /// </summary>
        int MaxHealth { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, описывающий получение урона.
        /// </summary>
        /// <param name="damage">Количество жизней, которые следует отнять у объекта</param>
        void GetDamage(int damage);

        /// <summary>
        /// Метод, описывающий лечение объекта.
        /// </summary>
        /// <param name="health">количество здоровья, на которое следует увеличить жизни объекта.</param>
        void GetHealed(int health);

        /// <summary>
        /// Метод, описывающий смерть объекта.
        /// </summary>
        void Die();
        #endregion
    }
}
