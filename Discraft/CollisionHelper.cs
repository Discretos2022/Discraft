using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discraft
{
    public static class CollisionHelper
    {

        public static BlockFace RayBox(Vector3 start, Vector3 end, Vector3 block)
        {

            RectangleBorder resXY = LineRect(new Vector2(start.X, start.Y), new Vector2(end.X, end.Y), new Rectangle((int)block.X, (int)block.Y, 1, 1));
            RectangleBorder resXZ = LineRect(new Vector2(start.X, start.Z), new Vector2(end.X, end.Z), new Rectangle((int)block.X, (int)block.Z, 1, 1));
            RectangleBorder resYZ = LineRect(new Vector2(start.Z, start.Y), new Vector2(end.Z, end.Y), new Rectangle((int)block.Z, (int)block.Y, 1, 1));


            if (resXY == RectangleBorder.None || resXZ == RectangleBorder.None || resYZ == RectangleBorder.None)
                return BlockFace.None;

           if (resXY == RectangleBorder.Left && resXZ == RectangleBorder.Left)
                return BlockFace.Left;
            if (resXY == RectangleBorder.Right && resXZ == RectangleBorder.Right)
                return BlockFace.Right;


            if (resXY == RectangleBorder.Top && resYZ == RectangleBorder.Top)
                return BlockFace.Top;

            if (resXY == RectangleBorder.Bottom && resYZ == RectangleBorder.Bottom)
                return BlockFace.Bottom;


            if (resXZ == RectangleBorder.Bottom && resYZ == RectangleBorder.Left)
                return BlockFace.Front;

            if (resXZ == RectangleBorder.Top && resYZ == RectangleBorder.Right)
                return BlockFace.Back;



            /*if (resXY == RectangleBorder.Top) return BlockFace.Top;
            if (resXY == RectangleBorder.Bottom) return BlockFace.Bottom;
            if (resXY == RectangleBorder.Left) return BlockFace.Left;
            if (resXY == RectangleBorder.Right) return BlockFace.Right;*/

            return BlockFace.None;
        }

        public static RectangleBorder LineRect(Vector2 start, Vector2 end, Rectangle rect)
        {

            Vector2 oldInter = Vector2.Zero;
            Vector2 inter = Vector2.Zero;
            RectangleBorder border = RectangleBorder.None;

            if (LineLine(start, end, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), out inter))
            {
                if (border == RectangleBorder.None) 
                {
                    border = RectangleBorder.Bottom;
                    oldInter = inter;
                }
                else if (Vector2.Distance(start, oldInter) > Vector2.Distance(start, inter))
                {
                    border = RectangleBorder.Bottom;
                    oldInter = inter;
                }
            }

            if (LineLine(start, end, new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), out inter))
            {
                if (border == RectangleBorder.None)
                {
                    border = RectangleBorder.Right;
                    oldInter = inter;
                }
                else if (Vector2.Distance(start, oldInter) > Vector2.Distance(start, inter))
                {
                    border = RectangleBorder.Right;
                    oldInter = inter;
                }
            }

            if (LineLine(start, end, new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), out inter))
            {
                if (border == RectangleBorder.None)
                {
                    border = RectangleBorder.Top;
                    oldInter = inter;
                }
                else if (Vector2.Distance(start, oldInter) > Vector2.Distance(start, inter))
                {
                    border = RectangleBorder.Top;
                    oldInter = inter;
                }
            }

            if (LineLine(start, end, new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y + rect.Height), out inter))
            {
                if (border == RectangleBorder.None)
                {
                    border = RectangleBorder.Left;
                    oldInter = inter;
                }
                else if (Vector2.Distance(start, oldInter) > Vector2.Distance(start, inter))
                {
                    border = RectangleBorder.Left;
                    oldInter = inter;
                }
            }

            if (RectContainsPoint(start, rect) && RectContainsPoint(end, rect))
            {
                border = RectangleBorder.Center;
            }

            return border;
        }

        public static bool LineLine(Vector2 s1, Vector2 e1, Vector2 s2, Vector2 e2, out Vector2 intersectionPoint)
        {

            float x1 = s1.X;
            float x2 = e1.X;
            float x3 = s2.X;
            float x4 = e2.X;

            float y1 = s1.Y;
            float y2 = e1.Y;
            float y3 = s2.Y;
            float y4 = e2.Y;

            // calculate the distance to intersection point
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            intersectionPoint = Vector2.Zero;
            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {

                // optionally, draw a circle where the lines meet
                float intersectionX = x1 + (uA * (x2 - x1));
                float intersectionY = y1 + (uA * (y2 - y1));

                intersectionPoint = new Vector2(intersectionX, intersectionY);

                return true;
            }
            return false;

        }


        public static bool RectContainsPoint(Vector2 pt, Rectangle rect)
        {
            if (pt.X >= rect.X && pt.Y >= rect.Y && pt.X <= rect.X + rect.Width && pt.Y <= rect.Y + rect.Height)
                return true;
            return false;
        }



        public enum RectangleBorder
        {
            None = 0,
            Top = 1,
            Right = 2,
            Bottom = 3,
            Left = 4,
            Center = 5,
        }

        public enum BlockFace
        {
            None = 0,
            Top = 1,
            Bottom = 2,
            Left = 3,
            Right = 4,
            Front = 5,
            Back = 6,
        }


    }
}
