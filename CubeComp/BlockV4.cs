using CubeComp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscCraft
{
    public class BlockV4
    {

        public Vector3 Position;
        public Vector3 PositionInChunk;

        private VertexPositionTexture[] vertex;

        private short[] indices = new short[36];
        //private short[] indices = new short[60];

        private IndexBuffer indexBuffer;

        private Texture2D beforeTex;
        private Texture2D behindTex;
        private Texture2D rightTex;
        private Texture2D leftTex;
        private Texture2D upTex;
        private Texture2D downTex;

        private bool isHide;

        public bool isSelected;

        public BlockV4(Vector3 _position, Vector3 _positionInChunk)
        {
            Position = _position;
            PositionInChunk = _positionInChunk;

            //vertex = new VertexPositionTexture[36];
            vertex = new VertexPositionTexture[12];

            CreateVertex();

        }

        public void Update()
        {



        }

        public void SetVertexData(VertexBuffer vertexBuffer, GraphicsDevice g)
        {
            CreateVertex();
            vertexBuffer.SetData(vertex);

            indexBuffer = new IndexBuffer(g, typeof(short), indices.Length, BufferUsage.WriteOnly);
            
        }

        [Obsolete]
        public void Draw(GraphicsDevice g, BasicEffect basicEffect, Vector3 cameraPos)
        {



            //indexBuffer = new IndexBuffer(g, typeof(short), indices.Length, BufferUsage.WriteOnly);
            //indexBuffer.SetData(vertex);


            indexBuffer.SetData(indices);
            g.Indices = indexBuffer;


            int x = (int)PositionInChunk.X;
            int y = (int)PositionInChunk.Y;
            int z = (int)PositionInChunk.Z;

            int xG = (int)Position.X;
            int yG = (int)Position.Y;
            int zG = (int)Position.Z;

                

            basicEffect.VertexColorEnabled = false;
            //basicEffect.Alpha = 1f;


            basicEffect.TextureEnabled = true;
            //basicEffect.VertexColorEnabled = true;


            basicEffect.Texture = Main.GrassSide;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {

                pass.Apply();
                g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 12);
                //g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12, 0, 20);


            }


        }


        private void CreateVertex()
        {

            float x = Position.X;
            float y = Position.Y;
            float z = Position.Z;

            vertex[0] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(1, 1));
            vertex[1] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(0, 1));
            vertex[2] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(1, 0));
            vertex[3] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(0, 0));

            vertex[4] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(0, 0));
            vertex[5] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(1, 0));
            vertex[6] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 1));
            vertex[7] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(1, 1));


            /*
            vertex[0] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(0, 1));
            vertex[1] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(1, 1));
            vertex[2] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(0, 0));
            vertex[3] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(0, 0));

            vertex[4] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(0, 1));
            vertex[5] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(1, 1));
            vertex[6] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 0));
            vertex[7] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(0, 0));
            */




            //vertex[0] = new VertexPositionColor(new Vector3(x + 0, y + 0, z + 0), Color.Red);
            //vertex[1] = new VertexPositionColor(new Vector3(x + 1, y + 0, z + 0), Color.Yellow);
            //vertex[2] = new VertexPositionColor(new Vector3(x + 0, y + 1, z + 0), Color.Green);
            //vertex[3] = new VertexPositionColor(new Vector3(x + 1, y + 1, z + 0), Color.Blue);

            //vertex[4] = new VertexPositionColor(new Vector3(x + 0, y + 0, z + 1), Color.Red);
            //vertex[5] = new VertexPositionColor(new Vector3(x + 1, y + 0, z + 1), Color.Red);
            //vertex[6] = new VertexPositionColor(new Vector3(x + 0, y + 1, z + 1), Color.Red);
            //vertex[7] = new VertexPositionColor(new Vector3(x + 1, y + 1, z + 1), Color.Red);


            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;

            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            indices[6] = 1;
            indices[7] = 5;
            indices[8] = 3;

            indices[9] = 5;
            indices[10] = 7;
            indices[11] = 3;

            indices[12] = 5;
            indices[13] = 4;
            indices[14] = 7;

            indices[15] = 4;
            indices[16] = 6;
            indices[17] = 7;

            indices[18] = 4;
            indices[19] = 0;
            indices[20] = 6;

            indices[21] = 0;
            indices[22] = 2;
            indices[23] = 6;

            indices[24] = 2;
            indices[25] = 3;
            indices[26] = 6;

            indices[27] = 3;
            indices[28] = 7;
            indices[29] = 6;

            indices[30] = 0;
            indices[31] = 1;
            indices[32] = 4;

            indices[33] = 1;
            indices[34] = 5;
            indices[35] = 4;


            // vertex position and color information for icosahedron
            //vertex[0] = new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, 0.42532500f), Color.Red);
            //vertex[1] = new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, 0.42532500f), Color.Orange);
            //vertex[2] = new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, -0.42532500f), Color.Yellow);
            //vertex[3] = new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, -0.42532500f), Color.Green);
            //vertex[4] = new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, 0.26286500f), Color.Blue);
            //vertex[5] = new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, -0.26286500f), Color.Indigo);
            //vertex[6] = new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, 0.26286500f), Color.Purple);
            //vertex[7] = new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, -0.26286500f), Color.White);
            //vertex[8] = new VertexPositionColor(new Vector3(0.42532500f, 0.26286500f, 0.0000000f), Color.Cyan);
            //vertex[9] = new VertexPositionColor(new Vector3(-0.42532500f, 0.26286500f, 0.0000000f), Color.Black);
            //vertex[10] = new VertexPositionColor(new Vector3(0.42532500f, -0.26286500f, 0.0000000f), Color.DodgerBlue);
            //vertex[11] = new VertexPositionColor(new Vector3(-0.42532500f, -0.26286500f, 0.0000000f), Color.Crimson);


            //indices[0] = 0; indices[1] = 6; indices[2] = 1;
            //indices[3] = 0; indices[4] = 11; indices[5] = 6;
            //indices[6] = 1; indices[7] = 4; indices[8] = 0;
            //indices[9] = 1; indices[10] = 8; indices[11] = 4;
            //indices[12] = 1; indices[13] = 10; indices[14] = 8;
            //indices[15] = 2; indices[16] = 5; indices[17] = 3;
            //indices[18] = 2; indices[19] = 9; indices[20] = 5;
            //indices[21] = 2; indices[22] = 11; indices[23] = 9;
            //indices[24] = 3; indices[25] = 7; indices[26] = 2;
            //indices[27] = 3; indices[28] = 10; indices[29] = 7;
            //indices[30] = 4; indices[31] = 8; indices[32] = 5;
            //indices[33] = 4; indices[34] = 9; indices[35] = 0;
            //indices[36] = 5; indices[37] = 8; indices[38] = 3;
            //indices[39] = 5; indices[40] = 9; indices[41] = 4;
            //indices[42] = 6; indices[43] = 10; indices[44] = 1;
            //indices[45] = 6; indices[46] = 11; indices[47] = 7;
            //indices[48] = 7; indices[49] = 10; indices[50] = 6;
            //indices[51] = 7; indices[52] = 11; indices[53] = 2;
            //indices[54] = 8; indices[55] = 10; indices[56] = 3;
            //indices[57] = 9; indices[58] = 11; indices[59] = 0;



        }


    }
}
