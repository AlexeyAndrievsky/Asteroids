using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    [Serializable]
    class GameObj
    {
        public string imgName;
        public string resType;

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
