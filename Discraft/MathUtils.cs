using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscCraft_2
{
    public class MathUtils
    {

        public static int RoundLower(float num)
        {
            if (num >= 0) return (int)num;
            else return (int)num - 1;
        }

    }


    [Serializable]
    public struct Vect2 : IEquatable<Vect2>
    {
        public int X;
        public int Y;

        public Vect2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vect2 operator +(Vect2 vect1, Vect2 vect2)
        {
            return new Vect2(vect1.X + vect2.X, vect1.Y + vect2.Y);
        }

        public static Vect2 operator -(Vect2 vect1, Vect2 vect2)
        {
            return new Vect2(vect1.X - vect2.X, vect1.Y - vect2.Y);
        }

        public static Vect2 operator *(Vect2 vect1, Vect2 vect2)
        {
            return new Vect2(vect1.X * vect2.X, vect1.Y * vect2.Y);
        }

        public static Vect2 operator *(Vect2 vect1, int num)
        {
            return new Vect2(vect1.X * num, vect1.Y * num);
        }

        public static Vect2 operator /(Vect2 vect1, Vect2 vect2)
        {
            return new Vect2(vect1.X / vect2.X, vect1.Y / vect2.Y);
        }

        public static Vect2 operator /(Vect2 vect1, int num)
        {
            return new Vect2(vect1.X / num, vect1.Y / num);
        }

        public bool Equals(Vect2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Vect2 vect1, Vect2 vect2)
        {
            return vect1.X == vect2.X && vect1.Y == vect2.Y;
        }

        public static bool operator !=(Vect2 vect1, Vect2 vect2)
        {
            return vect1.X != vect2.X || vect1.Y != vect2.Y;
        }

        public static double Distance(Vect2 v1, Vect2 v2)
        {
            return Math.Sqrt((v2.X - v1.X) * (v2.X - v1.X) + (v2.Y - v1.Y) * (v2.Y - v1.Y));
        }

    }

}
