using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//TifaZell
using TifaZell.Graphics;
using TifaZell.ParticleSystems;
using TifaZell.GameSystem.GameObjects;

//DPSF
using DPSF;

namespace TifaZell.GameSystem
{
    /// <summary>
    /// Main Game for TifaZell Project.
    /// </summary>
    static public class MainGame
    {
        // The World, View, and Projection matrices
        static Matrix mWorldMatrix = Matrix.Identity;
        static Matrix mViewMatrix = Matrix.Identity;
        static Matrix mProjectionMatrix = Matrix.Identity;

        //Camera
        static GameCamera mCamera = new GameCamera(true);

        //Particle System
        static ParticleSystemManager mPSManager = new ParticleSystemManager();
        static MagnetParticleSystem mMagnetPS = null;

        //Game References
        static public Game mGame;
        static public SpriteBatch mSpriteBatch;

        //Game Objects
        static MainCharacter mMainChar;

        /// <summary>
        /// Constructor.
        /// </summary>
        static MainGame()
        {
        }

        /// <summary>
        /// Loading Graphics Content
        /// </summary>
        static public void LoadContent(Game game, SpriteBatch spriteBatch)
        {
            //Set the important stuff.
            mGame = game;
            mSpriteBatch = spriteBatch;

            //Initialize Particle System.
            mMagnetPS = new MagnetParticleSystem(game);
            mMagnetPS.AutoInitialize(game.GraphicsDevice, game.Content, null);
            mMagnetPS.UseMagnetType(DefaultParticleSystemMagnet.MagnetTypes.PointMagnet);
            //Add particle system to ps manager.
            mPSManager.AddParticleSystem(mMagnetPS);

            //let setup.
            Initiliaze();
        }

        /// <summary>
        /// Setup for all the other stuff.
        /// </summary>
        static public void Initiliaze()
        {
            mMainChar = new MainCharacter();
            mMainChar.Positon = new Vector2(100, mGame.GraphicsDevice.Viewport.Height / 2.0f);
        }

        /// <summary>
        /// Updating game-loop.
        /// </summary>
        /// <param name="gameTime"></param>
        static public void Update(GameTime gameTime)
        {
            //Update Matrices
            UpdateProjectionMatrices();

            mPSManager.SetWorldViewProjectionMatricesForAllParticleSystems(mWorldMatrix, mViewMatrix, mProjectionMatrix);
            mPSManager.SetCameraPositionForAllParticleSystems(mCamera.Position);
            mPSManager.UpdateAllParticleSystems((float)gameTime.ElapsedGameTime.TotalSeconds); 

            //Update the game.
            UpdateMouseInput(gameTime);

            //Update the main character.
            mMainChar.Update(gameTime);
        }

        /// <summary>
        /// Update the matrix.
        /// </summary>
        static private void UpdateProjectionMatrices()
        {
            GraphicsDevice gd = mGame.GraphicsDevice;
            
            //Get Camera View Matrix.
            if (mCamera.UsingFixedCamera)
            {
                mViewMatrix = Matrix.CreateTranslation(mCamera.LookAtPosition) *
                    Matrix.CreateRotationY(MathHelper.ToRadians(mCamera.CameraRotation)) *
                    Matrix.CreateRotationX(MathHelper.ToRadians(mCamera.CameraArc)) *
                    Matrix.CreateLookAt(new Vector3(0, 0, -mCamera.CameraDistance),
                                        new Vector3(0, 0, 0), Vector3.Up);
            }
            else
                mViewMatrix = Matrix.CreateLookAt(mCamera.ReferencePoint, mCamera.ReferencePoint + mCamera.PlaneNormal, mCamera.Up);

            //Get the Projection Matrix.
            mProjectionMatrix = Matrix.CreateOrthographic((float)gd.Viewport.Width, (float)gd.Viewport.Height, 0, 10000);
        }

        /// <summary>
        /// Update the mouse input.
        /// </summary>
        /// <param name="gameTime"></param>
        static public void UpdateMouseInput(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 vec = new Vector2(mouseState.X,mouseState.Y);

            float height = mGame.GraphicsDevice.Viewport.Height;
            float width  = mGame.GraphicsDevice.Viewport.Width;
            Vector3 newVec = mCamera.GetScreenToWorld(vec,height,width);

            mMagnetPS.UpdateEmittorPoint(newVec, 0);
        }

        /// <summary>
        /// Drawing graphics loop.
        /// </summary>
        /// <param name="gameTime"></param>
        static public void Draw(GameTime gameTime)
        {
            mMainChar.Draw(gameTime);
            mPSManager.DrawAllParticleSystems();
        }
    }
}
