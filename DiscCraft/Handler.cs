using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscCraft
{
    public static class Handler
    {

        public static Chunk[,] chunks;

        public static void Init()
        {
            chunks = new Chunk[1, 1]; //  6 6
        }


        public static void Update()
        {

        }

        public static void Draw(Camera cameraV2, GraphicsDevice g, BasicEffect basicEffect)
        {
            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int z = 0; z < chunks.GetLength(1); z++)
                {
                    if(chunks[x, z] != null)
                    {
                        //if(x == 0 || x == 2 || x == 4 || x == 6 || x == 8)
                            //if (z == 0 || z == 2 || z == 4 || z == 6 || z == 8)
                                chunks[x, z].Draw(cameraV2, g, basicEffect);
                    }
                }
            }

        }


    }
}
