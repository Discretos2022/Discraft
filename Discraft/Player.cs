using DiscCraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discraft
{
    public class Player
    {

        public Vector3 Position;
        public Vector3 Velocity;

        public Vector3 Rotation;

        public Vector3 mouseRotationBuffer;


        public Player(Vector3 _position)
        {
            Position = _position;
        }


        public void Update(GameTime gameTime, Game game)
        {

            Vector3 moveVector = Vector3.Zero;

            if (KeyInput.getKeyState().IsKeyDown(Keys.W))
                moveVector.Z = 1f;
            if (KeyInput.getKeyState().IsKeyDown(Keys.S))
                moveVector.Z = -1f;

            if (KeyInput.getKeyState().IsKeyDown(Keys.A))
                moveVector.X = 1f;
            if (KeyInput.getKeyState().IsKeyDown(Keys.D))
                moveVector.X = -1f;

            if (KeyInput.getKeyState().IsKeyDown(Keys.Up))
                moveVector.Y = 1f;
            if (KeyInput.getKeyState().IsKeyDown(Keys.Down))
                moveVector.Y = -1f;

            if (moveVector != Vector3.Zero)
            {
                /// Normalize Vector
                moveVector.Normalize();

                moveVector *= 0.2f;

                /// Move camera
                Move(new Vector3(moveVector.X, 0, 0));
                Move(new Vector3(0, moveVector.Y, 0));
                Move(new Vector3(0, 0, moveVector.Z));
                //Move(moveVector);

            }

            float deltaX;
            float deltaY;

            if (/*MouseInput.getMouseState() != MouseInput.getOldMouseState() &&*/ Main.cameraActived)
            {
                /// Cache mouse location
                deltaX = (MouseInput.getMouseState().X - game.GraphicsDevice.Viewport.Width / 2);
                deltaY = (MouseInput.getMouseState().Y - game.GraphicsDevice.Viewport.Height / 2);

                if (KeyInput.getKeyState().IsKeyDown(Keys.Right))
                    deltaX = 10f;
                if (KeyInput.getKeyState().IsKeyDown(Keys.Left))
                    deltaX = -10f;

                mouseRotationBuffer.X -= Main.cameraV2.cameraSensibility * deltaX;
                mouseRotationBuffer.Y -= Main.cameraV2.cameraSensibility * deltaY;

                if (mouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                ///
                Rotation = new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)), MathHelper.WrapAngle(mouseRotationBuffer.X), 0);


                //Console.WriteLine(mouseRotationBuffer);

                deltaX = 0;
                deltaY = 0;

                Mouse.SetPosition(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);

            }


            Main.cameraV2.Position = Position;
            Main.cameraV2.Rotation = Rotation;

        }





        public void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }


        /// Simulate movement
        private Vector3 PreviewMove(Vector3 amount)
        {
            /// Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(Rotation.Y);

            /// Create a movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);

            /// Return the valueof camera position + mouvement vector
            return Position + movement;

        }


        public void Move(Vector3 Scale)
        {
            MoveTo(PreviewMove(Scale), Rotation);
        }


    }
}
