namespace Asteroids
{
    /// <summary>
    /// Класс, реализующий аргументы делегата MessageEventHandler.
    /// </summary>
    public class MessageEventArgs
    {
        #region Fields
        /// <summary>
        /// Поле, хранящее строку с текстом для передачи в сообщении.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Перечисление, хранящее тип пересланного сообщения.
        /// </summary>
        public enum EventTypeEnum
        {
            /// <summary>
            /// Сообщение возникло при исцелении объекта.
            /// </summary>
            Healed,

            /// <summary>
            /// Сообщение возникло при получении урона объектом.
            /// </summary>
            GotDamage,

            /// <summary>
            /// Сообщение возникло при смерти объекта.
            /// </summary>
            Killed,

            /// <summary>
            /// Событие возникло при выходе объекта з пределы игровой области.
            /// </summary>
            OutOfScreen
        };

        /// <summary>
        /// Поле, хранящее тип сообщения.
        /// </summary>
        public EventTypeEnum EventType { get; private set; }

        /// <summary>
        /// Поле для передачи дополнительного числового значения (количество набранных очков или понесенного урона).
        /// </summary>
        public int IntParam { get; private set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса MessageEventArgs.
        /// </summary>
        /// <param name="s">Строка с текстом для передачи в сообщении</param>
        /// <param name="i">Количество набранных очков</param>
        /// <param name="e">Тип сообщения</param>
        public MessageEventArgs(string s, int i, EventTypeEnum e)
        {
            Text = s;
            IntParam = i;
            EventType = e;
        }
        #endregion
    }
}
