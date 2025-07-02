using DiscCraft;
using DiscCraft_2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discraft
{

    /*
     *
     * 32x16x32 -> 5 000 000 vertex -> 700 Mo
     * 
    */


    public class ChunkV2 : IDisposable
    {

        public static byte SIZE = 16;
        public static short MIN = -1; // -1
        public static short MAX = 14; // 14

        private GraphicsDevice gpu;
        private bool disposedValue;


        /// Index Position
        public int X;
        public int Z;

        public Dictionary<short, SubChunk> subChunks;

        public bool isVertexBuilded = false;
        

        public ChunkV2(int _x, int _z, GraphicsDevice _gpu)
        {

            gpu = _gpu;
            X = _x;
            Z = _z;

            subChunks = new Dictionary<short, SubChunk>();

            InitChunk();

        }

        public void InitChunk()
        {

            for (int i = MIN; i < MAX; i++)
            {
                SubChunk sb = new SubChunk(X, i, Z, gpu);
                sb.InitSubChunk();

                subChunks.Add((short)i, sb);
            }

        }

        public void BuildVertex()
        {
            for (int i = MIN; i < MAX; i++)
            {
                subChunks[(short)i].BuildVertex();
            }
            isVertexBuilded = true;
        }

        public void BuildBuffer()
        {
            for (int i = MIN; i < MAX; i++)
            {
                subChunks[(short)i].BuildBuffer();
            }
        }

        public void BuildVertex(int subchunk)
        {
            if(subChunks.TryGetValue((short)subchunk, out SubChunk sc))
                sc.BuildVertex();
        }

        public void BuildBuffer(int subchunk)
        {
            if (subChunks.TryGetValue((short)subchunk, out SubChunk sc))
                sc.BuildBuffer();
        }

        public void Draw(BasicEffect basicEffect, Camera camera)
        {

            Vector3 chunkWorldPos = new Vector3(
                X * 16,
                MIN * 16,
                Z * 16
            );

            BoundingBox chunkBounds = new BoundingBox(
                chunkWorldPos,
                chunkWorldPos + new Vector3(16, (MAX - MIN) * 16, 16)
            );

            if (!camera.IsChunkVisible(chunkBounds) ) // || Vector3.DistanceSquared(chunkWorldPos, Main.cameraV2.Position) > (16 * Handler.viewLength) * (16 * Handler.viewLength))
                return;


            for (int i = MIN; i < MAX; i++)
            {

                float CHUNK_SIZE = 16f;
                Vector3 subChunkWorldPos = new Vector3(
                    X * CHUNK_SIZE,
                    i * CHUNK_SIZE,
                    Z * CHUNK_SIZE
                );


                BoundingBox subChunkBounds = new BoundingBox(
                    subChunkWorldPos,
                    subChunkWorldPos + new Vector3(CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE)
                );


                if (!camera.IsChunkVisible(subChunkBounds) ) //|| Vector3.DistanceSquared(subChunkWorldPos, Main.cameraV2.Position) > (16 * Handler.viewLength) * (16 * Handler.viewLength))
                    continue;

                if(!subChunks[(short)i].disposedValue)
                    subChunks[(short)i].Draw(basicEffect);

            }
        }

        public int GetBlock(Vector3 coord)
        {

            int subC = MathUtils.RoundLower(coord.Y / 16);
            if (coord.Y < 0 && (int)(coord.Y / 16) == (coord.Y / 16)) subC += 1;

            int Y = (int)(((coord.Y / 16) - MathUtils.RoundLower(coord.Y / 16)) * 16);
            if (Y == 16) Y = 0;

            if (subChunks.ContainsKey((short)subC))
                return subChunks[(short)subC].GetBlock((int)coord.X, (int)Y, (int)coord.Z);
            else
                return 0;

        }

        public void SetBlock(Vector3 coord, int type)
        {

            int subC = MathUtils.RoundLower(coord.Y / 16);
            if (coord.Y < 0 && (int)(coord.Y / 16) == (coord.Y / 16)) subC += 1;

            int Y = (int)(((coord.Y / 16) - MathUtils.RoundLower(coord.Y / 16)) * 16);
            if (Y == 16) Y = 0;

            if (subChunks.ContainsKey((short)subC))
                subChunks[(short)subC].SetBlock((int)coord.X, (int)Y, (int)coord.Z, type);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var chunk in subChunks)
                    {
                        chunk.Value?.Dispose();
                    }
                    subChunks.Clear();
                    subChunks = null;
                }

                gpu = null;

                disposedValue = true;
            }
        }

        ~ChunkV2()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }



    public class SubChunk : IDisposable
    {

        private GraphicsDevice gpu;
        private DynamicVertexBuffer buffer;
        private DynamicIndexBuffer index;

        /// Index Position
        public int X;
        public int Y;
        public int Z;

        private Texture2D tex = Main.BlockSheet;

        public List<VertexPositionNormalTexture> vertex;
        public List<ushort> indices;


        private short[,,] blocks;

        private int uniformValue = 0;
        private bool isUniform = false;
        public bool hasVertex = true;
        private bool isBufferSet = false;

        public bool disposedValue;

        public SubChunk(int _x, int _y, int _z, GraphicsDevice _gpu)
        {

            X = _x;
            Y = _y;
            Z = _z;
            gpu = _gpu;

        }

        public void InitSubChunk()
        {

            blocks = new short[16, 16, 16];

            int lastId = -64;
            bool uni = true;

            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        int id = WorldGenerator.GetBlockID(new Vector3(x + X*16, y + Y*16, z + Z*16));

                        if (id >= 0)
                        {

                            blocks[x, y, z] = (short)id;

                            if (lastId == -64)
                                lastId = id;
                            else if (id != lastId)
                                uni = false;

                        }
                    }
                }
            }

            /*if(uni == true)
            {
                uniformValue = lastId == -64 ? 0 : lastId;
                isUniform = true;
                blocks = null;
            }*/

            vertex = new List<VertexPositionNormalTexture>();
            indices = new List<ushort>();

        }

        public int GetBlock(int x, int y, int z)
        {
            if (isUniform == true)
                return uniformValue;
            else
                return blocks[x, y, z];
        }

        public void SetBlock(int x, int y, int z, int type)
        {
            if (isUniform)
            {
                blocks = new short[16, 16, 16];
                for (int i = 0; i < blocks.GetLength(0); i++)
                    for (int j = 0; j < blocks.GetLength(1); j++)
                        for (int k = 0; k < blocks.GetLength(2); k++)
                            blocks[i, j, k] = 0;

                blocks[x, y, z] = (short)type;
                isUniform = false;

            }
            else
                blocks[x, y, z] = (short)type;
        }


        public void BuildVertex()
        {

            if (isUniform && uniformValue != 0)
            {
                for (int x = 0; x < 16; x++)
                    for (int y = 0; y < 16; y++)
                        for (int z = 0; z < 16; z++)
                            AddBlock(new Vector3(x + X * 16, y + Y * 16, z + Z * 16), new Vector3(x, y, z), blocks, uniformValue);

            }
            else if(!isUniform)
            {
                for (int x = 0; x < blocks.GetLength(0); x++)
                    for (int y = 0; y < blocks.GetLength(1); y++)
                        for (int z = 0; z < blocks.GetLength(2); z++)
                            if (blocks[x, y, z] != 0)
                                AddBlock(new Vector3(x + X * 16, y + Y * 16, z + Z * 16), new Vector3(x, y, z), blocks, blocks[x, y, z]);

            }

            hasVertex = (vertex.Count > 0);

        }

        public void BuildBuffer()
        {
            if (hasVertex && vertex.Count > 0)
            {
                buffer = new DynamicVertexBuffer(gpu, typeof(VertexPositionNormalTexture), vertex.Count, BufferUsage.WriteOnly);
                index = new DynamicIndexBuffer(gpu, typeof(ushort), indices.Count, BufferUsage.WriteOnly);

                buffer.SetData(vertex.ToArray());
                index.SetData(indices.ToArray());

                vertex.Clear();
                indices.Clear();

                isBufferSet = true;
            }
        }

        public void Draw(BasicEffect basicEffect)
        {
            if (hasVertex && isBufferSet && buffer.VertexCount > 0)
            {
                gpu.SetVertexBuffer(buffer);
                gpu.Indices = index;

                #region //[Reminder_To_Set_Lighting_Draw_Params_Later for custom lighting class and effect]
                // TO DO (later): 
                // if (DrawDepth)        light.SetDepthParams(ob.transform);           // for drawing to a depth shader
                // else if (DrawShadows) light.SetShadowParams(ob.transform, cam);     // for drawing shadows (using depth shader results) 
                // else                  light.SetDrawParams(ob.transform,cam,ob.tex); // regular drawing
                #endregion
                // SET SHADER PARAMETERS:

                basicEffect.TextureEnabled = true;
                basicEffect.Texture = tex;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gpu.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, buffer.VertexCount / 2);
                }
                Main.drawedChunk += 1;
            }
        }








        public void AddBlock(Vector3 _position, Vector3 _positionInChunk, short[,,] blocks, int type)
        {
            CreateTexturedCube(_position, blocks, gpu, new Vector3(1, 1, 1), type);
        }

        public void CreateTexturedCube(Vector3 Pos, short[,,] blocks, GraphicsDevice gpu, Vector3 size, int type)
        {

            float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = size.X / 2, hl = size.Z / 2, hh = size.Y / 2; // uv's, half-width, half-length, half-height
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[0]);
            //float t = Pos.Y - hh, b = Pos.Y + hh, l = Pos.X - hw, r = Pos.X + hw, n = Pos.Z - hl, f = Pos.Z + hl;  // y-coord, left, right, near, far

            float t = Pos.Y, b = Pos.Y + 1, l = Pos.X, r = Pos.X + 1, n = Pos.Z, f = Pos.Z + 1;  // y-coord, left, right, near, far

            bool front = false, back = false, left = false, right = false, up = false, down = false;
            GetAdjacentBlock(Pos, ref front, ref back, ref left, ref right, ref up, ref down);

            Vector3 norm = Vector3.Up;

            /// Down
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[0]);
            if (!down) { norm = Vector3.Down; AddVertex(l, t, f, norm, u1, v1); AddVertex(r, t, f, norm, u2, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2); } // (left,y,far),(right,y,far),(right,y,near) [clockwise]

            /// Left
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[1]);
            if (!left) { norm = Vector3.Right; AddVertex(r, b, f, norm, u1, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(r, b, n, norm, u2, v1); AddVertex(r, t, f, norm, u1, v2); }

            /// Up /// Vector3.Down
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[2]);
            if (!up) { norm = Vector3.Up; AddVertex(r, b, f, norm, u1, v1); AddVertex(l, b, n, norm, u2, v2); AddVertex(l, b, f, norm, u2, v1); AddVertex(r, b, n, norm, u1, v2); }

            /// Right
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[3]);
            if (!right) { norm = Vector3.Left; AddVertex(l, b, n, norm, u1, v1); AddVertex(l, b, f, norm, u2, v1); AddVertex(l, t, f, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2); }

            /// Front
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[4]);
            if (!front) { norm = Vector3.Forward; AddVertex(l, t, n, norm, u2, v2); AddVertex(r, t, n, norm, u1, v2); AddVertex(r, b, n, norm, u1, v1); AddVertex(l, b, n, norm, u2, v1); }

            /// Back
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[5]);
            if (!back) { norm = Vector3.Backward; AddVertex(r, t, f, norm, u2, v2); AddVertex(l, t, f, norm, u1, v2); AddVertex(l, b, f, norm, u1, v1); AddVertex(r, b, f, norm, u2, v1); }


            ushort offset = 0;
            ushort offset2 = 0;

            if (down) offset2 += 4;
            if (left) offset2 += 4;
            if (up) offset2 += 4;
            if (right) offset2 += 4;
            if (front) offset2 += 4;
            if (back) offset2 += 4;

            /// DOWN
            if (!down) { AddTriangle(0, 1, 2, offset2); AddTriangle(2, 3, 0, offset2); }  // clockwise order
            if (down) offset += 4;

            /// LEFT
            if (!left) { AddTriangle((ushort)(5 - offset), (ushort)(7 - offset), (ushort)(6 - offset), offset2); AddTriangle((ushort)(6 - offset), (ushort)(7 - offset), (ushort)(4 - offset), offset2); }  // clockwise order
            if (left) offset += 4;

            /// UP
            if (!up) { AddTriangle((ushort)(11 - offset), (ushort)(8 - offset), (ushort)(10 - offset), offset2); AddTriangle((ushort)(10 - offset), (ushort)(9 - offset), (ushort)(11 - offset), offset2); }  // clockwise order
            if (up) offset += 4;

            /// RIGHT
            if (!right) { AddTriangle((ushort)(12 - offset), (ushort)(13 - offset), (ushort)(14 - offset), offset2); AddTriangle((ushort)(14 - offset), (ushort)(15 - offset), (ushort)(12 - offset), offset2); }  // clockwise order
            if (right) offset += 4;

            /// FRONT
            if (!front) { AddTriangle((ushort)(16 - offset), (ushort)(17 - offset), (ushort)(18 - offset), offset2); AddTriangle((ushort)(18 - offset), (ushort)(19 - offset), (ushort)(16 - offset), offset2); }  // clockwise order
            if (front) offset += 4;

            /// BACK
            if (!back) { AddTriangle((ushort)(20 - offset), (ushort)(21 - offset), (ushort)(22 - offset), offset2); AddTriangle((ushort)(22 - offset), (ushort)(23 - offset), (ushort)(20 - offset), offset2); }  // clockwise order


        }


        public void AddVertex(float x, float y, float z, Vector3 norm, float u, float v)
        {
            vertex.Add(new VertexPositionNormalTexture(new Vector3(x, y, z), norm, new Vector2(u, v)));
            Main.VERTEX += 1;
        }

        public void AddTriangle(ushort a, ushort b, ushort c, ushort _offset)
        {
            ushort offset = (ushort)((ushort)vertex.Count - 24 + _offset);
            a += offset; b += offset; c += offset;

            indices.Add(a);
            indices.Add(b);
            indices.Add(c);

        }


        protected void GetUVCoords(ref float u1, ref float v1, ref float u2, ref float v2, Rectangle source_rect)
        {
            u1 = source_rect.X / (float)tex.Width;                                                // get the uv coords (texture map coords)
            v1 = source_rect.Y / (float)tex.Height;
            u2 = (source_rect.X + source_rect.Width) / (float)tex.Width;
            v2 = (source_rect.Y + source_rect.Height) / (float)tex.Height;
        }



        public Rectangle[] GetSourceRect(int type)
        {

            Rectangle[] rects = new Rectangle[6];

            switch (type)
            {
                case 1:
                    rects[0] = new Rectangle(34, 0, 16, 16);     /// Down
                    rects[1] = new Rectangle(0, 0, 16, 16);     /// Left
                    rects[2] = new Rectangle(17, 0, 16, 16);     /// Up
                    rects[3] = new Rectangle(0, 0, 16, 16);     /// Right
                    rects[4] = new Rectangle(0, 0, 16, 16);     /// Front
                    rects[5] = new Rectangle(0, 0, 16, 16);     /// Back
                    break;

                case 2:
                    rects[0] = new Rectangle(34 + Util.random.Next(0, 1) * 17, 0, 16, 16);     /// Down
                    rects[1] = new Rectangle(34 + Util.random.Next(0, 1) * 17, 0, 16, 16);     /// Left
                    rects[2] = new Rectangle(34 + Util.random.Next(0, 1) * 17, 0, 16, 16);     /// Up
                    rects[3] = new Rectangle(34 + Util.random.Next(0, 1) * 17, 0, 16, 16);     /// Right
                    rects[4] = new Rectangle(34 + Util.random.Next(0, 1) * 17, 0, 16, 16);     /// Front
                    rects[5] = new Rectangle(34 + Util.random.Next(0, 1) * 17, 0, 16, 16);     /// Back
                    break;

                case 3:
                    rects[0] = new Rectangle(68, 0, 16, 16);     /// Down
                    rects[1] = new Rectangle(68, 0, 16, 16);     /// Left
                    rects[2] = new Rectangle(68, 0, 16, 16);     /// Up
                    rects[3] = new Rectangle(68, 0, 16, 16);     /// Right
                    rects[4] = new Rectangle(68, 0, 16, 16);     /// Front
                    rects[5] = new Rectangle(68, 0, 16, 16);     /// Back
                    break;
            }

            return rects;

        }


        public void GetAdjacentBlock(Vector3 position, ref bool front, ref bool back, ref bool left, ref bool right, ref bool up, ref bool down)
        {

            int x = (int)position.X;
            int y = (int)position.Y;
            int z = (int)position.Z;


            if (Handler.GetBlock(new Vector3(x - 1, y, z)) != 0) right = true;
            if (Handler.GetBlock(new Vector3(x + 1, y, z)) != 0) left = true;
            if (Handler.GetBlock(new Vector3(x, y, z - 1)) != 0) front = true;
            if (Handler.GetBlock(new Vector3(x, y, z + 1)) != 0) back = true;
            if (Handler.GetBlock(new Vector3(x, y - 1, z)) != 0) down = true;
            if (Handler.GetBlock(new Vector3(x, y + 1, z)) != 0) up = true;

        }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    if(buffer != null)
                        Main.VERTEX -= buffer.VertexCount;

                    buffer?.Dispose();
                    buffer = null;

                    index?.Dispose();
                    index = null;

                    vertex?.Clear();
                    vertex = null;

                    indices?.Clear();
                    indices = null;

                    blocks = null;
                }

                gpu = null;
                tex = null;
                disposedValue = true;
            }
        }

        ~SubChunk()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


}
