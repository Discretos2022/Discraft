using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscCraft
{
    public class Block
    {

        public Vector3 Position;

        public int ID; 


        public Block(Vector3 _position, int type)
        {
            ID = type;
        }

    }
}
