using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Test
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        BasicEffect basicEffect;
        Matrix world = Matrix.CreateTranslation(0, 0, 0);
        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
        double angle = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            basicEffect = new BasicEffect(GraphicsDevice);

            // A temporary array, with 12 items in it, because
            // the icosahedron has 12 distinct vertices
            VertexPositionColor[] vertices = new VertexPositionColor[9];
            //VertexPositionColor[] vertices = new VertexPositionColor[12];

            // vertex position and color information for icosahedron
            //vertices[0] = new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, 0.42532500f), Color.Red);
            //vertices[1] = new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, 0.42532500f), Color.Orange);
            //vertices[2] = new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, -0.42532500f), Color.Yellow);
            //vertices[3] = new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, -0.42532500f), Color.Green);
            //vertices[4] = new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, 0.26286500f), Color.Blue);
            //vertices[5] = new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, -0.26286500f), Color.Indigo);
            //vertices[6] = new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, 0.26286500f), Color.Purple);
            //vertices[7] = new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, -0.26286500f), Color.White);
            //vertices[8] = new VertexPositionColor(new Vector3(0.42532500f, 0.26286500f, 0.0000000f), Color.Cyan);
            //vertices[9] = new VertexPositionColor(new Vector3(-0.42532500f, 0.26286500f, 0.0000000f), Color.Black);
            //vertices[10] = new VertexPositionColor(new Vector3(0.42532500f, -0.26286500f, 0.0000000f), Color.DodgerBlue);
            //vertices[11] = new VertexPositionColor(new Vector3(-0.42532500f, -0.26286500f, 0.0000000f), Color.Crimson);

            vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(1, 0, 0), Color.Aqua);
            vertices[2] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(1, 1, 0), Color.Chartreuse);
            vertices[4] = new VertexPositionColor(new Vector3(0, 0, 1), Color.DarkSlateGray);
            vertices[5] = new VertexPositionColor(new Vector3(1, 0, 1), Color.GhostWhite);
            vertices[6] = new VertexPositionColor(new Vector3(0, 1, 1), Color.LightGreen);
            vertices[7] = new VertexPositionColor(new Vector3(1, 1, 1), Color.RoyalBlue);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            //short[] indices = new short[60];
            short[] indices = new short[36];
            //indices[0] = 0; indices[1] = 6; indices[2] = 1;
            //indices[3] = 0; indices[4] = 11; indices[5] = 6;
            //indices[6] = 1; indices[7] = 4; indices[8] = 0;
            //indices[9] = 1; indices[10] = 8; indices[11] = 4;
            //indices[12] = 1; indices[13] = 10; indices[14] = 8;
            //indices[15] = 2; indices[16] = 5; indices[17] = 3;
            //indices[18] = 2; indices[19] = 9; indices[20] = 5;
            //indices[21] = 2; indices[22] = 11; indices[23] = 9;
            //indices[24] = 3; indices[25] = 7; indices[26] = 2;
            //indices[27] = 3; indices[28] = 10; indices[29] = 7;
            //indices[30] = 4; indices[31] = 8; indices[32] = 5;
            //indices[33] = 4; indices[34] = 9; indices[35] = 0;
            //indices[36] = 5; indices[37] = 8; indices[38] = 3;
            //indices[39] = 5; indices[40] = 9; indices[41] = 4;
            //indices[42] = 6; indices[43] = 10; indices[44] = 1;
            //indices[45] = 6; indices[46] = 11; indices[47] = 7;
            //indices[48] = 7; indices[49] = 10; indices[50] = 6;
            //indices[51] = 7; indices[52] = 11; indices[53] = 2;
            //indices[54] = 8; indices[55] = 10; indices[56] = 3;
            //indices[57] = 9; indices[58] = 11; indices[59] = 0;

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

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            angle += 0.01f;
            view = Matrix.CreateLookAt(
                new Vector3(5 * (float)Math.Sin(angle), -2, 5 * (float)Math.Cos(angle)),
                new Vector3(0.5f, 0.5f, 0.5f),
                Vector3.UnitY);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            LoadContent();

            GraphicsDevice.Clear(Color.Black);

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            basicEffect.LightingEnabled = true;
            basicEffect.AmbientLightColor = new Vector3(1, 1, 1);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12, 0, 20);
            }

            base.Draw(gameTime);
        }
    }
}