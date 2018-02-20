using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Asteroids
{
    class XmlLoader
    {
        string fileName;
        List<GameObj> ResList;

        public string FileName
        {
            set { fileName = value; }
        }

        public int Count
        {
            get { return ResList.Count; }
        }

        public XmlLoader(string fileName)
        {
            this.fileName = fileName;
            ResList = new List<GameObj>();
        }

        public void Load()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<GameObj>));
            Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            ResList = (List<GameObj>)xmlFormat.Deserialize(fStream);
            fStream.Close();
        }
    }
}
