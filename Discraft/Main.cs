using DiscCraft_2;
using Discraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;


/**
 * 
 *  Copyright (c) SIEDEL 2024
 *
 *  💿
 * 
 *
**/

namespace DiscCraft
{
    public class Main : Game
    {
        public static long vertexNum = 0;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Camera
        Vector3 camTarget;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //BasicEffect for rendering
        BasicEffect basicEffect;

        public static Camera cameraV2;

        public static bool cameraActived = true;

        public static Texture2D Cursor;

        public static Texture2D GrassSide;
        public static Texture2D GrassUp;
        public static Texture2D GrassDown;

        public static Texture2D BlockSheet;

        public static Texture2D Bounds;
        public static Texture2D Circle;

        public static Vector2 cp;
        public static Vector2 cn;

        public static SpriteFont UltimateFont = null;      //= new SpriteFont(null, null, null, null, 0, 0, null, null);
        Texture2D SuperFont;
        List<Rectangle> glyphRect = new List<Rectangle>();
        List<Rectangle> croppingList = new List<Rectangle>();
        List<char> charList = new List<char>();
        List<Vector3> Vector3List = new List<Vector3>();


        Stopwatch stop = new Stopwatch();


        //public Block block;

        public Chunk chunk;
        public Chunk chunk2;
        public Chunk chunk3;
        public Chunk chunk4;

        public Chunk chunk5;
        public Chunk chunk6;
        public Chunk chunk7;
        public Chunk chunk8;

        public int tick = 0;



        public static int VERTEX = 0;
        public static int TRIANGLES = 0;


        public Stopwatch FpsTime;
        public float fpsTime;

        public static Ray playerRay;

        public static int drawedChunk = 0;


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f); // 30


            IsFixedTimeStep = true;


            graphics.PreferredBackBufferWidth = 1920 / 2;      //(1920/5) * 5;      //500;         640* 360
            graphics.PreferredBackBufferHeight = 1080 / 2;     //(1080/5) * 5;     //250;


            Window.AllowUserResizing = true;    


