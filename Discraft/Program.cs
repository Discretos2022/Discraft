
using DiscCraft_2;
using Discraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//Vector3 start = new Vector3(0.5f, -10, 0.5f);
//Vector3 end = new Vector3(0.5f, 2, 0.5f);

/*Vector3 start = new Vector3(-10, 2, 0.5f);
Vector3 end = new Vector3(10, 2, 0.5f);

Vector3 b = new Vector3(0,0,0);


Console.WriteLine(CollisionHelper.RayBox(start, end, b));*/


/*Vector2 pt1 = new Vector2(0, 0);
Vector2 pt2 = new Vector2(2, 2);
Vector2 pt3 = new Vector2(0, 2);
Vector2 pt4 = new Vector2(0, 1);

Vector2 pt5 = Vector2.Zero;

var res = CollisionHelper.LineLine(pt1, pt2, pt3, pt4, out pt5);

Console.WriteLine($"Result : {res} -> {pt5}");*/

//Console.WriteLine(sizeof(VertexPositionNormalTexture));

/*Vector3 blockCoord = new Vector3(-16, 0, 0);
int X = (int)(((blockCoord.X / 16) - MathUtils.RoundLower(blockCoord.X / 16)) * 16);
int Z = (int)(((blockCoord.Z / 16) - MathUtils.RoundLower(blockCoord.Z / 16)) * 16);

Vect2 chunkCoord = new Vect2(MathUtils.RoundLower(blockCoord.X / 16), MathUtils.RoundLower(blockCoord.Z / 16));

if (blockCoord.X < 0 && (int)(blockCoord.X / 16) == (blockCoord.X / 16)) chunkCoord.X += 1;
if (blockCoord.Z < 0 && (int)(blockCoord.Z / 16) == (blockCoord.Z / 16)) chunkCoord.Y += 1;

Console.WriteLine(X + " ; " + Z);
Console.WriteLine(chunkCoord.X + " ; " + chunkCoord.Y);*/


/*Stopwatch sw = new Stopwatch();

int[] tab = new int[20000];

for (int i = 0; i < tab.Length; i++)
{
    tab[i] = i;
}

sw.Start();

tab.ToArray();

Console.WriteLine("ELAPSED TIME : " + sw.ElapsedMilliseconds);*/


/*Vector2 start = new Vector2 (0, 0);
Vector2 end = new Vector2 (6, 8);

Rectangle rect = new Rectangle(5,5,2,2);

Console.WriteLine(CollisionHelper.LineRect(start, end, rect));*/



using var game = new DiscCraft.Main();
game.Run();
