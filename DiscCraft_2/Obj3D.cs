using DiscCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;

namespace DiscCraft
{
    public class Obj3D
    {

        public Vector3 Pos;

        public Texture2D tex = Main.BlockSheet;
        public Rectangle source_rect = new Rectangle(0, 0, 16, 16);

        public VertexBuffer vertexBuffer; // 65535
        public IndexBuffer indexBuffer;

        public VertexPositionNormalTexture[] vertex = new VertexPositionNormalTexture[65535];
        public ushort[] indices = new ushort[65535];

        public int vertexCount = 0;
        public int indexCount = 0;
        public int triangle_count = 0;

        public ushort vbytes = sizeof(ushort) * 8;
        public ushort ibytes = sizeof(ushort) * 8;


        public Obj3D(Vector3 _position, GraphicsDevice gpu, Vector3 size)
        {
            Pos = _position;

            vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionNormalTexture), 65535, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gpu, typeof(ushort), 65535, BufferUsage.WriteOnly);





            //CreateCube(Pos, gpu, size);


            CreateTexturedCube(Pos, gpu, size);


        }


        public void Draw(GraphicsDevice gpu, BasicEffect basicEffect)
        {

            //CreateCube(new Vector3(0,0,0), gpu, new Vector3(1,1,1));

            //CreateTexturedCube(Pos, gpu, new Vector3(1f, 1f, 1f));

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


            basicEffect.EnableDefaultLighting();   // make sure lighting is on
            basicEffect.LightingEnabled = true;
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.AmbientLightColor = new Vector3(0.1f, 0.2f, 0.3f);    // medium-dark for dark parts of object
            basicEffect.DiffuseColor = new Vector3(0.94f, 0.94f, 0.94f); // fairly bright for lit parts of object
            basicEffect.TextureEnabled = true;  // make sure this is enabled
            basicEffect.EmissiveColor = new Vector3(0.0f, 0.0f, 0.0f);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gpu.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, triangle_count);
            }
            
        }


        /// <summary>
        /// For MonoTextured Block
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="gpu"></param>
        /// <param name="size"></param>
        public void CreateCube(Vector3 _position, GraphicsDevice gpu, Vector3 size)
        {

            float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = size.X / 2, hl = size.Z / 2, hh = size.Y / 2; // uv's, half-width, half-length, half-height
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            float t = Pos.Y - hh, b = Pos.Y + hh, l = Pos.X - hw, r = Pos.X + hw, n = Pos.Z - hl, f = Pos.Z + hl;  // y-coord, left, right, near, far

            source_rect = new Rectangle(0, 17, 16, 16);

            Vector3 norm = Vector3.Up; AddVertex(l, t, f, norm, u1, v1); AddVertex(r, t, f, norm, u2, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2); // (left,y,far),(right,y,far),(right,y,near) [clockwise]
            norm = Vector3.Right; AddVertex(r, b, f, norm, u1, v1); AddVertex(r, b, n, norm, u1, v2);
            norm = Vector3.Down; AddVertex(l, b, f, norm, u2, v1); AddVertex(l, b, n, norm, u2, v2);
            norm = Vector3.Backward; AddVertex(l, t, n, norm, u1, v1); AddVertex(r, t, n, norm, u2, v1); AddVertex(r, b, n, norm, u2, v2); AddVertex(l, b, n, norm, u1, v2);
            norm = Vector3.Forward; AddVertex(r, t, f, norm, u1, v1); AddVertex(l, t, f, norm, u2, v1); AddVertex(l, b, f, norm, u2, v2); AddVertex(r, b, f, norm, u1, v2);

            AddTriangle(0, 1, 2); triangle_count++; AddTriangle(2, 3, 0); triangle_count++;  // clockwise order
            AddTriangle(2, 1, 4); triangle_count++; AddTriangle(4, 5, 2); triangle_count++;
            AddTriangle(5, 4, 6); triangle_count++; AddTriangle(6, 7, 5); triangle_count++;
            AddTriangle(7, 6, 0); triangle_count++; AddTriangle(0, 3, 7); triangle_count++;
            AddTriangle(8, 9, 10); triangle_count++; AddTriangle(10, 11, 8); triangle_count++;
            AddTriangle(12, 13, 14); triangle_count++; AddTriangle(14, 15, 12); triangle_count++;

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertex); /*0 = vertexCount;*/ vertexCount = 0;
            indexBuffer.SetData<ushort>(indices); /*0 = indexCount;*/ indexCount = 0;
        }



        /// <summary>
        /// Multi_textured Block
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="gpu"></param>
        /// <param name="size"></param>
        public void CreateTexturedCube(Vector3 _position, GraphicsDevice gpu, Vector3 size)
        {

            vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionNormalTexture), 65535, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gpu, typeof(ushort), 65535, BufferUsage.WriteOnly);

            vertexCount = 0;
            indexCount = 0;
            triangle_count = 0;



            source_rect = new Rectangle(0, 17, 16, 16);
            float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = size.X / 2, hl = size.Z / 2, hh = size.Y / 2; // uv's, half-width, half-length, half-height
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            float t = Pos.Y - hh, b = Pos.Y + hh, l = Pos.X - hw, r = Pos.X + hw, n = Pos.Z - hl, f = Pos.Z + hl;  // y-coord, left, right, near, far


            /// Down
            source_rect = new Rectangle(0, 34, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            Vector3 norm = Vector3.Up; AddVertex(l, t, f, norm, u1, v1); AddVertex(r, t, f, norm, u2, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2); // (left,y,far),(right,y,far),(right,y,near) [clockwise]

            /// Left
            source_rect = new Rectangle(0, 0, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            norm = Vector3.Right; AddVertex(r, b, f, norm, u1, v1); AddVertex(r, t, n, norm, u2, v2); AddVertex(r, b, n, norm, u2, v1); AddVertex(r, t, f, norm, u1, v2);

            /// Up
            source_rect = new Rectangle(0, 17, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            norm = Vector3.Down; AddVertex(r, b, f, norm, u1, v1); AddVertex(l, b, n, norm, u2, v2); AddVertex(l, b, f, norm, u2, v1); AddVertex(r, b, n, norm, u1, v2);

            /// Right
            source_rect = new Rectangle(0, 0, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            norm = Vector3.Left; AddVertex(l, b, n, norm, u1, v1); AddVertex(l, b, f, norm, u2, v1); AddVertex(l, t, f, norm, u2, v2); AddVertex(l, t, n, norm, u1, v2);

            /// Front
            source_rect = new Rectangle(0, 0, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            norm = Vector3.Backward; AddVertex(l, t, n, norm, u2, v2); AddVertex(r, t, n, norm, u1, v2); AddVertex(r, b, n, norm, u1, v1); AddVertex(l, b, n, norm, u2, v1);

            /// Front
            source_rect = new Rectangle(0, 0, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            norm = Vector3.Forward; AddVertex(r, t, f, norm, u2, v2); AddVertex(l, t, f, norm, u1, v2); AddVertex(l, b, f, norm, u1, v1); AddVertex(r, b, f, norm, u2, v1);


            //norm = Vector3.Down; AddVertex(l, b, f, norm, u2, v1); AddVertex(l, b, n, norm, u2, v2);


            //source_rect = new Rectangle(0, 0, 16, 16);
            //GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            //norm = Vector3.Backward; AddVertex(l, t, n, norm, u1, v1); AddVertex(r, t, n, norm, u2, v1); AddVertex(r, b, n, norm, u2, v2); AddVertex(l, b, n, norm, u1, v2);



            source_rect = new Rectangle(0, 0, 16, 16);
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            //norm = Vector3.Forward; AddVertex(r, t, f, norm, u1, v1); AddVertex(l, t, f, norm, u2, v1); AddVertex(l, b, f, norm, u2, v2); AddVertex(r, b, f, norm, u1, v2);

            /// DOWN
            AddTriangle(0, 1, 2); triangle_count++; AddTriangle(2, 3, 0); triangle_count++;  // clockwise order

            /// LEFT
            AddTriangle(5, 7, 6); triangle_count++; AddTriangle(7, 6, 4); triangle_count++;  // clockwise order

            /// UP
            AddTriangle(11, 8, 10); triangle_count++; AddTriangle(10, 9, 11); triangle_count++;  // clockwise order

            /// RIGHT
            AddTriangle(12, 13, 14); triangle_count++; AddTriangle(14, 15, 12); triangle_count++;  // clockwise order

            /// Front
            AddTriangle(16, 17, 18); triangle_count++; AddTriangle(18, 19, 16); triangle_count++;  // clockwise order

            /// Back
            AddTriangle(20, 21, 22); triangle_count++; AddTriangle(22, 23, 20); triangle_count++;  // clockwise order

            // AddTriangle(12, 13, 14); triangle_count++; AddTriangle(14, 15, 12); triangle_count++


            /// AddTriangle(5, 7, 4); triangle_count++; AddTriangle(4, 5, 2); triangle_count++;

            ///AddTriangle(5 + 2, 4 + 2, 6 + 2); triangle_count++; AddTriangle(6 + 2, 7 + 2, 5 + 2); triangle_count++;
            //AddTriangle(7 + 2, 6 + 2, 0 + 2); triangle_count++; AddTriangle(0 + 2, 3 + 2, 7 + 2); triangle_count++;
            //AddTriangle(8 + 2, 9 + 2, 10 + 2); triangle_count++; AddTriangle(10 + 2, 11 + 2, 8 + 2); triangle_count++;
            //AddTriangle(12 + 2, 13 + 2, 14 + 2); triangle_count++; AddTriangle(14 + 2, 15 + 2, 12 + 2); triangle_count++;

            //AddTriangle(4, 5, 6); triangle_count++; AddTriangle(6, 7, 4); triangle_count++;

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertex); /*0 = vertexCount;*/ vertexCount = 0;
            indexBuffer.SetData<ushort>(indices); /*0 = indexCount;*/ indexCount = 0;

            //Console.WriteLine(vertexCount);

        }


        public void AddVertex(float x, float y, float z, Vector3 norm, float u, float v)
        {

            if (vertexCount > 65535) { Console.WriteLine("MAX VERTEX !"); return; }

            vertex[vertexCount] = new VertexPositionNormalTexture(new Vector3(x, y, z), norm, new Vector2(u, v));

            vertexCount += 1;


        }

        public void AddTriangle(ushort a, ushort b, ushort c)
        {
            if (indexCount + 3 > 65530) { Console.WriteLine("MAX TRIANGLE"); return; }
            ushort offset = (ushort)0;
            a += offset; b += offset; c += offset;
            indices[indexCount] = a; indexCount++; 
            indices[indexCount] = b; indexCount++; 
            indices[indexCount] = c; indexCount++;
        }


        protected void GetUVCoords(ref float u1, ref float v1, ref float u2, ref float v2)
        {
            u1 = source_rect.X / (float)tex.Width;                                                // get the uv coords (texture map coords)
            v1 = source_rect.Y / (float)tex.Height;
            u2 = (source_rect.X + source_rect.Width) / (float)tex.Width;
            v2 = (source_rect.Y + source_rect.Height) / (float)tex.Height;
        }


    }
}
