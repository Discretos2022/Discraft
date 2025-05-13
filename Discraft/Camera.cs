using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscCraft
{
    public class Camera
    {

        public Game game;

        /// Attribute
        public Vector3 cameraPosition;
        public Vector3 cameraRotation;
        public float cameraSpeed;
        public Vector3 cameraLookAt;
        public Vector3 mouseRotationBuffer;
        public MouseState currentMouseState;
        public MouseState prevMouseState;

        public float cameraSensibility = 0.002f;


        public Vector3 Position
        {
            get { return cameraPosition; }
            set 
            {
                cameraPosition = value;
                    UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
                UpdateLookAt();
            }
        }



        public Matrix Projection;

        public Matrix View
        {
            get { return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up); }
        }


        public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
        {

            this.game = game;

            cameraSpeed = speed;

            /// Setup projection matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 0.05f, 2000);//256.0f); // game.GraphicsDevice.Viewport.AspectRatio

            /// Set camera position
            MoveTo(position, rotation);


        }


        public void MoveTo(Vector3 pos, Vector3 rot)
        {
            cameraPosition = pos;
            cameraRotation = rot;
        }

        /// Update the look at Vector
        public void UpdateLookAt()
        {

            /// Build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationY(cameraRotation.Y); // * Matrix.CreateRotationZ(cameraRotation.Z);

            /// Build look at offset vector
            Vector3 LookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);

            /// Update camera look at vector
            cameraLookAt = cameraPosition + LookAtOffset;

        }


        /// Simulate movement
        private Vector3 PreviewMove(Vector3 amount)
        {
            /// Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);

            /// Create a movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);

            /// Return the valueof camera position + mouvement vector
            return cameraPosition + movement;

        }


        public void Move(Vector3 Scale)
        {
            MoveTo(PreviewMove(Scale), Rotation);
        }


        public void Update()
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

                moveVector *= cameraSpeed;

                /// Move camera
                Move(moveVector);

            }


            /// Handle mouse movement

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

                mouseRotationBuffer.X -= cameraSensibility * deltaX;
                mouseRotationBuffer.Y -= cameraSensibility * deltaY;

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

            //if(Main.cameraActived)
                



        }




    }
}
