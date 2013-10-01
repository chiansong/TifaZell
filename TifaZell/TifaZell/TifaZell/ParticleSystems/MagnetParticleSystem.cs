using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//X.N.A
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//D.P.S.F
using DPSF;

namespace TifaZell.ParticleSystems
{
    /// <summary>
    /// Create a new Magnet Particle System class
    /// </summary>
    class MagnetParticleSystem : DefaultTexturedQuadParticleSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagnetParticleSystem(Game game) : base(game) { }

        //=======================//
        // Structure & Variables //
        //=======================//
        //Variable for particles to travel in same direction.
        private bool mInitializeParticlesWithRandomDirection = true;

        //Min and Max Particle Distance.
        private float mMinDistance = 0;
        private float mMaxDistance = 150;

        //Magnet Force.
        private float mMagnetForce = 20;
        /// <summary>
        /// Get/Set the Max Force of magnets on particles.
        /// </summary>
        public float MagnetsForce
        {
            get { return mMagnetForce; }
            set
            {
                mMagnetForce = value;
                //Set the Max force of the magnet.
                foreach (DefaultParticleSystemMagnet magnet in MagnetList)
                {
                    magnet.MaxForce = mMagnetForce;
                }
            }
        }

        //Magnet Distance Function
        private DefaultParticleSystemMagnet.DistanceFunctions mMagnetDistanceFunction = DefaultParticleSystemMagnet.DistanceFunctions.SquaredInverse;
        /// <summary>
        /// Get/Set the Distance Function that the magnets uses.
        /// </summary>
        public DefaultParticleSystemMagnet.DistanceFunctions MagnetsDistanceFunction
        {
            get { return mMagnetDistanceFunction; }
            set
            {
                mMagnetDistanceFunction = value;
                //Set the Distance Function of each Magnet.
                foreach (DefaultParticleSystemMagnet magnet in MagnetList)
                {
                    magnet.DistanceFunction = mMagnetDistanceFunction;
                }
            }
        }

        //Magnet Modes.
        private DefaultParticleSystemMagnet.MagnetModes mMagnetMode = DefaultParticleSystemMagnet.MagnetModes.Attract;
        /// <summary>
        /// Get/Set the Magnet Mode.
        /// </summary>
        public DefaultParticleSystemMagnet.MagnetModes MagnetMode
        {
            get { return mMagnetMode; }
            set
            {
                mMagnetMode = value;
                //Set the magnet mode of the magnet
                foreach(DefaultParticleSystemMagnet magnet in MagnetList)
                {
                    magnet.Mode = mMagnetMode;
                }
            }
        }

        /// <summary>
        /// Get if magnets affects a particle's position or velocity.
        /// </summary>
        public bool MagnetsAffectPosition { get; private set; }

        /// <summary>
        /// Function to Initialize the Particle System with default values.
        /// Particle system properties should not be set until after this is called, as 
        /// they are likely to be reset to their default values.
        /// </summary>
        public override void AutoInitialize(GraphicsDevice graphicsDevice, ContentManager contentManager, SpriteBatch spriteBatch)
        {
            //Initialize the particle system.
            InitializeTexturedQuadParticleSystem(graphicsDevice, contentManager, 1000, 50000,
                                                 UpdateVertexProperties, "Textures/Dot");

             LoadParticleSystem();
        }

