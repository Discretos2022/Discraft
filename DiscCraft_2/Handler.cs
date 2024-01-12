using DiscCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DiscCraft_2
{
    public static class Handler
    {


        public static Chunk[,] chunks = new Chunk[1, 1];


        public static void Init(GraphicsDevice gpu)
        {

            int num = 0;

            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for(int j = 0; j < chunks.GetLength(1); j++)
                {

                    chunks[i, j] = new Chunk(new Vector2(i, j), gpu);

                    Console.Clear();

                    int num2 = (int)(num * 100 / (chunks.GetLength(0) * chunks.GetLength(1)));

                    Console.WriteLine("Loading : " + num2 + "%");

                    Console.Write("[");

                    for (int l = 0; l < num2 / 10; l++)
                    {
                        Console.Write("=");
                    }

                    Console.Write(">");

                    for (int l = 0; l < 10 - (num2 / 10) - 1; l++)
                    {
                        Console.Write(" ");
                    }

                    Console.Write("]");

                    num += 1;
                }
            }

            Console.Clear();
            Console.WriteLine("Loading : 100%");
            Console.WriteLine("[==========]");

        }


        public static void Draw(GraphicsDevice gpu, BasicEffect basicEffect, Camera camera)
        {

            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for (int j = 0; j < chunks.GetLength(1); j++)
                {

                    //if (Vector2.Distance(chunks[i, j].Position, new Vector2(camera.Position.X, camera.Position.Z)) < 200)
                    chunks[i, j].Draw(gpu, basicEffect);

                }
            }

        }



    }
}
