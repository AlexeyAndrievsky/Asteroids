using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Asteroids
{
    class XmlLoader
    {
        private string fileName;
        private List<GameObj> resList;

        public string FileName
        {
            set { fileName = value; }
        }

        public List<GameObj> ResList
        {
            get { return resList; }
        }

        public XmlLoader(string fileName)
        {
            this.fileName = fileName;
            resList = new List<GameObj>();
        }

        public void Load()
        {
            if (File.Exists(fileName))
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<GameObj>));
                Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                resList = (List<GameObj>)xmlFormat.Deserialize(fStream);
                fStream.Close();

                foreach (GameObj obj in resList)
                {
                    if (File.Exists(@"..\..\res\img\" + obj.imgName))
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
    }
}
