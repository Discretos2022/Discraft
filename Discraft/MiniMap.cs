using DiscCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscCraft_2
{
    public class MiniMap
    {

        static Color color = new Color(Random.Shared.Next(0, 255), Random.Shared.Next(0, 255), Random.Shared.Next(0, 255));

        public static void Draw(SpriteBatch spriteBatch)
        {


            int baseX = 800;
            int baseY = 200;


            /*for (int i = 0; i < Handler.chunks.Count; i++)
            {

                KeyValuePair<Vect2, Chunk> chunk = Handler.chunks.ElementAt(i);
                color = new Color(Random.Shared.Next(0, 255), Random.Shared.Next(0, 255), Random.Shared.Next(0, 255));

                if (Handler.chunks.ContainsKey(chunk.Key))
                {
                    if (Handler.chunks[chunk.Key].isDrawed)
                    {
                        if (chunk.Key.X % 2 == 0 && chunk.Key.Y % 2 != 0)
                            spriteBatch.Draw(Main.Bounds, new Rectangle((int)chunk.Key.X * 16 + baseX - (int)Main.cameraV2.Position.X, (int)chunk.Key.Y * 16 + baseY - (int)Main.cameraV2.Position.Z, 16, 16), Color.Green);
                        else if (chunk.Key.X % 2 != 0 && chunk.Key.Y % 2 == 0)
                            spriteBatch.Draw(Main.Bounds, new Rectangle((int)chunk.Key.X * 16 + baseX - (int)Main.cameraV2.Position.X, (int)chunk.Key.Y * 16 + baseY - (int)Main.cameraV2.Position.Z, 16, 16), Color.Green);
                        else
                            spriteBatch.Draw(Main.Bounds, new Rectangle((int)chunk.Key.X * 16 + baseX - (int)Main.cameraV2.Position.X, (int)chunk.Key.Y * 16 + baseY - (int)Main.cameraV2.Position.Z, 16, 16), Color.DarkGreen);
                    }
                    else
                    {
                        if (chunk.Key.X % 2 == 0 && chunk.Key.Y % 2 != 0)
                            spriteBatch.Draw(Main.Bounds, new Rectangle((int)chunk.Key.X * 16 + baseX - (int)Main.cameraV2.Position.X, (int)chunk.Key.Y * 16 + baseY - (int)Main.cameraV2.Position.Z, 16, 16), Color.Red);
                        else if (chunk.Key.X % 2 != 0 && chunk.Key.Y % 2 == 0)
                            spriteBatch.Draw(Main.Bounds, new Rectangle((int)chunk.Key.X * 16 + baseX - (int)Main.cameraV2.Position.X, (int)chunk.Key.Y * 16 + baseY - (int)Main.cameraV2.Position.Z, 16, 16), Color.Red);
                        else
                            spriteBatch.Draw(Main.Bounds, new Rectangle((int)chunk.Key.X * 16 + baseX - (int)Main.cameraV2.Position.X, (int)chunk.Key.Y * 16 + baseY - (int)Main.cameraV2.Position.Z, 16, 16), Color.DarkRed);
                    }
                }
                    


            }*/

            //Render.DrawLineV1_1(Main.Bounds, new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z), new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z), spriteBatch, Color.Red, 4, Render.LineType.Center);


            //Vector2 point = (new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z) - new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) * 100 + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z);

            ///Vector2 point = (new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z) - new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) * 120 + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z);


            //Render.DrawLineV1_1(Main.Bounds, new Vector2(Main.cameraV2.Position.X + 400, Main.cameraV2.Position.Z + 16), point, spriteBatch, Color.Red, 4, Render.LineType.Center);
            //Render.DrawLineV1_1(Main.Bounds, new Vector2(Main.cameraV2.Position.X + 400, Main.cameraV2.Position.Z + 16), new Vector2(Main.cameraV2.Position.X + 400, Main.cameraV2.Position.Z + 16), spriteBatch, Color.Red, 4, Render.LineType.Center);

            //spriteBatch.Draw(Main.Bounds, new Rectangle((int)Main.cameraV2.Position.X + 400 - 4, (int)Main.cameraV2.Position.Z + 4, 8, 8), Color.Yellow);
            spriteBatch.Draw(Main.Bounds, new Rectangle(baseX - 4, baseY - 4, 8, 8), Color.Yellow);

            ///spriteBatch.Draw(Main.Circle, point + new Vector2(400, 16), null, Color.Red, 0f, new Vector2(32, 32), 4f, SpriteEffects.None, 0f);

        }

    }
}