            graphics.IsFullScreen = false;
            graphics.HardwareModeSwitch = false;

        }

        protected override void Initialize()
        {

            cameraV2 = new Camera(this, new Vector3(0,40,0), new Vector3(0,0,0), 0.2f); // 0.5f


            //Setup Camera
            worldMatrix = Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.Forward, Vector3.Up);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            projectionMatrix = cameraV2.Projection;
            viewMatrix = cameraV2.View;

            FpsTime = new Stopwatch();

            RPC.Connect();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GrassSide = Content.Load<Texture2D>("Images\\Tile_1");
            GrassUp = Content.Load<Texture2D>("Images\\Tile_2");
            GrassDown = Content.Load<Texture2D>("Images\\Tile_3");

            //BlockSheet = Content.Load<Texture2D>("Images\\Block_1");

            BlockSheet = Content.Load<Texture2D>("Images\\BlockSheet");

            //block = new Block(new Vector3(0,0,0), GraphicsDevice, new Vector3(1,1,1));

            //chunk  = new Chunk(new Vector2(-1, -1), GraphicsDevice);
            //chunk2 = new Chunk(new Vector2(-1, 0), GraphicsDevice);
            //chunk3 = new Chunk(new Vector2(0, -1), GraphicsDevice);
            //chunk4 = new Chunk(new Vector2(0, 0), GraphicsDevice);

            //chunk5 = new Chunk(new Vector2(2, 2), GraphicsDevice);
            //chunk6 = new Chunk(new Vector2(-3, 2), GraphicsDevice);
            //chunk7 = new Chunk(new Vector2(2, -3), GraphicsDevice);
            //chunk8 = new Chunk(new Vector2(-3, -3), GraphicsDevice);


            Handler.Init(GraphicsDevice);
            //SerializeNow(Handler.GetChunk(new Vect2(0, 0)));

            //ChunkLoader.SaveChunkAsync(new Vect2(0, 0));
            //ChunkLoader.SaveChunk2(new Vect2(0, 0));


            Cursor = Content.Load<Texture2D>("Images\\Cursor");

            Bounds = Content.Load<Texture2D>("Images\\Bounds");
            Circle = Content.Load<Texture2D>("Images\\Circle");

            SuperFont = Content.Load<Texture2D>("Images\\SuperFont");
            InitFont();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyInput.Update();
            MouseInput.Update();


            //Console.WriteLine("Vertex : " + vertexNum);

            //Console.WriteLine("CAMERA ROT : " + cameraV2.Rotation);

            if(KeyInput.getKeyState().IsKeyDown(Keys.P) && !KeyInput.getOldKeyState().IsKeyDown(Keys.P))
            {
                if (cameraActived)
                {
                    cameraActived = false;
                    IsMouseVisible = true;
                }
                else
                {
                    cameraActived = true;
                    IsMouseVisible = false;
                }

            }

            if (KeyInput.getKeyState().IsKeyDown(Keys.F11) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F11))
            {
                if (graphics.IsFullScreen)
                    graphics.IsFullScreen = false;
                else
                    graphics.IsFullScreen = true;

                graphics.ApplyChanges();

            }


            projectionMatrix = cameraV2.Projection;
            viewMatrix = cameraV2.View;

            cameraV2.Update();
            cameraV2.UpdateLookAt();

            projectionMatrix = cameraV2.Projection;
            viewMatrix = cameraV2.View;


            if (KeyInput.getKeyState().IsKeyDown(Keys.F5) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F5))
            {

                for (int i = 0; i < Handler.chunks.Count; i++)
                {

                    //Vector2 point = (new Vector2(Main.cameraV2.cameraLookAt.X, Main.cameraV2.cameraLookAt.Z) - new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z)) * 120 + new Vector2(Main.cameraV2.Position.X, Main.cameraV2.Position.Z);

                    KeyValuePair<Vect2, Chunk> chunk = Handler.chunks.ElementAt(i);

                    ChunkLoader.SaveChunk(chunk.Key);
                    Console.WriteLine(i);


                }

            }

            tick += 1;

            if(tick == 240)
            {
                Handler.Update(gameTime, GraphicsDevice);
                tick = 0;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(0, 0, 0.2f));

            FpsTime.Stop();
            fpsTime = ((float)FpsTime.Elapsed.TotalMilliseconds);
            FpsTime.Restart();

            stop.Start();

            GraphicsDevice.Clear(Color.Black);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;


            //GraphicsDevice.SetVertexBuffer(vertexBuffer);

            basicEffect.TextureEnabled = true;


            //Turn off culling so we see both sides of our rendered triangle

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //Handler.Draw(cameraV2, GraphicsDevice, basicEffect);

            bool lightEnable = true;
            if (lightEnable) {
                basicEffect.EnableDefaultLighting();   // make sure lighting is on
                basicEffect.LightingEnabled = true;
                basicEffect.PreferPerPixelLighting = true;
                basicEffect.AmbientLightColor = new Vector3(0.1f, 0.2f, 0.3f);    // medium-dark for dark parts of object
                basicEffect.DiffuseColor = new Vector3(0.94f, 0.94f, 0.94f); // fairly bright for lit parts of object
                basicEffect.TextureEnabled = true;  // make sure this is enabled
                basicEffect.EmissiveColor = new Vector3(0.0f, 0.0f, 0.0f);
                basicEffect.PreferPerPixelLighting = false;
                basicEffect.SpecularColor = new Vector3(1, 1, 1);
                //basicEffect.SpecularPower = 20f;
                basicEffect.DiffuseColor = new Vector3(1, 1, 1); //basicEffect.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f);
                basicEffect.FogEnabled = true;
                basicEffect.FogStart = 256f; //80f
                basicEffect.FogEnd = 306f; //130f
                basicEffect.FogColor = new Vector3(0.3f,0.3f,0.3f);
            }
            else
                basicEffect.LightingEnabled = false;

            basicEffect.Alpha = 1f;

            basicEffect.VertexColorEnabled = false;

            //if(Handler.chunks.ContainsKey(new Vector2(0, 0)))
                //Console.WriteLine(Handler.chunks[new Vector2(0, 0)].vertexCount);

            //if(Vector2.Distance(chunk.Position, new Vector2(cameraV2.Position.X, cameraV2.Position.Y)) < 100)
            //chunk.Draw(GraphicsDevice, basicEffect);
            //chunk2.Draw(GraphicsDevice, basicEffect);
            //chunk3.Draw(GraphicsDevice, basicEffect);
            //chunk4.Draw(GraphicsDevice, basicEffect);

            //chunk5.Draw(GraphicsDevice, basicEffect);
            //chunk6.Draw(GraphicsDevice, basicEffect);
            //chunk7.Draw(GraphicsDevice, basicEffect);
            //chunk8.Draw(GraphicsDevice, basicEffect);




            Handler.Draw(GraphicsDevice, basicEffect, cameraV2);




            int range = 10;

            Vector3 cursorOrigine = cameraV2.Position + new Vector3(0, 0, 0); // 1
            Vector3 cursorEnd = new Vector3(range, 0, 0);

            /// Build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraV2.cameraRotation.X) * Matrix.CreateRotationY(cameraV2.cameraRotation.Y);

            /// Calculate end of vision
            cursorEnd = Vector3.Transform(new Vector3(0, 0, range), rotationMatrix);

            Vector3 selectPos = new Vector3(0, 255, 0);
            CollisionHelper.BlockFace face = CollisionHelper.BlockFace.None;


            /*if (Handler.chunks.ContainsKey(new Vect2(0, 0)))
            {

                for (int x = 0; x < Handler.chunks[new Vect2(0, 0)].blocks.GetLength(0); x++)
                    for (int y = 0; y < Handler.chunks[new Vect2(0, 0)].blocks.GetLength(1); y++)
                        for (int z = 0; z < Handler.chunks[new Vect2(0, 0)].blocks.GetLength(2); z++)
                        {
                            if(Handler.chunks[new Vect2(0, 0)].blocks[x, y, z] != null)
                            {
                                CollisionHelper.BlockFace res = CollisionHelper.RayBox(cursorOrigine, cursorOrigine + cursorEnd, new Vector3(x, y, z));

                                if (res != CollisionHelper.BlockFace.None)
                                {
                                    if (selectPos.Y == 255 || Vector3.Distance(selectPos + new Vector3(0.5f, 0.5f, 0.5f), cursorOrigine) > Vector3.Distance(new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f), cursorOrigine))
                                    {
                                        selectPos = new Vector3(x, y, z);
                                        //Console.WriteLine($"Collision {x};{y};{z} -> {res}");
                                    }
                                    
                                }
                            }
                            

                        }

            }*/



            for (int x = (int)cursorOrigine.X - range; x < (int)cursorOrigine.X + range; x++)
            {
                for (int y = (int)cursorOrigine.Y - range; y < (int)cursorOrigine.Y + range; y++)
                {
                    for (int z = (int)cursorOrigine.Z - range; z < (int)cursorOrigine.Z + range; z++)
                    {
                        Vector3 v = new Vector3(x, y, z);

                        Chunk c = Handler.GetChunkWithBlockCoord(v);

                        if (c != null)
                        {

                            Vector3 bCoord = Handler.GetBlockCoordInChunk(v);

                            if (c.blocks[(int)bCoord.X, (int)bCoord.Y, (int)bCoord.Z] != null)
                            {

                                CollisionHelper.BlockFace res = CollisionHelper.RayBox(cursorOrigine, cursorOrigine + cursorEnd, v);

                                if (res != CollisionHelper.BlockFace.None)
                                {
                                    if (selectPos.Y == 255 || Vector3.Distance(selectPos + new Vector3(0.5f, 0.5f, 0.5f), cursorOrigine) > Vector3.Distance(v + new Vector3(0.5f, 0.5f, 0.5f), cursorOrigine))
                                    {
                                        selectPos = v;
                                        face = res;

                                        //Console.WriteLine($"Collision {x};{y};{z} -> {res}");
                                    }

                                }
                            }

                        }

                    }
                }
            }





            if (MouseInput.getMouseState().LeftButton == ButtonState.Pressed && MouseInput.getOldMouseState().LeftButton != ButtonState.Pressed)
            {
                if(selectPos.Y != 255)
                {

                    Chunk c = Handler.GetChunkWithBlockCoord(selectPos);

                    if (c != null)
                    {

                        Vector3 bCoord = Handler.GetBlockCoordInChunk(selectPos);

                        c.blocks[(int)bCoord.X, (int)bCoord.Y, (int)bCoord.Z] = null;

                        Handler.UpdateChunkVertexTask(c.Position/16, GraphicsDevice);

                    }
                }
            }

            if (MouseInput.getMouseState().RightButton == ButtonState.Pressed && MouseInput.getOldMouseState().RightButton != ButtonState.Pressed)
            {
                if (selectPos.Y != 255)
                {

                    Chunk c = Handler.GetChunkWithBlockCoord(selectPos);

                    if (c != null)
                    {
                        Vector3 a = Vector3.Zero;
                        if (face == CollisionHelper.BlockFace.Right)
                            a = new Vector3(1, 0, 0);
                        else if (face == CollisionHelper.BlockFace.Left)
                            a = new Vector3(-1, 0, 0);
                        else if (face == CollisionHelper.BlockFace.Top)
                            a = new Vector3(0, 1, 0);
                        else if (face == CollisionHelper.BlockFace.Bottom)
                            a = new Vector3(0, -1, 0);
                        else if (face == CollisionHelper.BlockFace.Front)
                            a = new Vector3(0, 0, -1);
                        else if (face == CollisionHelper.BlockFace.Back)
                            a = new Vector3(0, 0, 1);

                        Chunk ca = Handler.GetChunkWithBlockCoord(selectPos + a);
                        Vector3 bCoord = Handler.GetBlockCoordInChunk(selectPos + a);

                        ca.blocks[(int)bCoord.X, (int)bCoord.Y, (int)bCoord.Z] = new Block(3);
                        Handler.UpdateChunkVertexTask(c.Position / 16, GraphicsDevice);


                    }
                }
            }


            //Console.WriteLine(face);

            VertexPositionColor[] vertices = new VertexPositionColor[9];

            vertices[0] = new VertexPositionColor(selectPos + new Vector3(-0.01f, -0.01f, -0.01f), Color.Black);
            vertices[1] = new VertexPositionColor(selectPos + new Vector3(1.01f, -0.01f, -0.01f), Color.Black);
            vertices[2] = new VertexPositionColor(selectPos + new Vector3(-0.01f, 1.01f, -0.01f), Color.Black);
            vertices[3] = new VertexPositionColor(selectPos + new Vector3(1.01f, 1.01f, -0.01f), Color.Black);
            vertices[4] = new VertexPositionColor(selectPos + new Vector3(-0.01f, -0.01f, 1.01f), Color.Black);
            vertices[5] = new VertexPositionColor(selectPos + new Vector3(1, -0.01f, 1.01f), Color.Black);
            vertices[6] = new VertexPositionColor(selectPos + new Vector3(-0.01f, 1.01f, 1.01f), Color.Black);
            vertices[7] = new VertexPositionColor(selectPos + new Vector3(1.01f, 1.01f, 1.01f), Color.Black);

            VertexBuffer vertexBuffer;
            IndexBuffer indexBuffer;

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
            short[] indices = new short[36];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;

            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            indices[6] = 1;
            indices[7] = 5;
            indices[8] = 3;

            indices[9] = 5;
            indices[10] = 7;
            indices[11] = 3;

            indices[12] = 5;
            indices[13] = 4;
            indices[14] = 7;

            indices[15] = 4;
            indices[16] = 6;
            indices[17] = 7;

            indices[18] = 4;
            indices[19] = 0;
            indices[20] = 6;

            indices[21] = 0;
            indices[22] = 2;
            indices[23] = 6;

            indices[24] = 2;
            indices[25] = 3;
            indices[26] = 6;

            indices[27] = 3;
            indices[28] = 7;
            indices[29] = 6;

            indices[30] = 0;
            indices[31] = 1;
            indices[32] = 4;

            indices[33] = 1;
            indices[34] = 5;
            indices[35] = 4;

            indexBuffer = new IndexBuffer(graphics.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;
            basicEffect.TextureEnabled = false;  // make sure this is enabled
            basicEffect.VertexColorEnabled = true;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }







            //Block b = new Block(new Vector3(0, 0, 0), 4) + new Block(Vector3.One, 8);

            //Console.WriteLine(b.ToString());


            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.DrawString(UltimateFont, "position x : " + cameraV2.Position.X, new Vector2(10,10), Color.White);
            spriteBatch.DrawString(UltimateFont, "position y : " + cameraV2.Position.Y, new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(UltimateFont, "position z : " + cameraV2.Position.Z, new Vector2(10, 50), Color.White);

            spriteBatch.DrawString(UltimateFont, "rotation x : " + cameraV2.cameraLookAt.X, new Vector2(10, 100), Color.White);
            spriteBatch.DrawString(UltimateFont, "rotation y : " + cameraV2.cameraLookAt.Y, new Vector2(10, 120), Color.White);
            spriteBatch.DrawString(UltimateFont, "rotation z : " + cameraV2.cameraLookAt.Z, new Vector2(10, 140), Color.White);

            spriteBatch.DrawString(UltimateFont, "view origine : " + cameraV2.Position, new Vector2(10, 190), Color.White);

            spriteBatch.DrawString(UltimateFont, "chunk : " + (int)(cameraV2.Position.X / 16) + ":" + (int)(cameraV2.Position.Z / 16), new Vector2(10, 210), Color.White);
            spriteBatch.DrawString(UltimateFont, "drawed chunk : " + drawedChunk + " : " + Handler.chunkBufferQueue.Count, new Vector2(10, 235), Color.White);


            MiniMap.Draw(spriteBatch);


            Process process = Process.GetCurrentProcess();
            //Console.WriteLine(process.NonpagedSystemMemorySize64);
            //Console.WriteLine(process.VirtualMemorySize64);
            //Console.WriteLine(process.PagedMemorySize64);

            spriteBatch.DrawString(UltimateFont, "memory : " + (((int)process.WorkingSet64 / 8) / 1024 / 1024), new Vector2(10, 260), Color.White);
            
            
            spriteBatch.DrawString(UltimateFont, "vertex : " + VERTEX, new Vector2(10, 300), Color.White);
            spriteBatch.DrawString(UltimateFont, "triangle : " + TRIANGLES, new Vector2(10, 320), Color.White);

            spriteBatch.DrawString(UltimateFont, "fps : " + Math.Round(1.0f / (fpsTime / 1000)), new Vector2(10, 400), Color.White);


            spriteBatch.DrawString(UltimateFont, "ready to update : " + Handler.chunkStack.Count, new Vector2(10, 500), Color.White);
            spriteBatch.DrawString(UltimateFont, "loaded chunks   : " + Handler.chunks.Count, new Vector2(10, 520), Color.White);




            spriteBatch.Draw(Cursor, new Vector2(GraphicsDevice.Viewport.Width / 2 - 16 / 2, GraphicsDevice.Viewport.Height / 2 - 16 / 2), Color.White);

            spriteBatch.End();



            stop.Stop();

            //Console.WriteLine("Time : " + stop.ElapsedMilliseconds);

            stop.Reset();

            base.Draw(gameTime);
        }

        public static bool RayVsRect(Ray ray, BoundingBox box)
        {

            float t0x = (box.Min.X - ray.Position.X) / ray.Direction.X;
            float t0y = (box.Min.Y - ray.Position.Y) / ray.Direction.Y;
            float tmin = (t0x > t0y) ? t0x : t0y;

            float t1x = (box.Max.X - ray.Position.X) / ray.Direction.X;
            float t1y = (box.Max.Y - ray.Position.Y) / ray.Direction.Y;
            float tmax = (t1x < t1y) ? t1x : t1y;

            if (t0x > t1y || t0y > t1x) return false;

            float t0z = (box.Min.Z - ray.Position.Z) / ray.Direction.Z;
            float t1z = (box.Max.Z - ray.Position.Z) / ray.Direction.Z;
            if (tmin > t1z || t0z > tmax)
                return false;
            if (t0z > tmin) tmin = t0z;
            if (t1z < tmax) tmax = t1z;

            return true;

        }

        public static bool RayVsRect2(Vector2 ray_origin, Vector2 ray_dir, Rectangle target, Vector2 contact_point, Vector2 contact_normal, float t_hit_near, SpriteBatch _spriteBatch)
        {
            
                Vector2 t_near = (new Vector2(target.X, target.Y) - ray_origin) / ray_dir;
                Vector2 t_far = (new Vector2(target.X, target.Y) + new Vector2(target.Width, target.Height) - ray_origin) / ray_dir;

                //if (t_near.X > t_far.X) throw new ArgumentOutOfRangeException("ERROR");
                //if (t_near.Y > t_far.Y) throw new ArgumentOutOfRangeException("ERROR");


                if (t_near.X > t_far.Y || t_near.Y > t_far.X) return false;

                t_hit_near = Math.Max(t_near.X, t_near.Y);
                float t_hit_far = Math.Min(t_far.X, t_far.Y);

                if (t_hit_far < 0) return false;


                contact_point = ray_origin + t_hit_near * ray_dir;

                if (t_near.X > t_near.Y)
                    if (ray_dir.X < 0)
                        contact_normal = new Vector2(1, 0);
                    else
                        contact_normal = new Vector2(-1, 0);
                else if (t_near.X < t_near.Y)
                    if (ray_dir.Y < 0)
                        contact_normal = new Vector2(0, 1);
                    else
                        contact_normal = new Vector2(0, -1);


                if (t_hit_near >= 1.0f)
                    return false;

                cp = contact_point;
                cn = contact_normal;

                return true;

        }

        bool lineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {

            // calculate the distance to intersection point
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {

                // optionally, draw a circle where the lines meet
                float intersectionX = x1 + (uA * (x2 - x1));
                float intersectionY = y1 + (uA * (y2 - y1));

                cp.X = intersectionX;
                cp.Y = intersectionY;

                return true;
            }
            return false;
        }


        public static bool RayIntersectsBox(Ray ray, BoundingBox box, out float distance)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 179

            distance = 0f;
            float tmax = float.MaxValue;

            if (ray.Direction.X == 0)
            {
                if (ray.Position.X < box.Min.X || ray.Position.X > box.Max.X)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                float inverse = 1.0f / ray.Direction.X;
                float t1 = (box.Min.X - ray.Position.X) * inverse;
                float t2 = (box.Max.X - ray.Position.X) * inverse;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                distance = Math.Max(t1, distance);
                tmax = Math.Min(t2, tmax);

                if (distance > tmax)
                {
                    distance = 0f;
                    return false;
                }
            }

            if (ray.Direction.Y == 0)
            {
                if (ray.Position.Y < box.Min.Y || ray.Position.Y > box.Max.Y)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                float inverse = 1.0f / ray.Direction.Y;
                float t1 = (box.Min.Y - ray.Position.Y) * inverse;
                float t2 = (box.Max.Y - ray.Position.Y) * inverse;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                distance = Math.Max(t1, distance);
                tmax = Math.Min(t2, tmax);

                if (distance > tmax)
                {
                    distance = 0f;
                    return false;
                }
            }

            if (ray.Direction.Z == 0)
            {
                if (ray.Position.Z < box.Min.Z || ray.Position.Z > box.Max.Z)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                float inverse = 1.0f / ray.Direction.Z;
                float t1 = (box.Min.Z - ray.Position.Z) * inverse;
                float t2 = (box.Max.Z - ray.Position.Z) * inverse;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                distance = Math.Max(t1, distance);
                tmax = Math.Min(t2, tmax);

                if (distance > tmax)
                {
                    distance = 0f;
                    return false;
                }
            }

            return true;
        }

        public void InitFont()
        {
            glyphRect.Add(new Rectangle(600, 0, 9, 16));   //SPACE
            glyphRect.Add(new Rectangle(533, 0, 1, 16));   //!
            glyphRect.Add(new Rectangle(583, 0, 1, 16));   //.

            glyphRect.Add(new Rectangle(456, 0, 6, 16));   //0
            glyphRect.Add(new Rectangle(463, 0, 5, 16));   //1
            glyphRect.Add(new Rectangle(469, 0, 7, 16));   //2
            glyphRect.Add(new Rectangle(477, 0, 6, 16));   //3
            glyphRect.Add(new Rectangle(484, 0, 7, 16));   //4
            glyphRect.Add(new Rectangle(492, 0, 7, 16));   //5
            glyphRect.Add(new Rectangle(500, 0, 7, 16));   //6
            glyphRect.Add(new Rectangle(508, 0, 7, 16));   //7
            glyphRect.Add(new Rectangle(516, 0, 6, 16));   //8
            glyphRect.Add(new Rectangle(523, 0, 6, 16));   //9

            glyphRect.Add(new Rectangle(530, 0, 2, 16));   //:
            glyphRect.Add(new Rectangle(535, 0, 7, 16));   //?

            glyphRect.Add(new Rectangle(0, 0, 11, 16));    //A
            glyphRect.Add(new Rectangle(12, 0, 9, 16));    //B
            glyphRect.Add(new Rectangle(22, 0, 11, 16));   //C
            glyphRect.Add(new Rectangle(34, 0, 10, 16));   //D
            glyphRect.Add(new Rectangle(45, 0, 9, 16));    //E
            glyphRect.Add(new Rectangle(55, 0, 9, 16));    //F
            glyphRect.Add(new Rectangle(65, 0, 11, 16));   //G
            glyphRect.Add(new Rectangle(77, 0, 9, 16));    //H
            glyphRect.Add(new Rectangle(87, 0, 7, 16));    //I
            glyphRect.Add(new Rectangle(95, 0, 10, 16));   //J
            glyphRect.Add(new Rectangle(106, 0, 9, 16));   //K
            glyphRect.Add(new Rectangle(116, 0, 8, 16));   //L
            glyphRect.Add(new Rectangle(125, 0, 11, 16));  //M
            glyphRect.Add(new Rectangle(137, 0, 9, 16));   //N
            glyphRect.Add(new Rectangle(147, 0, 9, 16));   //O
            glyphRect.Add(new Rectangle(157, 0, 9, 16));   //P
            glyphRect.Add(new Rectangle(167, 0, 9, 16));   //Q
            glyphRect.Add(new Rectangle(177, 0, 9, 16));   //R
            glyphRect.Add(new Rectangle(187, 0, 9, 16));   //S
            glyphRect.Add(new Rectangle(197, 0, 9, 16));   //T
            glyphRect.Add(new Rectangle(207, 0, 9, 16));   //U
            glyphRect.Add(new Rectangle(217, 0, 11, 16));  //V
            glyphRect.Add(new Rectangle(229, 0, 15, 16));  //W
            glyphRect.Add(new Rectangle(245, 0, 10, 16));  //X
            glyphRect.Add(new Rectangle(256, 0, 11, 16));  //Y
            glyphRect.Add(new Rectangle(268, 0, 9, 16));   //Z

            glyphRect.Add(new Rectangle(278, 0, 6, 16));   //a
            glyphRect.Add(new Rectangle(285, 0, 5, 16));   //b
            glyphRect.Add(new Rectangle(291, 0, 6, 16));   //c
            glyphRect.Add(new Rectangle(298, 0, 5, 16));   //d
            glyphRect.Add(new Rectangle(304, 0, 5, 16));   //e
            glyphRect.Add(new Rectangle(310, 0, 5, 16));   //f
            glyphRect.Add(new Rectangle(316, 0, 6, 16));   //g
            glyphRect.Add(new Rectangle(323, 0, 5, 16));   //h
            glyphRect.Add(new Rectangle(329, 0, 5, 16));   //i
            glyphRect.Add(new Rectangle(335, 0, 5, 16));   //j
            glyphRect.Add(new Rectangle(341, 0, 5, 16));   //k
            glyphRect.Add(new Rectangle(347, 0, 5, 16));   //l
            glyphRect.Add(new Rectangle(352, 0, 7, 16));   //m
            glyphRect.Add(new Rectangle(360, 0, 5, 16));   //n
            glyphRect.Add(new Rectangle(366, 0, 7, 16));   //o
            glyphRect.Add(new Rectangle(374, 0, 5, 16));   //p
            glyphRect.Add(new Rectangle(380, 0, 7, 16));   //q
            glyphRect.Add(new Rectangle(388, 0, 5, 16));   //r
            glyphRect.Add(new Rectangle(394, 0, 6, 16));   //s
            glyphRect.Add(new Rectangle(401, 0, 5, 16));   //t
            glyphRect.Add(new Rectangle(407, 0, 7, 16));   //u
            glyphRect.Add(new Rectangle(414, 0, 7, 16));   //v
            glyphRect.Add(new Rectangle(422, 0, 11, 16));  //w
            glyphRect.Add(new Rectangle(434, 0, 6, 16));   //x
            glyphRect.Add(new Rectangle(441, 0, 7, 16));   //y
            glyphRect.Add(new Rectangle(449, 0, 6, 16));   //z


            glyphRect.Add(new Rectangle(600, 0, 8, 16));   //...


            charList.Add(' ');
            charList.Add('!');
            charList.Add('.');

            charList.Add('0');
            charList.Add('1');
            charList.Add('2');
            charList.Add('3');
            charList.Add('4');
            charList.Add('5');
            charList.Add('6');
            charList.Add('7');
            charList.Add('8');
            charList.Add('9');

            charList.Add(':');
            charList.Add('?');

            charList.Add('A');
            charList.Add('B');
            charList.Add('C');
            charList.Add('D');
            charList.Add('E');
            charList.Add('F');
            charList.Add('G');
            charList.Add('H');
            charList.Add('I');
            charList.Add('J');
            charList.Add('K');
            charList.Add('L');
            charList.Add('M');
            charList.Add('N');
            charList.Add('O');
            charList.Add('P');
            charList.Add('Q');
            charList.Add('R');
            charList.Add('S');
            charList.Add('T');
            charList.Add('U');
            charList.Add('V');
            charList.Add('W');
            charList.Add('X');
            charList.Add('Y');
            charList.Add('Z');

            charList.Add('a');
            charList.Add('b');
            charList.Add('c');
            charList.Add('d');
            charList.Add('e');
            charList.Add('f');
            charList.Add('g');
            charList.Add('h');
            charList.Add('i');
            charList.Add('j');
            charList.Add('k');
            charList.Add('l');
            charList.Add('m');
            charList.Add('n');
            charList.Add('o');
            charList.Add('p');
            charList.Add('q');
            charList.Add('r');
            charList.Add('s');
            charList.Add('t');
            charList.Add('u');
            charList.Add('v');
            charList.Add('w');
            charList.Add('x');
            charList.Add('y');
            charList.Add('z');

            charList.Add('§');

            int numberCaractere = charList.Count;


            /// NE CHANGE RIEN
            for (int i = 0; i < numberCaractere; i++)
                croppingList.Add(new Rectangle(0, 0, 16, 16));

            Vector3List.Add(new Vector3(0, -4, 0));//SPACE
            Vector3List.Add(new Vector3(0, -8, 0));//!
            Vector3List.Add(new Vector3(0, -8, 0));//.

            Vector3List.Add(new Vector3(0, -3, 0));//0
            Vector3List.Add(new Vector3(0, -4, 0));//1
            Vector3List.Add(new Vector3(0, -2, 0));//2
            Vector3List.Add(new Vector3(0, -3, 0));//3
            Vector3List.Add(new Vector3(0, -2, 0));//4
            Vector3List.Add(new Vector3(0, -2, 0));//5
            Vector3List.Add(new Vector3(0, -2, 0));//6
            Vector3List.Add(new Vector3(0, -2, 0));//7
            Vector3List.Add(new Vector3(0, -3, 0));//8
            Vector3List.Add(new Vector3(0, -3, 0));//9

            Vector3List.Add(new Vector3(0, -7, 0));//:
            Vector3List.Add(new Vector3(0, -3, 0));//?

            Vector3List.Add(new Vector3(0, 2, 0));//A
            Vector3List.Add(new Vector3(0, 0, 0));//B
            Vector3List.Add(new Vector3(0, 2, 0));//C
            Vector3List.Add(new Vector3(0, 1, 0));//D
            Vector3List.Add(new Vector3(0, 0, 0));//E
            Vector3List.Add(new Vector3(0, 0, 0));//F
            Vector3List.Add(new Vector3(0, 2, 0));//G
            Vector3List.Add(new Vector3(0, 0, 0));//H
            Vector3List.Add(new Vector3(0, -2, 0));//I
            Vector3List.Add(new Vector3(0, 1, 0));//J
            Vector3List.Add(new Vector3(0, 0, 0));//K
            Vector3List.Add(new Vector3(0, -1, 0));//L
            Vector3List.Add(new Vector3(0, 2, 0));//M
            Vector3List.Add(new Vector3(0, 0, 0));//N
            Vector3List.Add(new Vector3(0, 0, 0));//O
            Vector3List.Add(new Vector3(0, 0, 0));//P
            Vector3List.Add(new Vector3(0, 0, 0));//Q
            Vector3List.Add(new Vector3(0, 0, 0));//R
            Vector3List.Add(new Vector3(0, 0, 0));//S
            Vector3List.Add(new Vector3(0, 0, 0));//T
            Vector3List.Add(new Vector3(0, 0, 0));//U
            Vector3List.Add(new Vector3(0, 2, 0));//V
            Vector3List.Add(new Vector3(0, 6, 0));//W
            Vector3List.Add(new Vector3(0, 1, 0));//X
            Vector3List.Add(new Vector3(0, 2, 0));//Y
            Vector3List.Add(new Vector3(0, 0, 0));//Z

            Vector3List.Add(new Vector3(0, -3, 0));//a
            Vector3List.Add(new Vector3(0, -4, 0));//b
            Vector3List.Add(new Vector3(0, -3, 0));//c
            Vector3List.Add(new Vector3(0, -4, 0));//d
            Vector3List.Add(new Vector3(0, -4, 0));//e
            Vector3List.Add(new Vector3(0, -4, 0));//f
            Vector3List.Add(new Vector3(0, -3, 0));//g
            Vector3List.Add(new Vector3(0, -4, 0));//h
            Vector3List.Add(new Vector3(0, -4, 0));//i
            Vector3List.Add(new Vector3(0, -4, 0));//j
            Vector3List.Add(new Vector3(0, -4, 0));//k
            Vector3List.Add(new Vector3(0, -5, 0));//l
            Vector3List.Add(new Vector3(0, -2, 0));//m
            Vector3List.Add(new Vector3(0, -4, 0));//n
            Vector3List.Add(new Vector3(0, -2, 0));//o
            Vector3List.Add(new Vector3(0, -4, 0));//p
            Vector3List.Add(new Vector3(0, -2, 0));//q
            Vector3List.Add(new Vector3(0, -4, 0));//r
            Vector3List.Add(new Vector3(0, -3, 0));//s
            Vector3List.Add(new Vector3(0, -4, 0));//t
            Vector3List.Add(new Vector3(0, -3, 0));//u
            Vector3List.Add(new Vector3(0, -2, 0));//v
            Vector3List.Add(new Vector3(0, 2, 0));//w
            Vector3List.Add(new Vector3(0, -3, 0));//x
            Vector3List.Add(new Vector3(0, -2, 0));//y
            Vector3List.Add(new Vector3(0, -3, 0));//z


            Vector3List.Add(new Vector3(0, 0, 0));//...
            /// NE CHANGE RIEN


            UltimateFont = new SpriteFont(SuperFont, glyphRect, croppingList, charList, 0, 12f, Vector3List, '§');


            for (int i = 0; i < numberCaractere; i++)
            {
                glyphRect.Remove(glyphRect[0]);
                charList.Remove(charList[0]);
                croppingList.Remove(croppingList[0]);
                Vector3List.Remove(Vector3List[0]);
            }


        }


        public bool RayIntersectBox2DResolution(Vector3 rayO, Vector3 rayE, BoundingBox box)
        {

            bool isCollidedDown = false;
            bool isCollidedRight = false;
            bool isCollidedBehind = false;


            ///Resolution Plan Du Bas
            if (lineLine(rayO.X, rayO.Z, rayE.X, rayE.Z, box.Min.X, box.Min.Z, box.Max.X, box.Min.Z) ||
                lineLine(rayO.X, rayO.Z, rayE.X, rayE.Z, box.Max.X, box.Min.Z, box.Max.X, box.Max.Z))
                isCollidedDown = true;


            ///Resolution Plan Du Coté
            if (lineLine(rayO.Y, rayO.Z, rayE.Y, rayE.Z, box.Min.Y, box.Min.Z, box.Max.Y, box.Min.Z))
                isCollidedRight = true;


            ///Resolution Plan De Derriere
            if (lineLine(rayO.Y, rayO.Z, rayE.Y, rayE.Z, box.Min.Y, box.Min.Z, box.Max.Y, box.Min.Z))
                isCollidedBehind = true;


            if (isCollidedDown && isCollidedRight)
                return true;


            return false;


        }


        public Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(MouseInput.GetPos(), 0f);
            Vector3 farSource = new Vector3(MouseInput.GetPos(), 4f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        private Vector3 cursorRayToCoords(Ray ray, Vector3 cameraPos)
        {
            Nullable<float> distance = ray.Intersects(new Plane(Vector3.Up, 0.0f));
            if (distance == null)
                return Vector3.Zero;
            else
            {
                return cameraPos + ray.Direction * (float)distance;
            }
        }



        public void SerializeNow(Chunk chunk)
        {
            Chunk c = chunk;

            Stream s = new FileStream("Test.txt", FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, c);
            s.Close();
        }
        public void DeSerializeNow()
        {

        }


    }
}