        /// <summary>
        /// Load the particle stehm.
        /// </summary>
        public void LoadParticleSystem()
        {
            // Set the Particle Initialization Function, as it can be changed on the fly
            // and we want to make sure we are using the right one to start with to start with.
            ParticleInitializationFunction = InitializeParticleProperties;

            //Remove the events
            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            //Setup the emitter
            Emitter.ParticlesPerSecond = 100;
            Emitter.PositionData.Position = new Vector3(0, 0, 0);

            //Act in the events
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            //Fade out
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);
            //Billboard
            ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 200);

            //Set the position maganet.
            ToogleMagnetAffectPositionOrVelocity();
            //Point Magnet
            UseMagnetType(DefaultParticleSystemMagnet.MagnetTypes.PointMagnet);
        }

        /// <summary>
        /// Create a Particle Initialization Function
        /// </summary>
        public void InitializeParticleProperties(DefaultTexturedQuadParticle particle)
        {
            //Set position and particle system
            particle.Lifetime = 5;
            particle.Position = Emitter.PositionData.Position;

            //If it should travel in random direction
            if (mInitializeParticlesWithRandomDirection)
                particle.Velocity = DPSFHelper.RandomNormalizedVector() * 50;
            else
            {
                particle.Velocity = Vector3.Right * 50;
                particle.Velocity = Vector3.Transform(particle.Velocity, Emitter.OrientationData.Orientation);
            }

            particle.Size = 10;
            particle.Color = Color.WhiteSmoke;
        }

        /// <summary>
        /// Create a Particle Event Functions.
        /// </summary>
        public void UpdateParticleSystemFunctionExample(float fElapsedTimeInSeconds)
        {
            // Place code to update the Particle System here
            // Example: Emitter.EmitParticles = true;
            // Example: SetTexture("TextureAssetName");
        }

        //=================================//
        // Other Particle System Functions //
        //=================================//
        /// <summary>
        /// Create a Particle Event Functions.
        /// </summary>
        public void ToogleMagnetAffectPositionOrVelocity()
        {
            //Toggle Magnet Position or Velocity
            MagnetsAffectPosition = !MagnetsAffectPosition;
            //Remove previous magnet particle events.
            ParticleEvents.RemoveAllEventsInGroup(1);

            if (MagnetsAffectPosition)
                ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAccordingToMagnets, 0, 1);
            else
                ParticleEvents.AddEveryTimeEvent(UpdateParticleVelocityAccordingToMagnets, 0, 1);
        }

        /// <summary>
        /// Basic Magnet Type.
        /// </summary>
        public void UseMagnetType(DefaultParticleSystemMagnet.MagnetTypes magnetTypeToUse)
        {
            UseMagnetType(magnetTypeToUse, Vector3.Zero, Vector3.Zero);
        }

        /// <summary>
        /// Specify which Type of Magnet we should use to affect the Particles.
        /// </summary>
        public void UseMagnetType(DefaultParticleSystemMagnet.MagnetTypes magnetTypeToUse, 
                                  Vector3 vec1, Vector3 vec2)
        {
            //Clear Magnet
            MagnetList.Clear();
            //Find Type of Magnet.
            switch (magnetTypeToUse)
            {
                default:
                case DefaultParticleSystemMagnet.MagnetTypes.PointMagnet:
                    MagnetList.Add(new MagnetPoint(vec1, MagnetMode, MagnetsDistanceFunction,
                                                   mMinDistance, mMaxDistance, mMagnetForce, 0));
                    break;

                case DefaultParticleSystemMagnet.MagnetTypes.LineMagnet:
                    MagnetList.Add(new MagnetLine(vec1, Vector3.Up, MagnetMode, MagnetsDistanceFunction,
                                                   mMinDistance, mMaxDistance, mMagnetForce, 0));
                    break;

                case DefaultParticleSystemMagnet.MagnetTypes.LineSegmentMagnet:
                    MagnetList.Add(new MagnetLineSegment(vec1, vec2, MagnetMode, MagnetsDistanceFunction,
                                                   mMinDistance, mMaxDistance, mMagnetForce, 0));
                    break;

                case DefaultParticleSystemMagnet.MagnetTypes.PlaneMagnet:
                    MagnetList.Add(new MagnetPlane(vec1, Vector3.Right, MagnetMode, MagnetsDistanceFunction,
                                                   mMinDistance, mMaxDistance, mMagnetForce, 0));
                    break;
            }
        }

        public void UpdateEmittorPoint(Vector3 vec, int no)
        {
            //Let handle point only.
            if (MagnetList[no].MagnetType != DefaultParticleSystemMagnet.MagnetTypes.PointMagnet)
                return;

            MagnetPoint point = (MagnetPoint)MagnetList[no];
            point.PositionData.Position = vec;
        }
    }
}
