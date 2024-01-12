using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeComp
{

    internal class BlockV5
    {

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        // Définis les positions des sommets du cube
        //VertexPositionTexture cubeVertices = new VertexPositionTexture[]
        //{
        //    // Face arrière
        //    new VertexPositionTexture(new Vector3(-1, 1, -1), new Vector2(1, 0)); // Coin supérieur gauche arrière
        //    new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(0, 0)); // Coin supérieur droit arrière
        //    new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(1, 1)), // Coin inférieur gauche arrière
        //    new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(0, 1)), // Coin inférieur droit arrière

        //    // Face supérieure
        //    new VertexPositionTexture(new Vector3(-1, 1, 1), new Vector2(0, 0)), // Coin supérieur gauche avant
        //    new VertexPositionTexture(new Vector3(1, 1, 1), new Vector2(1, 0)), // Coin supérieur droit avant
        //    new VertexPositionTexture(new Vector3(-1, 1, -1), new Vector2(0, 1)), // Coin supérieur gauche arrière
        //    new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 1)), // Coin supérieur droit arrière
        //};

    // Définis les indices pour former les faces du cube
    int[] cubeIndices =
        {
            0, 1, 2, 2, 3, 0,    // Face arrière
            1, 5, 6, 6, 2, 1,    // Face droite
            4, 0, 3, 3, 7, 4,    // Face gauche
            4, 5, 1, 1, 0, 4,    // Face inférieure
            3, 2, 6, 6, 7, 3,    // Face supérieure
            7, 6, 5, 5, 4, 7     // Face avant
        };


        public BlockV5()
        {



        }


        public void Setindex(GraphicsDevice g)
        {

            indexBuffer = new IndexBuffer(g, IndexElementSize.SixteenBits, cubeIndices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(cubeIndices);

        }

        public void SetVertexData(VertexBuffer vertex)
        {
            //vertex.SetData(cubeVertices);
        }

        public void Draw(GraphicsDevice g, VertexBuffer vertex, BasicEffect basicEffect) 
        {

            

            g.Indices = indexBuffer;


            basicEffect.TextureEnabled = true;

            basicEffect.Texture = Main.GrassUp;


            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, cubeVertices.Length, 0, cubeIndices.Length / 1);
            }


        }


    }
}
