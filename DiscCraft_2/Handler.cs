using DiscCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DiscCraft_2
{

    /// <summary>
    /// WORLD : 32x32
    /// no optimisation : 18 874 368 vertex
    /// optimisation 1 :   2 883 584 vertex
    /// optimisation 2 :   2 121 728 vertex 
    /// optimisation 3 :   2 097 152 vertex     (Si le tronçon adjacent est null)
    /// </summary>

    public static class Handler
    {
        /// 
        ///  

        //public static Chunk[,] chunks = new Chunk[1, 1];


        public static Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();


        public static void Init(GraphicsDevice gpu)
        {

            int num = 0;

            //for (int i = 0; i < chunks.GetLength(0); i++)
            //{
            //    for(int j = 0; j < chunks.GetLength(1); j++)
            //    {

            //        chunks[i, j] = new Chunk(new Vector2(i, j), gpu);

            //        Console.Clear();

            //        int num2 = (int)(num * 100 / (chunks.GetLength(0) * chunks.GetLength(1)));

            //        Console.WriteLine("Loading : " + num2 + "%");

            //        Console.Write("[");

            //        for (int l = 0; l < num2 / 10; l++)
            //        {
            //            Console.Write("=");
            //        }

            //        Console.Write(">");

            //        for (int l = 0; l < 10 - (num2 / 10) - 1; l++)
            //        {
            //            Console.Write(" ");
            //        }

            //        Console.Write("]");

            //        num += 1;
            //    }
            //}

            int worldSize = 32;


            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {

                    chunks.Add(new Vector2(i, j), new Chunk(new Vector2(i, j), gpu));

                    Console.Clear();

                    int num2 = (int)(num * 100 / (worldSize * worldSize));

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

            for (int i = 0; i < chunks.Count; i++)
            {
                KeyValuePair<Vector2, Chunk> chunk = chunks.ElementAt(i);
                chunks[chunk.Key].InitVertexBuffer();
            }


            Console.Clear();
            Console.WriteLine("Loading : 100%");
            Console.WriteLine("[==========]");

        }


        public static void Draw(GraphicsDevice gpu, BasicEffect basicEffect, Camera camera)
        {

            //for (int i = 0; i < chunks.GetLength(0); i++)
            //{
            //    for (int j = 0; j < chunks.GetLength(1); j++)
            //    {

            //        //if (Vector2.Distance(chunks[i, j].Position, new Vector2(camera.Position.X, camera.Position.Z)) < 200)
            //        chunks[i, j].Draw(gpu, basicEffect);

            //    }
            //}

            for (int i = 0; i < chunks.Count; i++)
            {

                Vector2 point = (new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z) - new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) * 120 + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z);

                KeyValuePair<Vector2, Chunk> chunk = chunks.ElementAt(i);
                //if (Vector2.Distance(chunk.Key * 16, new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) < 350)
                //{

                    float num = Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Z - chunk.Key.Y * 16)); //Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.Rotation.Y, Main.cameraV2.Rotation.Y) + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Y - chunk.Key.Y * 16));

                    //if (chunk.Key == Vector2.Zero)
                        //Console.WriteLine(num);

                    chunks[chunk.Key].isDrawed = true;
                    if (Vector2.Distance(new Vector2(chunk.Key.X + 0.5f, chunk.Key.Y + 0.5f) * 16, point) > 128)//(Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Z - chunk.Key.Y * 16)) <= 0.025f) //if (Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Y), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Y - chunk.Key.Y * 16)) >= 0.025f) // 0.05
                        chunks[chunk.Key].isDrawed = false;
                    else
                        chunks[chunk.Key].Draw(gpu, basicEffect);


                //}
                   
            }            

        }


        public static Chunk GetChunk(Vector2 chunkPos)
        {
            if(chunks.ContainsKey(chunkPos))
                return chunks[chunkPos];

            return null;
        }

    }

}
