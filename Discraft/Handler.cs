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

    public static class Handler
    {

        public static ConcurrentDictionary<Vect2, ChunkV2> chunks2 = new ConcurrentDictionary<Vect2, ChunkV2>();
        public static ConcurrentQueue<Vect2> queueToBuffer = new ConcurrentQueue<Vect2>();
        public static ConcurrentQueue<Vect2> queueToConstructVertex = new ConcurrentQueue<Vect2>();

        public static ConcurrentDictionary<(Vect2, int), byte> updateVertexQueue = new ConcurrentDictionary<(Vect2, int), byte>();
        public static ConcurrentDictionary<(Vect2, int), byte> updateBufferQueue = new ConcurrentDictionary<(Vect2, int), byte>();

        public static Thread chunkLoader;
        public static Thread chunkVertexBuilder;
        public static Thread chunkUnloader;
        public static Thread chunkUpdater;

        public static int viewLength = 10;

        public static Player player;


        public static void Init(GraphicsDevice gpu)
        {

            player = new Player(new Vector3(0, 200, 0));

            

            int worldSize = 16; // 64

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

        }


        public static void Draw(GraphicsDevice gpu, BasicEffect basicEffect, Camera camera)
        {

            viewLength = 16; // 8

            for (int i = 0; i < 10; i++)
            {
                if (queueToBuffer.TryDequeue(out Vect2 v))
                {
                    ChunkV2 c;
                    chunks2.TryGetValue(v, out c);
                    c?.BuildBuffer();
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


        static int taskNum = 0;

        public static void LoadChunkTask(GraphicsDevice gpu)
        {

            Vect2 plr = new Vect2();

            while (true)
            {

                plr.X = (int)Main.cameraV2.Position.X / 16;
                plr.Y = (int)Main.cameraV2.Position.Z / 16;



                for (int r = 0; r <= viewLength; r++)
                {

                    for (int i = plr.X - r; i <= plr.X + r; i++)
                    {
                        int j = plr.Y + r;
                        if (chunks2.TryAdd(new Vect2(i, j), new ChunkV2(i, j, gpu)))
                            queueToConstructVertex.Enqueue(new Vect2(i, j));
                    }

                    for (int i = plr.X - r; i <= plr.X + r; i++)
                    {
                        int j = plr.Y - r;
                        if (chunks2.TryAdd(new Vect2(i, j), new ChunkV2(i, j, gpu)))
                            queueToConstructVertex.Enqueue(new Vect2(i, j));
                    }


                    for (int j = plr.Y - r + 1; j <= plr.Y + r - 1; j++)
                    {
                        int i = plr.X + r;
                        if (chunks2.TryAdd(new Vect2(i, j), new ChunkV2(i, j, gpu)))
                            queueToConstructVertex.Enqueue(new Vect2(i, j));
                    }

                    for (int j = plr.Y - r + 1; j <= plr.Y + r - 1; j++)
                    {
                        int i = plr.X - r;
                        if (chunks2.TryAdd(new Vect2(i, j), new ChunkV2(i, j, gpu)))
                            queueToConstructVertex.Enqueue(new Vect2(i, j));
                    }

                }

                Thread.Sleep(16);

            }

        }


        public static void VertexBuilderTask()
        {

            Vect2 plr = new Vect2();
            int iteration = 0;
            List<Vect2> fails = new List<Vect2>();

            while (true)
            {

                plr.X = (int)Main.cameraV2.Position.X / 16;
                plr.Y = (int)Main.cameraV2.Position.Z / 16;
                iteration = 0;
                fails.Clear();


                while (queueToConstructVertex.TryDequeue(out Vect2 v))
                {
                    int i = v.X;
                    int j = v.Y;

                    bool existAdjacent = chunks2.TryGetValue(new Vect2(i - 1, j), out _) &&
                                        chunks2.TryGetValue(new Vect2(i + 1, j), out _) &&
                                        chunks2.TryGetValue(new Vect2(i, j - 1), out _) &&
                                        chunks2.TryGetValue(new Vect2(i, j + 1), out _);

                    if (!existAdjacent && chunks2.TryGetValue(v, out _))
                        fails.Add(v);

                    if (existAdjacent && chunks2.TryGetValue(new Vect2(i, j), out ChunkV2 c))
                    {

                        if (taskNum < 10)
                        {
                            taskNum += 1;
                            Task.Run(() => ProcessChunkVertex(i, j, c));
                        }
                        else
                            fails.Add(v);

                    }

                    iteration += 1;

                }

                foreach (var f in fails)
                {
                    queueToConstructVertex.Enqueue(f);
                }

                Thread.Sleep(16);

            }

        }

        public static void ProcessChunkVertex(int i, int j, ChunkV2 c)
        {

            if (!c.isVertexBuilded)
            {
                Console.WriteLine("CHUNK " + i + " : " + j + " Vertex Started !");
                c.BuildVertex();
                queueToBuffer.Enqueue(new Vect2(i, j));
            }
            
            taskNum -= 1;
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
       

        public static int GetBlock(Vector3 block)
        {

            ChunkV2 chunk = GetChunkWithBlockCoord(block);
            Vector3 coordInChunk = GetBlockCoordInChunk(block);

            if (chunk == null)
                return 0;
            else
                return chunk.GetBlock(coordInChunk);

        }

        public static void SetBlock(Vector3 block, int type)
        {

            ChunkV2 chunk = GetChunkWithBlockCoord(block);
            Vector3 coordInChunk = GetBlockCoordInChunk(block);

            if (chunk != null)
                chunk.SetBlock(coordInChunk, type);

        }


        public static ChunkV2 GetChunkWithBlockCoord(Vector3 blockCoord)
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

        public static Vector3 GetBlockCoordInChunk(Vector3 blockCoord)
        {
            int X = (int)(((blockCoord.X / 16) - MathUtils.RoundLower(blockCoord.X / 16)) * 16);
            int Z = (int)(((blockCoord.Z / 16) - MathUtils.RoundLower(blockCoord.Z / 16)) * 16);

            if (X == 16) X = 0;
            if (Z == 16) Z = 0;

            return new Vector3(X, blockCoord.Y, Z);
        }

    }

}
