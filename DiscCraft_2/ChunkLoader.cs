using DiscCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DiscCraft_2
{
    public static class ChunkLoader
    {

        public static Chunk LoadChunk(Vect2 Position, GraphicsDevice gpu)
        {

            Chunk c = new Chunk(Position, gpu);

            Stream s = new FileStream("MAP/chunk_" + Position.X + "_" + Position.Y + ".txt", FileMode.OpenOrCreate, FileAccess.Read);
            BinaryFormatter b = new BinaryFormatter();
            try
            {
                c.blocks = (Block[,,])b.Deserialize(s);
            }
            catch (SerializationException e)
            {
                s.Close();
                return c;
            }
            
            s.Close();

            return c;

        }

        public static Task SaveChunk(Vect2 Position)
        {

            Chunk c = Handler.GetChunk(Position);

            Stream s = new FileStream("MAP/chunk_" + Position.X + "_" + Position.Y + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, c.blocks);
            s.Close();

            Console.WriteLine("CHUNK : " + Position.X + "_" + Position.Y + " SAVED !");

            return Task.CompletedTask;
        }


        public static async void SaveChunkAsync(Vect2 Position)
        {
            await SaveChunk(Position);
        }


        public static void SaveChunk2(Vect2 Position)
        {

            Chunk c = Handler.GetChunk(Position);

            Stream s = new FileStream("MAP/chunk_" + Position.X + "_" + Position.Y + ".chunk", FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter writer = new StreamWriter(s);

            for (int k = 0; k < c.blocks.GetLength(2); k++)
            {

                string line = "";

                for (int i = 0; i < c.blocks.GetLength(0); i++)
                {
                    for (int j = 0; j < c.blocks.GetLength(1); j++)
                    {

                        if(c.blocks[i, j, k] != null)
                            line += c.blocks[i, j, k].ID + ",";

                    }
                }

                writer.WriteLine(line);
                writer.WriteLine("");

            }


            writer.Close();
            s.Close();
           

            Console.WriteLine("CHUNK : " + Position.X + "_" + Position.Y + "SAVED !");

        }



    }
}
