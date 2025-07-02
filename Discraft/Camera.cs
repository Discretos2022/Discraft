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


        private BoundingFrustum _boundingFrustum;
        public bool _frustumNeedsUpdate = true;


        public BoundingFrustum BoundingFrustum
        {
            get
            {
                if (_frustumNeedsUpdate)
                {
                    UpdateFrustum();
                }
                return _boundingFrustum;
            }
        }


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
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000);//256.0f); // game.GraphicsDevice.Viewport.AspectRatio    0.05f, 2000

            /// Set camera position
            MoveTo(position, rotation);

            /// Init the frustrum
            _boundingFrustum = new BoundingFrustum(Matrix.Identity);
            UpdateFrustum();

        }

        private void UpdateFrustum()
        {
            _boundingFrustum.Matrix = View * Projection;
            _frustumNeedsUpdate = false;
        }


        public void MoveTo(Vector3 pos, Vector3 rot)
        {
            cameraPosition = pos;
            cameraRotation = rot;
            _frustumNeedsUpdate = true;
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

            _frustumNeedsUpdate = true;

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
            
            if (_frustumNeedsUpdate)
            {
                UpdateFrustum();
            }

        }

        
        public bool IsChunkVisible(BoundingBox chunkBounds)
        {
            return BoundingFrustum.Intersects(chunkBounds);
        }


    }
}
