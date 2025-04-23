using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscCraft
{

    [Serializable]
    public class Block
    {

        public short ID; 


        public Block(int type)
        {
            ID = (short)type;
        }

        public override string ToString()
        {
            return "Block : " + ID;
        }


        public static Block operator +(Block b1, Block b2)
        {
            b1.ID += b2.ID;
            return b1;
        }

    }

}
