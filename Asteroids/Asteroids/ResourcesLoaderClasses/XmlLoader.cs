using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Asteroids.ResourcesLoaderClasses
{
    /// <summary>
    /// Класс для загрузки ресурсов из xml файла.
    /// </summary>
    class XmlLoader
    {
        #region Fields
        /// <summary>
        /// Имя XML файла.
        /// </summary>
        private string fileName;

        /// <summary>
        /// Список объектов <see cref="LoadGameObj"/>, подгружаемых из xml.
        /// </summary>
        private List<LoadGameObj> resList;
        #endregion

        #region Properties
        /// <summary>
        /// Свойство поля fileName.
        /// </summary>
        public string FileName
        {
            set { fileName = value; }
        }

        /// <summary>
        /// Свойство для доступа к полю resList.
        /// </summary>
        public List<LoadGameObj> ResList
        {
            get { return resList; }
        }
        #endregion

        #region .ctor
        /// <summary>
        /// Конструктор класса XmlLoader.
        /// </summary>
        /// <param name="fileName">имя xml файла</param>
        public XmlLoader(string fileName)
        {
            this.fileName = fileName;
            resList = new List<LoadGameObj>();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Загрузка данных из XML файла.
        /// </summary>
        public void Load()
        {
            if (File.Exists(fileName)) //проверка наличия файла
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<LoadGameObj>));
                Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                resList = (List<LoadGameObj>)xmlFormat.Deserialize(fStream); //Десериализация данных
                fStream.Close();

                foreach (LoadGameObj obj in resList) //Подгрузка изображений, по именам файлов, полученных из XML
                {
                    if (File.Exists(@"..\..\res\img\" + obj.imgName)) //Проверка наличия изображения с данным именем, иначе - удаление объекта из списка
                        obj.img = Image.FromFile(@"..\..\res\img\" + obj.imgName);
                    else
                        resList.Remove(obj);
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        #endregion
    }
}
