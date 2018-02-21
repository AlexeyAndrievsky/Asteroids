using System;
using System.Drawing;

namespace Asteroids
{
    [Serializable]
    public class GameObj
    {
        public string imgName;
        public string resType;
        public Image img;

        public GameObj()
        {

        }

        public GameObj(string imgName, string resType)
        {
            this.imgName = imgName;
            this.resType = resType;
        }
    }
}
