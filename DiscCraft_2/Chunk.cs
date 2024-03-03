using DiscCraft;
using DiscCraft_2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;

namespace DiscCraft
{

    /// <summary>
    /// Tronçons : 16x3x16
    /// no optimisation : 18 432 vertex
    /// optimisation : 2 816 vertex
    /// </summary>

    public class Chunk
    {

        public Vector2 Position;
        //public Vector2 Pos;


        public Block[,,] blocks = new Block[16, 3, 16];


        public Texture2D tex = Main.BlockSheet;

        public VertexBuffer vertexBuffer; // 65535
        public IndexBuffer indexBuffer;
        public GraphicsDevice gpu;

        public VertexPositionNormalTexture[] vertex = new VertexPositionNormalTexture[65535];
        public ushort[] indices = new ushort[65535];

        public int vertexCount = 0;
        public int indexCount = 0;
        public int triangle_count = 0;

        public int vbuf_start = 0;
        public int ibuf_start = 0;

        public ushort vbytes = sizeof(float) * 8;
        public ushort ibytes = sizeof(ushort) * 8;

        public bool isDrawed = true;


        public Chunk(Vector2 _position, GraphicsDevice _gpu)
        {
            gpu = _gpu;

            Position = _position;

            blocks = new Block[16, 64, 16]; //new Block[16, 64, 16];

            //Pos = Position;

            Position = Position * 16;

            vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionNormalTexture), 65535, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gpu, typeof(ushort), 65535, BufferUsage.WriteOnly);


            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 1; y < blocks.GetLength(1); y++)
                {
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        blocks[x, y, z] = new Block(new Vector3(x, y, z), 2);
                    }
                }
            }

            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        if(Random.Shared.Next(0, 2) == 1)
                            blocks[x, y, z] = new Block(new Vector3(x, y, z), 1);
                    }
                }
            }

            //Console.WriteLine(vertexCount);

        }

        public void InitVertexBuffer()
        {

            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 1; y < blocks.GetLength(1); y++)
                {
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        /// TODO : Block Adder
                        if (blocks[x, y, z] != null)
                            AddBlock(new Vector3(x + Position.X, -y, z + Position.Y), new Vector3(x, y, z), 2);
                    }
                }
            }

            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        /// TODO : Block Adder
                        if (blocks[x, y, z] != null)
                            AddBlock(new Vector3(x + Position.X, -y, z + Position.Y), new Vector3(x, y, z), 1);
                    }
                }
            }

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertex);
            indexBuffer.SetData<ushort>(indices);
        }

        public void Draw(GraphicsDevice gpu, BasicEffect basicEffect)
        {

            gpu.SetVertexBuffer(vertexBuffer);
            gpu.Indices = indexBuffer;

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
                gpu.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, triangle_count);
            }

        }


        public void AddBlock(Vector3 _position, Vector3 _positionInChunk, int type)
        {
            CreateTexturedCube(_position, _positionInChunk, gpu, new Vector3(1,1,1), type);
        }



        /// <summary>
        /// Multi_textured Block
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="gpu"></param>
        /// <param name="size"></param>
        public void CreateTexturedCube(Vector3 Pos, Vector3 PosInChunk, GraphicsDevice gpu, Vector3 size, int type)
        {

            float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = size.X / 2, hl = size.Z / 2, hh = size.Y / 2; // uv's, half-width, half-length, half-height
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[0]);
            float t = Pos.Y - hh, b = Pos.Y + hh, l = Pos.X - hw, r = Pos.X + hw, n = Pos.Z - hl, f = Pos.Z + hl;  // y-coord, left, right, near, far


            bool front = false, back = false, left = false, right = false, up = false, down = false;

            GetAdjacentBlock(PosInChunk, ref front, ref back, ref left, ref right, ref up, ref down);

            Vector3 norm = Vector3.Up;

            /// Down
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[0]);
            if (!down) { norm = Vector3.Up; AddVertex(l, t, f, norm, u1, v1); AddVertex(r, t, f, norm, u2, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2); } // (left,y,far),(right,y,far),(right,y,near) [clockwise]

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
            if (!front) { norm = Vector3.Backward; AddVertex(l, t, n, norm, u2, v2); AddVertex(r, t, n, norm, u1, v2); AddVertex(r, b, n, norm, u1, v1); AddVertex(l, b, n, norm, u2, v1); }

            /// Back
            GetUVCoords(ref u1, ref v1, ref u2, ref v2, GetSourceRect(type)[5]);
            if (!back) { norm = Vector3.Forward; AddVertex(r, t, f, norm, u2, v2); AddVertex(l, t, f, norm, u1, v2); AddVertex(l, b, f, norm, u1, v1); AddVertex(r, b, f, norm, u2, v1); }


            ushort offset = 0;
            ushort offset2 = 0;

            if (down) offset2 += 4;
            if (left) offset2 += 4;
            if (up) offset2 += 4;
            if (right) offset2 += 4;
            if (front) offset2 += 4;
            if (back) offset2 += 4;

            /// DOWN
            if (!down) { AddTriangle(0, 1, 2, offset2); triangle_count++; AddTriangle(2, 3, 0, offset2); triangle_count++; }  // clockwise order
            if(down) offset += 4;

            /// LEFT
            if (!left) { AddTriangle((ushort)(5 - offset), (ushort)(7 - offset), (ushort)(6 - offset), offset2); triangle_count++; AddTriangle((ushort)(7 - offset), (ushort)(6 - offset), (ushort)(4 - offset), offset2); triangle_count++; }  // clockwise order
            if (left) offset += 4;

            /// UP
            if (!up) { AddTriangle((ushort)(11 - offset), (ushort)(8 - offset), (ushort)(10 - offset), offset2); triangle_count++; AddTriangle((ushort)(10 - offset), (ushort)(9 - offset), (ushort)(11 - offset), offset2); triangle_count++; }  // clockwise order
            if (up) offset += 4;

            /// RIGHT
            if (!right) { AddTriangle((ushort)(12 - offset), (ushort)(13 - offset), (ushort)(14 - offset), offset2); triangle_count++; AddTriangle((ushort)(14 - offset), (ushort)(15 - offset), (ushort)(12 - offset), offset2); triangle_count++; }  // clockwise order
            if (right) offset += 4;

            /// FRONT
            if (!front) { AddTriangle((ushort)(16 - offset), (ushort)(17 - offset), (ushort)(18 - offset), offset2); triangle_count++; AddTriangle((ushort)(18 - offset), (ushort)(19 - offset), (ushort)(16 - offset), offset2); triangle_count++; }  // clockwise order
            if (front) offset += 4;

            /// BACK
            if (!back) { AddTriangle((ushort)(20 - offset), (ushort)(21 - offset), (ushort)(22 - offset), offset2); triangle_count++; AddTriangle((ushort)(22 - offset), (ushort)(23 - offset), (ushort)(20 - offset), offset2); triangle_count++; }  // clockwise order

            //vertexBuffer.SetData<VertexPositionNormalTexture>(vertex); // vbuf_start * vbytes, vertex, 0, ve
            //rtexCount, vbytes //vbuf_start = vertexCount; vertexCount = 0;
            //indexBuffer.SetData<ushort>(indices); //ibuf_start * ibytes, indices, 0, ibytes  //ibuf_start = indexCount; indexCount = 0;

        }


        public void AddVertex(float x, float y, float z, Vector3 norm, float u, float v)
        {
            if ((vbuf_start + vertexCount) >= 65535) { Console.WriteLine("MAX VERTEX !"); return; }
            vertex[vertexCount] = new VertexPositionNormalTexture(new Vector3(x, y, z), norm, new Vector2(u, v));
            vertexCount += 1;
            Main.vertexNum += 1;

        }

        public void AddTriangle(ushort a, ushort b, ushort c, ushort _offset)
        {
            if (indexCount + 3 > 65535) { Console.WriteLine("MAX TRIANGLE"); return; }
            ushort offset = (ushort)((ushort)vertexCount - 24 + _offset);
            a += offset; b += offset; c += offset;
            indices[indexCount] = a; indexCount++;
            indices[indexCount] = b; indexCount++;
            indices[indexCount] = c; indexCount++;
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
            }


            return rects;


        }


        public void GetAdjacentBlock(Vector3 position, ref bool front, ref bool back, ref bool left, ref bool right, ref bool up, ref bool down)
        {

            int x = (int)position.X;
            int y = (int)position.Y;
            int z = (int)position.Z;

            if (x > 0) 
            {
                if (blocks[x - 1, y, z] == null) right = false;
                else right = true;
            }

            if (x < 16 - 1)
            {
                if (blocks[x + 1, y, z] == null) left = false;
                else left = true;
            }



            if (z > 0)
            {
                if (blocks[x, y, z - 1] == null) front = false;
                else front = true;
            }

            if (z < 16 - 1)
            {
                if (blocks[x, y, z + 1] == null) back = false;
                else back = true;
            }



            if (y > 0)
            {
                if (blocks[x, y - 1, z] == null) up = false;
                else up = true;
            }

            if (y < blocks.GetLength(1) - 1)
            {
                if (blocks[x, y + 1, z] == null) down = false;
                else down = true;
            }




            /// Between chunk !
            if (x == 0)
            {
                if (Handler.GetChunk(Position / 16 - new Vector2(1, 0)) != null)
                {
                    if (Handler.GetChunk(Position / 16 - new Vector2(1, 0)).blocks[16 - 1, y, z] == null) right = false;
                    else right = true;
                }
                //else right = true;
            }

            if (x == 16 - 1)
            {
                if (Handler.GetChunk(Position / 16 + new Vector2(1, 0)) != null)
                {
                    if (Handler.GetChunk(Position / 16 + new Vector2(1, 0)).blocks[0, y, z] == null) left = false;
                    else left = true;
                }
                //else left = true;
            }



            if (z == 0)
            {
                if (Handler.GetChunk(Position / 16 - new Vector2(0, 1)) != null)
                {
                    if (Handler.GetChunk(Position / 16 - new Vector2(0, 1)).blocks[x, y, 16 - 1] == null) front = false;
                    else front = true;
                }
                //else front = true;
            }

            if (z == 16 - 1)
            {
                if (Handler.GetChunk(Position / 16 + new Vector2(0, 1)) != null)
                {
                    if (Handler.GetChunk(Position / 16 + new Vector2(0, 1)).blocks[x, y, 0] == null) back = false;
                    else back = true;
                }
                //else back = true;
            }



        }

    }

}
