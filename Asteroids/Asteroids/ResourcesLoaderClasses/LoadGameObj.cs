using System;
using System.Drawing;

namespace Asteroids.ResourcesLoaderClasses
{
    /// <summary>
    /// Сериализуемый класс LoadGameObj для хранения загружаемых ресурсов.
    /// Используется в классе<seealso cref="XmlLoader">загружающем объекты из XML файла</seealso>
    /// </summary>
    [Serializable]
    public class LoadGameObj
    {
        #region Fields
        /// <summary>
        /// Имя файла, содержащего изображение.
        /// </summary>
        public string imgName { get; set; }

        /// <summary>
        /// Название типа игрового объекта.
        /// </summary>
        public string resType { get; set; }

        /// <summary>
        /// Загруженное изображение.
        /// </summary>
        public Image img { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса LoadGameObj по умолчанию.
        /// </summary>
        public LoadGameObj()
        {
            //not required any action
        }

        /// <summary>
        /// Конструктор класса LoadGameObj.
        /// </summary>
        /// <param name="imgName">Имя файла, содержащего изображение.</param>
        /// <param name="resType">Название типа игрового объекта.</param>
        public LoadGameObj(string imgName, string resType)
        {
            this.imgName = imgName;
            this.resType = resType;
        }
        #endregion
    }
}
