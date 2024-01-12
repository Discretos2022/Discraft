using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscCraft
{

    public class BlockV3
    {

        public Vector3 Position;
        public Vector3 PositionInChunk;

        public Chunk chunk;

        private VertexPositionTexture[] vertex;

        private Texture2D beforeTex;
        private Texture2D behindTex;
        private Texture2D rightTex;
        private Texture2D leftTex;
        private Texture2D upTex;
        private Texture2D downTex;

        private bool isHide;

        public bool isSelected;

        public BlockV3(Vector3 _position, Vector3 _positionInChunk, Chunk _chunk)
        {
            Position = _position;
            PositionInChunk = _positionInChunk;

            chunk = _chunk;

            vertex = new VertexPositionTexture[36];

            CreateVertex();

        }


        public void Update()
        {

        }


        public void Draw(GraphicsDevice g, BasicEffect basicEffect, BlockV3[,,] blockList, Vector3 cameraPos)
        {

            int x = (int)PositionInChunk.X;
            int y = (int)PositionInChunk.Y;
            int z = (int)PositionInChunk.Z;

            int xG = (int)Position.X;
            int yG = (int)Position.Y;
            int zG = (int)Position.Z;

            //CreateVertex();



            basicEffect.VertexColorEnabled = true;
            basicEffect.Alpha = 1f;


            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = false;

            basicEffect.AmbientLightColor = new Vector3(1, 1, 1);
            basicEffect.LightingEnabled = true;

            if (isSelected)
            {
                //basicEffect.TextureEnabled = false;
                //basicEffect.VertexColorEnabled = true;
                basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 1f);
                basicEffect.LightingEnabled = true;

            }

            basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 1f);
            basicEffect.LightingEnabled = true;



            if (!isHide)
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {

                    /// Uniquement si transparent   g.DepthStencilState = DepthStencilState.DepthRead;



                    /// Face 6
                    if (y == 0 || blockList[(int)x, (int)y - 1, (int)z] is null)
                        if (yG > cameraPos.Y)
                        {
                            basicEffect.Texture = downTex;
                            pass.Apply();
                            g.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 30, 2);
                        }

                    /// Uniquement si transparent   g.DepthStencilState = DepthStencilState.Default;




                    /// Face 1
                    if (z == 0 || blockList[(int)x, (int)y, (int)z - 1] is null)
                        //if(chunk.ChunkPosition.Y == 0 || (Handler.chunks[(int)chunk.ChunkPosition.X, (int)chunk.ChunkPosition.Y - 1].isLoaded && Handler.chunks[(int)chunk.ChunkPosition.X, (int)chunk.ChunkPosition.Y - 1].GetBlock(x, y, 15) == null))
                        if (zG > cameraPos.Z)
                        {
                            basicEffect.Texture = beforeTex;
                            pass.Apply();
                            g.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 0, 2);
                        }


                    /// Face 2
                    if (x == blockList.GetLength(0) - 1 || blockList[(int)x + 1, (int)y, (int)z] is null)
                        if (xG < cameraPos.X)
                        {
                            basicEffect.Texture = rightTex;
                            pass.Apply();
                            g.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 6, 2);
                        }



                    /// Face 3
                    if (z == blockList.GetLength(2) - 1 || blockList[(int)x, (int)y, (int)z + 1] is null)
                        if (zG < cameraPos.Z)
                        {
                            basicEffect.Texture = behindTex;
                            pass.Apply();
                            g.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 12, 2);
                        }
                            

                    

                    /// Face 4
                    if (x == 0 || blockList[(int)x - 1, (int)y, (int)z] is null)
                        if (xG > cameraPos.X)
                        {
                            basicEffect.Texture = leftTex;
                            pass.Apply();
                            g.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 18, 2);
                        }


                    /// Face 5
                    if (y == blockList.GetLength(1) - 1 || blockList[(int)x, (int)y + 1, (int)z] is null)
                        if (yG < cameraPos.Y)
                        {
                            basicEffect.Texture = upTex;
                            pass.Apply();
                            g.DrawUserPrimitives(PrimitiveType.TriangleList, vertex, 24, 2);
                        }


                    if (isSelected)
                    {
                        basicEffect.TextureEnabled = false;
                        basicEffect.VertexColorEnabled = true;

                        //Main.vertexBuffer.SetData(vertex);

                        g.DrawUserPrimitives(PrimitiveType.LineList, vertex, 0, 12);



                    }




                }

        }


        private void CreateVertex()
        {

            float x = Position.X;
            float y = Position.Y;
            float z = Position.Z;

            vertex[0] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(0, 1));
            vertex[1] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(1, 1));
            vertex[2] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(0, 0));

            vertex[3] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(1, 1));
            vertex[4] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(1, 0));
            vertex[5] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(0, 0));


            vertex[6] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(0, 1));
            vertex[7] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(1, 1));
            vertex[8] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(0, 0));

            vertex[9] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(1, 1));
            vertex[10] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(1, 0));
            vertex[11] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(0, 0));


            vertex[12] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(0, 1));
            vertex[13] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(1, 1));
            vertex[14] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(0, 0));

            vertex[15] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(1, 1));
            vertex[16] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(1, 0));
            vertex[17] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(0, 0));


            vertex[18] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(0, 1));
            vertex[19] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(1, 1));
            vertex[20] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 0));

            vertex[21] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(1, 1));
            vertex[22] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(1, 0));
            vertex[23] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 0));


            vertex[24] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 0), new Vector2(0, 1));
            vertex[25] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(1, 1));
            vertex[26] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 0));

            vertex[27] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 0), new Vector2(1, 1));
            vertex[28] = new VertexPositionTexture(new Vector3(x + 1, y + 1, z + 1), new Vector2(1, 0));
            vertex[29] = new VertexPositionTexture(new Vector3(x + 0, y + 1, z + 1), new Vector2(0, 0));


            vertex[30] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 0), new Vector2(0, 1));
            vertex[31] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(1, 1));
            vertex[32] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(0, 0));

            vertex[33] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 0), new Vector2(1, 1));
            vertex[34] = new VertexPositionTexture(new Vector3(x + 1, y + 0, z + 1), new Vector2(1, 0));
            vertex[35] = new VertexPositionTexture(new Vector3(x + 0, y + 0, z + 1), new Vector2(0, 0));

        }

        public void SetTexture(Texture2D _beforeTex, Texture2D _behindTex, Texture2D _rightTex, Texture2D _leftTex, Texture2D _upTex, Texture2D _downTex)
        {
            beforeTex = _beforeTex;
            behindTex = _behindTex;
            rightTex = _rightTex;
            leftTex = _leftTex;
            upTex = _upTex;
            downTex = _downTex;
        }

        public void Hide()
        {
            isHide = true;
        }

        public void Show()
        {
            isHide = false;
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(new Vector3(Position.X, Position.Y, Position.Z), Position + Vector3.One);
        }


    }
}
