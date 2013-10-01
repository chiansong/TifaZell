using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Include the DPSF
using DPSF;

namespace TifaZell.Graphics
{
    class GameCamera
    {
        //Camera View Variable
        public Vector3 ReferencePoint;
        public Vector3 PlaneNormal;
        public Vector3 Up;
        public Vector3 Left;

        //Camera Variable
        public float CameraArc;
        public float CameraRotation;
        public float CameraDistance;

        //Look-At Position
        public Vector3 LookAtPosition;

        //Type to use
        public bool UsingFixedCamera;

        //Constructor
        public GameCamera(bool fixedCamera)
        {
            ReferencePoint = PlaneNormal = Up = Left = Vector3.Zero;
            CameraArc = CameraRotation = CameraDistance = 0.0f;

            UsingFixedCamera = fixedCamera;

            //Initialize
            ResetFixedCamera();
            ResetFreeCamera();
        }

        public Vector3 Position
        {
            get
            {
                if (UsingFixedCamera)
                {
                    Matrix ViewMatrix = Matrix.CreateTranslation(LookAtPosition) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(CameraRotation)) *
                        Matrix.CreateRotationX(MathHelper.ToRadians(CameraArc)) *
                        Matrix.CreateLookAt(new Vector3(0, 0, -CameraDistance),
                                            new Vector3(0, 0, 0), Vector3.Up);

                    //Invert View Matrix.
                    ViewMatrix = Matrix.Invert(ViewMatrix);

                    return ViewMatrix.Translation;
                }
                else
                    return ReferencePoint;
            }
        }

        //Reset Fixed Camera
        public void ResetFixedCamera()
        {
            CameraArc = 0.0f;
            CameraRotation = 0.0f;
            CameraDistance = 300.0f;
            LookAtPosition = new Vector3(0, 0, 0);
        }
        //Reset Free Camera
        public void ResetFreeCamera()
        {
            ReferencePoint = new Vector3(0.0f, 50.0f, 300.0f);
            PlaneNormal = Vector3.Forward;
            Up = Vector3.Up;
            Left = Vector3.Left;
        }

        // Normalize the Camera Directions and maintain proper Right and Up directions
        public void NormalizeCameraAndCalculateProperUpAndRightDirections()
        {
            // Calculate the new Right and Up directions
            PlaneNormal.Normalize();
            Left = Vector3.Cross(Up, PlaneNormal);
            Left.Normalize();
            Up = Vector3.Cross(PlaneNormal, Left);
            Up.Normalize();
        }

        //Move Camera Forward
        public void MoveForward(float amount)
        {
            PlaneNormal.Normalize();
            ReferencePoint += PlaneNormal * amount;
        }

        //Move Camera Backward
        public void MoveBackward(float amount)
        {
            PlaneNormal.Normalize();
            ReferencePoint -= PlaneNormal * amount;
        }

        // Rotate the Camera Horizontally
        public void RotateCameraHorizontally(float amount)
        {
            // Rotate the Camera about the global Y axis
            Matrix cRotationMatrix = Matrix.CreateFromAxisAngle(Vector3.Up, amount);
            PlaneNormal = Vector3.Transform(PlaneNormal, cRotationMatrix);
            Up = Vector3.Transform(Up, cRotationMatrix);

            // Normalize all of the Camera directions since they have changed
            NormalizeCameraAndCalculateProperUpAndRightDirections();
        }

        /// Rotate the Camera Vertically
        public void RotateCameraVertically(float amount)
        {
            // Rotate the Camera
            Matrix cRotationMatrix = Matrix.CreateFromAxisAngle(Left, amount);
            PlaneNormal = Vector3.Transform(PlaneNormal, cRotationMatrix);
            Up = Vector3.Transform(Up, cRotationMatrix);

            // Normalize all of the Camera directions since they have changed
            NormalizeCameraAndCalculateProperUpAndRightDirections();
        }

        // Convert Screen Coord to World Coord.
        public Vector3 GetScreenToWorld(Vector2 point, float height, float width)
        {
            Matrix mat = new Matrix(2.0f / width, 0, 0, 0,
                                    0, -2.0f / height, 0, 0,
                                    0, 0, 1.0f, 0.0f,
                                    (-width + 1.0f) / width, (height - 1.0f) / height, 0, 1.0f);
            //in 2D to 4D
            Vector4 vec = new Vector4(point.X, point.Y, 1.0f, 1.0f);

            //Transform to NDC
            vec = Vector4.Transform(vec, mat);

            //count new points.
            Vector3 newPoint = new Vector3(-vec.X * width / 2.0f, vec.Y * height / 2.0f, 0.0f);
            return newPoint;
        }
    }
}
