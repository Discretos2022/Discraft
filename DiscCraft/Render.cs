using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DiscCraft
{
    public class Render
    {

        #region 1.1

        public static void DrawLineV1_1(Texture2D texture, Vector2 pos1, Vector2 pos2, SpriteBatch spriteBatch, Color color, int epaisseur = 1, LineType lineType = LineType.Center)
        {

            float distance = Vector2.Distance(pos1, pos2);

            float distanceX = pos2.X - pos1.X;
            float distanceY = pos2.Y - pos1.Y;

            float rotation = (float)Math.Atan2(distanceY, distanceX);

            float centerOfPointOneOfLine = 0f;
            if (lineType == LineType.Center)
                centerOfPointOneOfLine = 0.5f;
            else if(lineType == LineType.Exterior)
                centerOfPointOneOfLine = 1f;
            else if (lineType == LineType.Interior)
                centerOfPointOneOfLine = 0f;

            spriteBatch.Draw(texture, new Rectangle(Util.UpperInteger(pos1.X), Util.UpperInteger(pos1.Y), Util.UpperInteger(distance), epaisseur), null, color, rotation, new Vector2(0, centerOfPointOneOfLine), SpriteEffects.None, 0f);

        }

        public static void DrawRectangleV1_1(Texture2D texture, Rectangle rectangle, SpriteBatch spriteBatch, Color color, int epaisseur = 1)
        {
            int correctionBog = 0;
            if (!Util.IsMultiple((float)epaisseur, 2))
                correctionBog = 1;

            /// LineType = Center       Fonctionnel
            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y), spriteBatch, color, epaisseur); // - up
            DrawLineV1_1(texture, new Vector2(rectangle.X + correctionBog, rectangle.Y), new Vector2(rectangle.X + correctionBog, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur); // | ?

            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur); // - down
            DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur); // |  ?
            ///

            /// LineType = Interior     Fonctionnel
            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), spriteBatch, color, epaisseur, LineType.Interior); // - up
            //DrawLineV1_1(texture, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Interior); // | ?

            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Interior); // - down
            //DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Interior); // |  ?
            ///

            /// LineType = Exterior     Not Fonctionnel
            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y), spriteBatch, color, epaisseur, LineType.Exterior); // - up
            //DrawLineV1_1(texture, new Vector2(rectangle.X + correctionBog, rectangle.Y), new Vector2(rectangle.X + correctionBog, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Exterior); // | ?

            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Exterior); // - down
            //DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Exterior); // |  ?
            ///


        }


        public static void DrawRectangleV1_1(Texture2D texture, Rectangle rectangle, SpriteBatch spriteBatch, Color color1, Color color2, Color color3, Color color4, int epaisseur = 1)
        {

            int correctionBog = 0;
            if (!Util.IsMultiple((float)epaisseur, 2))
                correctionBog = 1;

            DrawLineV1_1(texture, new Vector2(rectangle.X + correctionBog, rectangle.Y), new Vector2(rectangle.X + correctionBog, rectangle.Y + rectangle.Height), spriteBatch, color2, epaisseur); // |  left
            DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color4, epaisseur); // |  right

            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y), spriteBatch, color1, epaisseur); // - up
            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y + rectangle.Height), spriteBatch, color3, epaisseur); // - down
            
        }


        public enum LineType
        {
            Center = 0,
            Interior = 1,
            Exterior = 2,
        };

        #endregion



    }
}
