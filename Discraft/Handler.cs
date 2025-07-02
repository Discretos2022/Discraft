using DiscCraft;
using Discraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

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

        public static ConcurrentDictionary<Vect2, ChunkV2> chunks2 = new ConcurrentDictionary<Vect2, ChunkV2>();
        public static List<Vect2> queue2 = new List<Vect2>();

        public static ConcurrentDictionary<(Vect2, int), byte> updateVertexQueue = new ConcurrentDictionary<(Vect2, int), byte>();
        public static ConcurrentDictionary<(Vect2, int), byte> updateBufferQueue = new ConcurrentDictionary<(Vect2, int), byte>();


        public static Thread chunkLoader;
        public static Thread chunkVertexBuilder;
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

            

            int worldSize = 8; // 64


            /*
            Console.Clear();
            Console.WriteLine("Loading : 100%");
            Console.WriteLine("[==========]");*/

            chunkLoader = new Thread(() => LoadChunkTask(gpu));
            chunkLoader.Priority = ThreadPriority.Normal;
            chunkLoader.Start();

            chunkVertexBuilder = new Thread(() => VertexBuilderTask());
            chunkVertexBuilder.Priority = ThreadPriority.Normal;
            chunkVertexBuilder.Start();

            chunkUnloader = new Thread(() => UnloadChunkTask());
            chunkUnloader.Priority = ThreadPriority.Normal;
            chunkUnloader.Start();

            chunkUpdater = new Thread(() => UpdateChunkTask());
            chunkUpdater.Priority = ThreadPriority.Normal;
            chunkUpdater.Start();

            /*chunkUnloader = new Thread(() => UnloadChunk());
            chunkUnloader.Priority = ThreadPriority.Lowest;
            chunkUnloader.Start();*/

            /*chunkUpdater = new Thread(() => UpdateChunk());
            chunkUpdater.Priority = ThreadPriority.Lowest;
            chunkUpdater.Start();*/

        }


        public static void Draw(GraphicsDevice gpu, BasicEffect basicEffect, Camera camera)
        {

            viewLength = 10; // 8


            /*for (int i = 0; i < 10; i++)
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
            }*/


            /*Main.drawedChunk = 0;
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


                if (!camera.IsChunkVisible(chunkBounds) || Vector3.DistanceSquared(chunkWorldPos, Main.cameraV2.Position) > (16 * viewLength) * (16 * viewLength))
                    continue;

                chunkBufferQueue[i].Draw(basicEffect);
                Main.drawedChunk += 1;
            }*/




            for (int i = 0; i < 10; i++)
            {
                if (queue2.Count > 0)
                {
                    ChunkV2 c;
                    chunks2.TryGetValue(queue2[0], out c);
                    c?.BuildBuffer();
                    queue2.RemoveAt(0);
                }
            }


            Main.drawedChunk = 0;
            foreach (var c in chunks2)
            {
                c.Value.Draw(basicEffect, camera);
            }

        }


        public static void Update(GameTime gameTime, Game game)
        {

            player.Update(gameTime, game);

            foreach (var sc in updateBufferQueue)
            {
                chunks2.TryGetValue(sc.Key.Item1, out ChunkV2 c);
                c?.BuildBuffer(sc.Key.Item2);
            }
            updateBufferQueue.Clear();

        }

        public static void UpdateSubChunk((Vect2, int) subChunk)
        {
            updateVertexQueue.TryAdd(subChunk, 0);
        }


        public static void LoadChunkTask(GraphicsDevice gpu)
        {

            Vect2 plr = new Vect2();

            while (true)
            {

                //Console.WriteLine("Thread Chunk loader is operationnal !");

                //Vect2 plr = new Vect2((int)Main.cameraV2.Position.X, (int)Main.cameraV2.Position.Z);
                //plr = plr / 16;

                plr.X = (int)Main.cameraV2.Position.X / 16;
                plr.Y = (int)Main.cameraV2.Position.Z / 16;


                for (int i = plr.X - viewLength; i <= plr.X + viewLength; i++)
                {
                    for (int j = plr.Y - viewLength; j <= plr.Y + viewLength; j++)
                    {

                        //if (!chunks.ContainsKey(new Vect2(i, j)))
                        //{
                            /*try
                            {

                                Chunk c = new Chunk(new Vect2(i, j), gpu);

                                chunks.Add(new Vect2(i, j), c);

                                Console.WriteLine("CHUNK " + i + " : " + j + " Created !");


                            }
                            catch (ArgumentException e) 
                            {
                                Console.WriteLine(e);
                            
                            }*/


                            chunks2.TryAdd(new Vect2(i, j), new ChunkV2(i, j, gpu));
                            //Console.WriteLine("CHUNK " + i + " : " + j + " Created !");


                        //}

                    }
                }

                //Thread.Sleep(1000);


                /*for (int i = plr.X - viewLength; i <= plr.X + viewLength; i++)
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

                        ChunkV2 c;
                        if(chunks2.TryGetValue(new Vect2(i, j), out c))
                        {
                            if (!c.isVertexBuilded)
                            {
                                c.BuildVertex();
                                queue2.Add(new Vect2(i, j));
                            }
                        }

                    }
                }*/


                Thread.Sleep(16); // 1000


            }

        }

        public static void VertexBuilderTask()
        {

            Vect2 plr = new Vect2();

            while (true)
            {

                plr.X = (int)Main.cameraV2.Position.X / 16;
                plr.Y = (int)Main.cameraV2.Position.Z / 16;

                for (int i = plr.X - viewLength; i <= plr.X + viewLength; i++)
                {
                    for (int j = plr.Y - viewLength; j <= plr.Y + viewLength; j++)
                    {


                        bool existAdjacent = chunks2.TryGetValue(new Vect2(i - 1, j), out _) &&
                                        chunks2.TryGetValue(new Vect2(i + 1, j), out _) &&
                                        chunks2.TryGetValue(new Vect2(i, j - 1), out _) &&
                                        chunks2.TryGetValue(new Vect2(i, j + 1), out _);

                        
                        if (existAdjacent && chunks2.TryGetValue(new Vect2(i, j), out ChunkV2 c))
                        {

                            int localI = i;
                            int localJ = j;

                            //Task.Run(() =>
                            //{

                                if (!c.isVertexBuilded)
                                {
                                    c.BuildVertex();
                                    queue2.Add(new Vect2(localI, localJ));
                                    Console.WriteLine("CHUNK " + localI + " : " + localJ + " Vertex Created !");
                                }

                            //});

                            
                        }

                    }
                }

                Thread.Sleep(16);

            }

        }

        public static void UnloadChunkTask()
        {
            Vect2 plr = new Vect2();

            while (true)
            {

                plr.X = (int)Main.cameraV2.Position.X / 16;
                plr.Y = (int)Main.cameraV2.Position.Z / 16;

                foreach (var chunk in chunks2)
                {

                    // Manhattan distance
                    if (Math.Abs(plr.X - chunk.Key.X) > (viewLength + 2) || Math.Abs(plr.Y - chunk.Key.Y) > (viewLength + 2))
                    {
                        chunk.Value?.Dispose();
                        chunks2.TryRemove(chunk);
                    }
                }

                Thread.Sleep(16);

            }
        }

        public static void UpdateChunkTask()
        {

            while (true)
            {

                foreach(var sc in updateVertexQueue)
                {
                    chunks2.TryGetValue(sc.Key.Item1, out ChunkV2 c);
                    c?.BuildVertex(sc.Key.Item2);
                    updateBufferQueue.TryAdd(sc.Key, 0);
                }
                updateVertexQueue.Clear();

                Thread.Sleep(16);

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


        public static List<ChunkBuffer> chunkBufferQueue = new List<ChunkBuffer>();
        public static List<ChunkBuffer> chunkGraphiqueQueue = new List<ChunkBuffer>();


        public static void GenerateChunkVertexTask(Vect2 chunk, int yCoord, GraphicsDevice gpu)
        {

            for (int h = 0; h < 4*4; h++)
            {
                ChunkBuffer b = new ChunkBuffer(gpu, chunk, h);

                b.BuildVertex(chunks[chunk].blocks);

                if (b.hasVertex)
                    chunkGraphiqueQueue.Add(b);

                Main.VERTEX += b.vertexCount;

            }

        }

        public static void UpdateChunkVertexTask(Vect2 chunk, GraphicsDevice gpu)
        {

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
                    
                    Main.VERTEX += b.vertexCount;

                }
                exist = false;
            }


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

            if (X == 16) X = 0;
            if (Z == 16) Z = 0;

            return new Vector3(X, blockCoord.Y, Z);
        }








        public static int GetBlock(Vector3 block)
        {

            ChunkV2 chunk = GetChunkWithBlockCoord2(block);
            Vector3 coordInChunk = GetBlockCoordInChunk2(block);

            if (chunk == null)
                return 0;
            else
                return chunk.GetBlock(coordInChunk);

        }

        public static void SetBlock(Vector3 block, int type)
        {

            ChunkV2 chunk = GetChunkWithBlockCoord2(block);
            Vector3 coordInChunk = GetBlockCoordInChunk2(block);

            if (chunk != null)
                chunk.SetBlock(coordInChunk, type);

        }


        public static ChunkV2 GetChunkWithBlockCoord2(Vector3 blockCoord)
        {
            Vect2 chunkCoord = new Vect2(MathUtils.RoundLower(blockCoord.X / 16), MathUtils.RoundLower(blockCoord.Z / 16));

            if (blockCoord.X < 0 && (int)(blockCoord.X / 16) == (blockCoord.X / 16)) chunkCoord.X += 1;
            if (blockCoord.Z < 0 && (int)(blockCoord.Z / 16) == (blockCoord.Z / 16)) chunkCoord.Y += 1;

            ChunkV2 c;
            if (chunks2.TryGetValue(chunkCoord, out c))
                return c;
            else
                return null;

        }

        public static (Vect2, int) GetSubchunkKeyWithBlockCoord(Vector3 blockCoord)
        {
            Vect2 chunkCoord = new Vect2(MathUtils.RoundLower(blockCoord.X / 16), MathUtils.RoundLower(blockCoord.Z / 16));

            if (blockCoord.X < 0 && (int)(blockCoord.X / 16) == (blockCoord.X / 16)) chunkCoord.X += 1;
            if (blockCoord.Z < 0 && (int)(blockCoord.Z / 16) == (blockCoord.Z / 16)) chunkCoord.Y += 1;

            int subC = MathUtils.RoundLower(blockCoord.Y / 16);
            if (blockCoord.Y < 0 && (int)(blockCoord.Y / 16) == (blockCoord.Y / 16)) subC += 1;

            return (chunkCoord, subC);
        }

        public static Vector3 GetBlockCoordInChunk2(Vector3 blockCoord)
        {
            int X = (int)(((blockCoord.X / 16) - MathUtils.RoundLower(blockCoord.X / 16)) * 16);
            int Z = (int)(((blockCoord.Z / 16) - MathUtils.RoundLower(blockCoord.Z / 16)) * 16);

            if (X == 16) X = 0;
            if (Z == 16) Z = 0;

            return new Vector3(X, blockCoord.Y, Z);
        }



    }

}
