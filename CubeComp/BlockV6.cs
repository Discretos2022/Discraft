using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CubeComp
{

    internal class BlockV6
    {

        public IndexBuffer indexBuffer;
        public VertexBuffer vertexBuffer;

        public VertexPositionTexture[] vertices = new VertexPositionTexture[24];

        private short[] indices = new short[36];


        public Vector3 Position;


        public BlockV6(Vector3 _position, GraphicsDevice g)
        {

            Position = _position;

            vertexBuffer = new VertexBuffer(g, typeof(VertexPositionColor), 10000, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(g, typeof(short), 36, BufferUsage.WriteOnly);

            CreateVertex();
            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);

            

        }


        public void Draw(GraphicsDevice g, BasicEffect basicEffect)
        {
            CreateVertex();
            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);


            g.Indices = indexBuffer;

            basicEffect.VertexColorEnabled = false;
            //basicEffect.Alpha = 1f;


            basicEffect.TextureEnabled = true;
            //basicEffect.VertexColorEnabled = true;


            basicEffect.Texture = Main.GrassUp;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {

                pass.Apply();
                g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 8, 0, 24);
                //g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12, 0, 20);


            }

        }



        private void CreateVertex()
        {

            float x = Position.X;
            float y = Position.Y;
            float z = Position.Z;

            /// Front face
            vertices[0] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(1, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(1, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(1, 1));

            vertices[4] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(0, 0));
            vertices[5] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(1, 0));
            vertices[6] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 1));
            vertices[7] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(1, 1));



            //vertices[0] = new VertexPositionNormalTexture(new Vector3(x + 0, y + 0, z + 0), new Vector3(0, 0, 0), new Vector2(0, 0));
            //vertices[1] = new VertexPositionNormalTexture(new Vector3(x + 1, y + 0, z + 0), new Vector3(0, 0, 0), new Vector2(0, 0));
            //vertices[2] = new VertexPositionNormalTexture(new Vector3(x + 0, y + 1, z + 0), new Vector3(0, 0, 0), new Vector2(0, 1));
            //vertices[3] = new VertexPositionNormalTexture(new Vector3(x + 1, y + 1, z + 0), new Vector3(0, 0, 0), new Vector2(1, 1));

            //vertices[4] = new VertexPositionNormalTexture(new Vector3(x + 0, y + 0, z + 1), new Vector3(0, 0, 0), new Vector2(0, 0));
            //vertices[5] = new VertexPositionNormalTexture(new Vector3(x + 1, y + 0, z + 1), new Vector3(0, 0, 0), new Vector2(1, 0));
            //vertices[6] = new VertexPositionNormalTexture(new Vector3(x + 0, y + 1, z + 1), new Vector3(0, 0, 0), new Vector2(0, 1));
            //vertices[7] = new VertexPositionNormalTexture(new Vector3(x + 1, y + 1, z + 1), new Vector3(0, 0, 0), new Vector2(1, 1));



            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;

            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            indices[6] = 1;
            indices[7] = 5;
            indices[8] = 3;

            //indices[9] = 5;
            //indices[10] = 7;
            //indices[11] = 3;

            //indices[12] = 5;
            //indices[13] = 4;
            //indices[14] = 7;

            //indices[15] = 4;
            //indices[16] = 6;
            //indices[17] = 7;

            //indices[18] = 4;
            //indices[19] = 0;
            //indices[20] = 6;

            //indices[21] = 0;
            //indices[22] = 2;
            //indices[23] = 6;

            //indices[24] = 2;
            //indices[25] = 3;
            //indices[26] = 6;

            //indices[27] = 3;
            //indices[28] = 7;
            //indices[29] = 6;

            //indices[30] = 0;
            //indices[31] = 1;
            //indices[32] = 4;

            //indices[33] = 1;
            //indices[34] = 5;
            //indices[35] = 4;



        }



    }
}
