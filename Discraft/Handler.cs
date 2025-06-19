using DiscCraft;
using Discraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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


        public static Dictionary<Vect2, Chunk> chunks = new Dictionary<Vect2, Chunk>();

        public static Thread chunkLoader;
        public static Thread chunkUnloader;
        public static Thread chunkUpdater;

        public static int viewLength = 10;

        public static int MAX_TASK = 100;
        public static int tasknum = 0;

        public static Player player;


        public static void Init(GraphicsDevice gpu)
        {

            int num = 0;

            player = new Player(new Vector3(0, 50, 0));

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

            int worldSize = 8; // 64


            /*for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {

                    //if(i == 0 && j == 0)
                    //{
                        chunks.Add(new Vect2(i, j), ChunkLoader.LoadChunk(new Vect2(i, j), gpu));
                    //}
                    //else
                    //{
                    //    chunks.Add(new Vect2(i, j), new Chunk(new Vect2(i, j), gpu));

                    Console.Clear();

                    int num2 = (int)(num * 100 / (worldSize * worldSize));

                    Console.WriteLine("Loading : " + num2 + "%" + " (Chunk loading...)");

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
                    //}



                }
            }

            Console.Clear();

            num -= 1;

            int num3 = (int)(num * 100 / (worldSize * worldSize));

            Console.WriteLine("Loading : " + num3 + "%" + " (Vertex Generation...)");

            Console.Write("[");

            for (int l = 0; l < num3 / 10; l++)
            {
                Console.Write("=");
            }

            Console.Write(">");

            for (int l = 0; l < 10 - (num3 / 10) - 1; l++)
            {
                Console.Write(" ");
            }

            Console.Write("]");

            for (int i = 0; i < chunks.Count; i++)
            {
                KeyValuePair<Vect2, Chunk> chunk = chunks.ElementAt(i);
                chunks[chunk.Key].InitVertexBuffer();
            }


            Console.Clear();
            Console.WriteLine("Loading : 100%");
            Console.WriteLine("[==========]");*/

            chunkLoader = new Thread(() => CheckChunk(gpu));
            chunkLoader.Priority = ThreadPriority.Highest;
            chunkLoader.Start();

            /*chunkUnloader = new Thread(() => UnloadChunk());
            chunkUnloader.Priority = ThreadPriority.Lowest;
            chunkUnloader.Start();*/

            /*chunkUpdater = new Thread(() => UpdateChunk());
            chunkUpdater.Priority = ThreadPriority.Lowest;
            chunkUpdater.Start();*/

        }


        public static void Draw(GraphicsDevice gpu, BasicEffect basicEffect, Camera camera)
        {

            viewLength = 16; // 8

            //for (int i = 0; i < chunks.GetLength(0); i++)
            //{
            //    for (int j = 0; j < chunks.GetLength(1); j++)
            //    {

            //        //if (Vector2.Distance(chunks[i, j].Position, new Vector2(camera.Position.X, camera.Position.Z)) < 200)
            //        chunks[i, j].Draw(gpu, basicEffect);

            //    }
            //}


            for (int i = 0; i < 10; i++)
            {
                if (chunkGraphiqueQueue.Count > 0)
                {
                    if (chunkGraphiqueQueue[0].hasVertex)
                    {
                        chunkGraphiqueQueue[0].BuildBuffer();
                        chunkBufferQueue.Add(chunkGraphiqueQueue[0]);
                        chunkGraphiqueQueue.RemoveAt(0);
                    }

                }
            }



            Main.drawedChunk = 0;
            int max = chunkBufferQueue.Count;
            for (int i = 0; i < max; i++)
            {

                ChunkBuffer chunk = chunkBufferQueue[i];

                if (!chunk.hasVertex)
                    continue;

                float CHUNK_SIZE = 16f;

                Vector3 chunkWorldPos = new Vector3(
                    chunk.Position.X * CHUNK_SIZE,
                    chunk.yCoord * CHUNK_SIZE,
                    chunk.Position.Y * CHUNK_SIZE
                );

                
                BoundingBox chunkBounds = new BoundingBox(
                    chunkWorldPos,
                    chunkWorldPos + new Vector3(CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE)
                );


                if (!camera.IsChunkVisible(chunkBounds) || Vector3.DistanceSquared(chunkWorldPos, Main.cameraV2.Position) > (16 * 24)*(16 * 24))
                    continue;

                chunkBufferQueue[i].Draw(basicEffect);
                Main.drawedChunk += 1;
            }




            /*lock (chunks)
            {
                for (int i = 0; i < chunks.Count; i++)
                {

                    ///Vector2 point = (new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z) - new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) * 120 + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z);

                    ///KeyValuePair<Vect2, Chunk> chunk = chunks.ElementAt(i);
                    //if (Vector2.Distance(chunk.Key * 16, new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) < 350)
                    //{

                    //float num = Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Z - chunk.Key.Y * 16)); //Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.Rotation.Y, Main.cameraV2.Rotation.Y) + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Y - chunk.Key.Y * 16));

                    //if (chunk.Key == Vector2.Zero)
                    //Console.WriteLine(num);

                    ///chunks[chunk.Key].isDrawed = true;
                    ///if (Vector2.Distance(new Vector2(chunk.Key.X + 0.5f, chunk.Key.Y + 0.5f) * 16, point) > 128)//(Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Z - chunk.Key.Y * 16)) <= 0.025f) //if (Util.GetAngleBetweenVector(new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Y), new Vector2(Main.cameraV2.Position.X - chunk.Key.X * 16, Main.cameraV2.Position.Y - chunk.Key.Y * 16)) >= 0.025f) // 0.05
                    ///chunks[chunk.Key].isDrawed = false;
                    ///else
                    ///chunks[chunk.Key].Draw(gpu, basicEffect);
                    ///
                    try
                    {
                        //if(chunks.ElementAt(i).Key == new Vect2(0, 0))
                        chunks.ElementAt(i).Value.Draw(gpu, basicEffect);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }



                    //}

                }
            }*/

                        

        }


        public static void Update(GameTime gameTime, Game game)
        {

            player.Update(gameTime, game);

            //CheckAsync(gpu);

            //for (int i = 0; i < chunks.Count; i++)
            //{
            //    KeyValuePair<Vect2, Chunk> chunk = chunks.ElementAt(i);

            //    if (chunk.Key.X > 6)
            //        chunks.Remove(chunk.Key);
            //    else if (chunk.Key.X < -6)
            //        chunks.Remove(chunk.Key);

            //    else if (chunk.Key.Y > 6)
            //        chunks.Remove(chunk.Key);
            //    else if (chunk.Key.Y < -6)
            //        chunks.Remove(chunk.Key);

            //}

        }

        public static async void CheckAsync(GraphicsDevice gpu)
        {
            await Task.Run(() => CheckChunk(gpu));
        }


        public static void CheckChunk(GraphicsDevice gpu)
        {

            while (true)
            {

                //Console.WriteLine("Thread Chunk loader is operationnal !");

                Vect2 plr = new Vect2((int)Main.cameraV2.Position.X, (int)Main.cameraV2.Position.Z);
                plr = plr / 16;


                for (int i = plr.X - viewLength; i <= plr.X + viewLength; i++)
                {
                    for (int j = plr.Y - viewLength; j <= plr.Y + viewLength; j++)
                    {

                        if (!chunks.ContainsKey(new Vect2(i, j)))
                        {
                            try
                            {

                                Chunk c = new Chunk(new Vect2(i, j), gpu);

                                chunks.Add(new Vect2(i, j), c);
                                //chunks[new Vect2(i, j)].InitVertexBuffer();

                                chunkStack.Add(new Vect2(i, j));

                                //for (int h = 0; h < 4; h++)
                                    //GenerateChunkVertexTask(new Vect2(i, j), h, gpu);

                                //for (int x = i - 1; x <= i + 1; x++)
                                //{
                                //    for (int y = j - 1; y <= j + 1; y++)
                                //    {
                                //        if (chunks.ContainsKey(new Vect2(x, y)))
                                //            chunks[new Vect2(x, y)].InitVertexBuffer();
                                //    }
                                //}

                                Console.WriteLine("CHUNK " + i + " : " + j + " Created !");


                                //Thread generate = new Thread(() => GenerateChunkAsync(gpu, i, j));
                                //generate.Start();


                            }
                            catch (ArgumentException e) 
                            {
                                Console.WriteLine(e);
                            
                            }

                            /*if (chunks.ContainsKey(new Vect2(i - 1, j)))
                                chunkStack.Add(new Vect2(i - 1, j));
                            if (chunks.ContainsKey(new Vect2(i + 1, j)))
                                chunkStack.Add(new Vect2(i + 1, j));
                            if (chunks.ContainsKey(new Vect2(i, j - 1)))
                                chunkStack.Add(new Vect2(i, j - 1));
                            if (chunks.ContainsKey(new Vect2(i, j + 1)))
                                chunkStack.Add(new Vect2(i, j + 1));*/

                        }

                    }
                }

                //Thread.Sleep(1000);


                for (int i = plr.X - viewLength; i <= plr.X + viewLength; i++)
                {
                    for (int j = plr.Y - viewLength; j <= plr.Y + viewLength; j++)
                    {

                        if (chunks.ContainsKey(new Vect2(i, j)))
                        {
                            try
                            {
                                
                                //for (int h = 0; h < 4; h++)
                                //if (tasknum <= MAX_TASK)
                                    if (chunks[new Vect2(i, j)].loaded[1] == 0)
                                    {
                                        chunks[new Vect2(i, j)].loaded[1] = 1;
                                        //Thread.Sleep(20);
                                        GenerateChunkVertexTask(new Vect2(i, j), 0, gpu);
                                    }

                            }
                            catch (ArgumentException e) {
                                Console.WriteLine(e);
                            }

                        }

                    }
                }


                Thread.Sleep(10000); // 1000


            }

        }

        public static void GenerateChunkAsync(GraphicsDevice gpu, int i, int j)
        {
            if(!chunks.ContainsKey(new Vect2(i, j)))
            {

                Chunk c = new Chunk(new Vect2(i, j), gpu);

                if (!chunks.ContainsKey(new Vect2(i, j)))
                    chunks.Add(new Vect2(i, j), c);
                //chunks[new Vect2(i, j)].InitVertexBuffer();

                if(chunkStack.Contains(new Vect2(i, j)))
                    chunkStack.Add(new Vect2(i, j));


                //for (int x = i - 1; x <= i + 1; x++)
                //{
                //    for (int y = j - 1; y <= j + 1; y++)
                //    {
                //        if (chunks.ContainsKey(new Vect2(x, y)))
                //            chunks[new Vect2(x, y)].InitVertexBuffer();
                //    }
                //}

                Console.WriteLine("CHUNK " + i + " : " + j + " Created !");
            }
            
        }


        public static Chunk GetChunk(Vect2 chunkPos)
        {
            if(chunks.ContainsKey(chunkPos))
                return chunks[chunkPos];

            return null;
        }

        public static Chunk GetChunk(Vector2 _chunkPos)
        {

            Vect2 chunk = new Vect2((int)_chunkPos.X, (int)_chunkPos.Y);

            if (chunks.ContainsKey(chunk))
                return chunks[chunk];

            return null;
        }




        public static void UnloadChunk()
        {

            while (true)
            {

                Console.WriteLine("Thread Chunk unloader is operationnal !");

                Vect2 plr = new Vect2((int)Main.cameraV2.Position.X, (int)Main.cameraV2.Position.Z);
                plr = plr / 16;


                for (int i = plr.X - 20; i <= plr.X + 20; i++)
                {
                    for (int j = plr.Y - 20; j <= plr.Y + 20; j++)
                    {

                        if(i < plr.X - viewLength - 1 || i > plr.X + viewLength + 1 || j > plr.Y + viewLength + 1 || j < plr.Y - viewLength - 1)
                            if (chunks.ContainsKey(new Vect2(i, j)))
                            {

                                Main.VERTEX -= chunks[new Vect2(i, j)].VERTEX;
                                Main.TRIANGLES -= chunks[new Vect2(i, j)].TRIANGLES;

                                ChunkLoader.SaveChunk(new Vect2(i, j));
                                chunks.Remove(new Vect2(i, j));

                            }

                    }
                }

                Thread.Sleep(5000);


            }

        }


        public static List<Vect2> chunkStack = new List<Vect2>();

        public static void UpdateChunk()
        {

            while (true)
            {

                Vect2 playerPos = new Vect2((int)Main.cameraV2.Position.X, (int)Main.cameraV2.Position.Z);
                //chunkStack.OrderBy((x) => (-Vect2.Distance(x * 16, playerPos)));

                for (int i = 0; i < chunkStack.Count; i++)
                {
                    if (chunks.ContainsKey(chunkStack[i]))
                        //if (chunks[chunkStack[i]].isDrawed == false)
                        if (chunks.ContainsKey(new Vect2(chunkStack[i].X + 1, chunkStack[i].Y)))
                            if (chunks.ContainsKey(new Vect2(chunkStack[i].X - 1, chunkStack[i].Y)))
                                if (chunks.ContainsKey(new Vect2(chunkStack[i].X, chunkStack[i].Y + 1)))
                                    if (chunks.ContainsKey(new Vect2(chunkStack[i].X, chunkStack[i].Y - 1)))
                                    {
                                        Main.VERTEX -= chunks[chunkStack[i]].VERTEX;
                                        Main.TRIANGLES -= chunks[chunkStack[i]].TRIANGLES;
                                        chunks[chunkStack[i]].InitVertexBuffer();
                                        Main.VERTEX += chunks[chunkStack[i]].VERTEX;
                                        Main.TRIANGLES += chunks[chunkStack[i]].TRIANGLES;
                                        chunkStack.Remove(chunkStack[i]);
                                    }

                }
                

                //Thread.Sleep(100);

            }

        }


        public static List<ChunkBuffer> chunkBufferQueue = new List<ChunkBuffer>();
        public static List<ChunkBuffer> chunkGraphiqueQueue = new List<ChunkBuffer>();


        public static void GenerateChunkVertexTask(Vect2 chunk, int yCoord, GraphicsDevice gpu)
        {

            //Task task = new Task(() => {

                for (int h = 0; h < 4*4; h++)
                {
                    ChunkBuffer b = new ChunkBuffer(gpu, chunk, h);

                    b.BuildVertex(chunks[chunk].blocks);
                //if (b.hasVertex)
                //b.BuildBuffer();

                if (b.hasVertex)
                    chunkGraphiqueQueue.Add(b);
                    //chunkBufferQueue.Add(b);
                //else

                //Console.WriteLine("--> " + chunk.X + ";" + chunk.Y + " | " + b.vertexCount);
                Main.VERTEX += b.vertexCount;

                tasknum -= 1;

                }

            //});

            tasknum += 1;
            //task.Start();

            //Console.WriteLine(task.Exception);


        }

        public static void UpdateChunkVertexTask(Vect2 chunk, GraphicsDevice gpu)
        {

            //Task task = new Task(() => {

            bool exist = false;
            for (int h = 0; h < 4; h++)
            {
                ChunkBuffer b = new ChunkBuffer(gpu, chunk, h);

                b.BuildVertex(chunks[chunk].blocks);
                if (b.hasVertex)
                {
                    b.BuildBuffer();

                    lock (chunkBufferQueue)
                    {
                        for (int i = 0; i < chunkBufferQueue.Count; i++)
                        {
                            if (chunkBufferQueue[i].Position == chunk && chunkBufferQueue[i].yCoord == h)
                            {
                                Main.VERTEX -= chunkBufferQueue[i].vertexCount;
                                chunkBufferQueue[i] = b;
                                Main.VERTEX += b.vertexCount;
                                exist = true;
                            }
                        }

                        if (!exist)
                        {
                            Main.VERTEX += b.vertexCount;
                            chunkBufferQueue.Add(b);
                        }

                    }
                    
                    //Console.WriteLine("--> " + chunk.X + ";" + chunk.Y + " | " + b.vertexCount);
                    Main.VERTEX += b.vertexCount;

                    tasknum -= 1;

                }
                exist = false;
            }

            //});

            tasknum += 1;
            //task.Start();

            //Console.WriteLine(task.Exception);


        }





        public static Chunk GetChunkWithBlockCoord(Vector3 blockCoord)
        {
            Vect2 chunkCoord = new Vect2(MathUtils.RoundLower(blockCoord.X / 16), MathUtils.RoundLower(blockCoord.Z / 16));

            if (blockCoord.X < 0 && (int)(blockCoord.X / 16) == (blockCoord.X / 16)) chunkCoord.X += 1;
            if (blockCoord.Z < 0 && (int)(blockCoord.Z / 16) == (blockCoord.Z / 16)) chunkCoord.Y += 1;

            lock (chunks)
            {
                if (chunks.ContainsKey(chunkCoord) && blockCoord.Y >= 0 && blockCoord.Y < 64)
                    return chunks[chunkCoord];
                else
                    return null;
            }
            
        }

        public static Vector3 GetBlockCoordInChunk(Vector3 blockCoord)
        {
            int X = (int)(((blockCoord.X / 16) - MathUtils.RoundLower(blockCoord.X / 16)) * 16);
            int Z = (int)(((blockCoord.Z / 16) - MathUtils.RoundLower(blockCoord.Z / 16)) * 16);

            //if (blockCoord.X < 0) X -= 1;
            //if (blockCoord.X < 0) X -= 1;

            if (X == 16) X = 0;
            if (Z == 16) Z = 0;

            return new Vector3(X, blockCoord.Y, Z);
        }



    }

}
