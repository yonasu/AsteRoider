using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AsteRoider
{
    class Camera
    {
        private Vector3 position = Vector3.Zero;
        private float rotation;

        public Matrix Projection { get; private set; }
        private Vector3 lookAt;
        private Vector3 baseCameraReference = new Vector3(0, 0, 1);
        private bool needViewResync = true;

        private Matrix cachedViewMatrix;


        #region Constructor
        public Camera(Vector3 position, float rotation, float aspectRatio, float nearClip, float farClip)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            aspectRatio,
            nearClip,
            farClip);
            MoveTo(position, rotation);
        }

        private void MoveTo(Vector3 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;

        }
        #endregion
        #region Helper Methods
        private void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(rotation);
            Vector3 lookAtOffset = Vector3.Transform(
            baseCameraReference,
            rotationMatrix);
            lookAt = position + lookAtOffset;
            needViewResync = true;
        }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateLookAt();
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value;
            UpdateLookAt();
            }
        }

        public Matrix View
        {
            get
            {
                if (needViewResync)
                    cachedViewMatrix = Matrix.CreateLookAt(
                    Position,
                    lookAt,
                    Vector3.Up);
                return cachedViewMatrix;
            }
        }

        #endregion

    }
}
