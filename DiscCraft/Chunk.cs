using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscCraft
{
    public class Chunk
    {

        private int Width = 16;
        private int Height = 255;

        public Vector3 Position;
        public Vector2 ChunkPosition;

        public BlockV3[,,] block;

        public bool isLoaded;


        public Chunk(Vector3 _position)
        {
            Position = _position;
            ChunkPosition.X = Position.X / 16;
            ChunkPosition.Y = Position.Z / 16;

            block = new BlockV3[Width, Height, Width];

        }

        public void Draw(Camera cameraV2, GraphicsDevice g, BasicEffect basicEffect)
        {

            int RenderZone = 50;

            int xMin = (((int)cameraV2.Position.X / 2 - RenderZone) * 4) / 4;
            int xMax = (((int)cameraV2.Position.X / 2 + RenderZone) * 4) / 4;
            int yMin = (((int)cameraV2.Position.Y - RenderZone) * 4) / 4;
            int yMax = (((int)cameraV2.Position.Y + RenderZone) * 4) / 4;
            int zMin = (((int)cameraV2.Position.Z / 2 - RenderZone) * 4) / 4;
            int zMax = (((int)cameraV2.Position.Z / 2 + RenderZone) * 4) / 4;


            if (xMin < 0)
                xMin = 0;

            if (yMin < 0)
                yMin = 0;

            if (zMin < 0)
                zMin = 0;

            if (xMax > block.GetLength(0))
                xMax = block.GetLength(0);

            if (yMax > block.GetLength(1))
                yMax = block.GetLength(1);

            if (zMax > block.GetLength(2))
                zMax = block.GetLength(2);


            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < 15; y++)
                {
                    for (int z = zMin; z < zMax; z++)
                    {
                        if (block[x, y, z] != null)
                        {

                            block[x, y, z].SetTexture(Main.GrassSide, Main.GrassSide, Main.GrassSide, Main.GrassSide, Main.GrassUp, Main.GrassDown);
                            block[x, y, z].Draw(g, basicEffect, block, cameraV2.Position);
                            block[x, y, z].isSelected = false;
                        }
                    }
                }
            }

        }

        public void Generate()
        {
            for (int x = 0; x < block.GetLength(0); x++)
            {
                for (int y = 0; y < block.GetLength(1); y++)
                {
                    for (int z = 0; z < block.GetLength(2); z++)
                    {
                        if (y < 10 + Util.random.Next(0, 2))
                            block[x, y, z] = new BlockV3(Position + new Vector3(x, y, z), new Vector3(x, y, z), this);
                    }
                }
            }

            isLoaded = true;

        }


        public BlockV3 GetBlock(int x, int y, int z)
        {
            return block[x, y, z];
        }


        public BlockV3[,,] GetBlockList()
        {
            return block;
        }


    }
}
