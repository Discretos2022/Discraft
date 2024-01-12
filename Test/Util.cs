using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectangleHSB
{
    class Util
    {

        public static Random random = new Random();


        public static int UpperInteger(float value)
        {
            if (value == (int)value)
                return (int)value;
            if (value < 0)
                return (int)value - 1;
            if (value > 0)
                return (int)value + 1;
            if (value == 0)
                return 0;

            return 0;

        }

        public static int UpperIntegerPlus(float value)
        {
            if (value == (int)value)
                return (int)value;
            if (value < 0)
                return (int)value - 1;
            if (value > 0)
                return (int)value;
            if (value == 0)
                return 0;

            return 0;

        }


        public static bool IsMultiple(int num, int multiple)
        {
            if ((num / multiple) == (int)(num / multiple))
                return true;

            return false;
        }

        /// <summary>
        /// version 1.1
        /// </summary>
        /// <param name="num"></param>
        /// <param name="multiple"></param>
        /// <returns></returns>
        public static bool IsMultiple(float num, float multiple)
        {
            if ((num / multiple) == (int)(num / multiple))
                return true;

            return false;
        }


        public static int Clamp(int value, int min, int max)
        {

            if(min > max)
            {
                throw new ArgumentOutOfRangeException("La valeur de \"min\" est superieur  à la valeur de \"max\".");
            }



            if(value < min)
            {
                return min;
            }
            else if(value > max)
            {
                return max;
            }else
            {
                return value;
            }
        }


        public static float Clamp(float value, float min, float max)
        {

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("La valeur de \"min\" est superieur  à la valeur de \"max\".");
            }



            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }


        public static decimal Clamp(decimal value, decimal min, decimal max)
        {

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("La valeur de \"min\" est superieur  à la valeur de \"max\".");
            }



            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }


        public static float NextFloat(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }


        public static int GetNumUnderPoint(float num, int numUnderPoint)
        {
            //double val = num - (Math.Round(num, numUnderPoint, MidpointRounding.ToZero) - (int)num) - (Math.Round(num, numUnderPoint + 2, MidpointRounding.ToZero) - (int)num);

            //double val = num - (int)num;

            //val = val * Math.Pow(10, numUnderPoint);

            if (numUnderPoint > 6)
                throw new ArgumentOutOfRangeException("NumUnderPoint is superior of 6");


            double val = num;

            for (int i = 0; i < numUnderPoint; i++)
            {
                val = val - (int)val;

                val *= 10;

            }


            return (int)val;
        }


        public static Color GetHSBColor(double valeur)
        {

            double r = 255;
            double g = 0;
            double b = 0;

            valeur += 255;

            //valeur = valeur * 1 / 7 * 255;


            // 0-255
            if (valeur < 255)
                r = valeur;
            else
            {
                r = 255;
                valeur -= 255;



                // 255-550
                if (valeur < 255)
                    g = valeur;
                else
                {
                    g = 255;
                    valeur -= 255;



                    // 550-765
                    if (valeur < 255)
                        r -= valeur;
                    else
                    {
                        r = 0;
                        valeur -= 255;


                        // ///////////////////////////////
                        if(valeur < 255)
                            b = valeur;
                        else//////          fonctionnel
                        {
                            b = 255;
                            valeur -= 255;

                            
                            /////////////////////
                            if(valeur < 255)
                                g -= valeur;
                            else
                            {
                                g = 0;
                                valeur -= 255;

                                if (valeur < 255)
                                    r = valeur;
                                else
                                {

                                    r = 255;
                                    valeur -= 255;

                                    if (valeur < 255)
                                        b -= valeur;
                                    else
                                    {
                                        b = 0;
                                        valeur -= 255;

                                        //if (valeur < 255)
                                        //    g = valeur;
                                        //else
                                        //{
                                        //    g = 255;
                                        //    valeur -= 255;

                                        //    if (valeur < 255)
                                        //        r -= valeur;
                                        //    else
                                        //    {
                                        //        r = 0;
                                        //        valeur -= 255;
                                        //    }


                                        //}

                                    }
                                }


                                
                                

                            }


                        }

                    }
                        


                }

            }
            



            


            return new Color((float)r / 255, (float)g / 255, (float)b / 255);

        }

    }
}
