using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//X.N.A
using Microsoft.Xna.Framework;

namespace TifaZell.Graphics
{
    class FrameAnimation : ICloneable
    {
        private Rectangle mRectInitialFrame;//First Frame of the animation.
        private int mFrameCount = 1; //No.of Frame
        private int mCurrentFrame = 0; //Current frame displayed
        private float mFrameLength = 0.2f; //time to display each frame.
        private float mFrameTimer = 0.0f; //time since last frame.
        private int mPlayCount = 0; //Number of time the animation has played. 
        private string mNextAnimation = null; //The next animation.

        //////////////////////////////////////////////////////////////////////////
        // Set/Get Accessory.
        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The number of frame
        /// </summary>
        public int FrameCount
        {
            get { return mFrameCount; }
            set { mFrameCount = value; }
        }

        /// <summary>
        /// The lenght of the each frame in seconds. 
        /// </summary>
        public float FrameLength
        {
            get { return mFrameLength; }
            set { mFrameLength = value; }
        }

        /// <summary>
        /// The no. of the current frame.
        /// </summary>
        public int CurrentFrame
        {
            get { return mCurrentFrame; }
            set { mCurrentFrame = (int)MathHelper.Clamp(value, 0, mFrameCount - 1); }
        }

        /// <summary>
        /// Width of each frame.
        /// </summary>
        public int FrameWidth
        {
            get { return mRectInitialFrame.Width; }
        }

        /// <summary>
        /// Height of each frame.
        /// </summary>
        public int FrameHeight
        {
            get { return mRectInitialFrame.Height; }
        }

        /// <summary>
        /// The rectangle associated with the currrent animation frame.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(mRectInitialFrame.X + (mRectInitialFrame.Width * mCurrentFrame),
                                     mRectInitialFrame.Y, mRectInitialFrame.Width, mRectInitialFrame.Height);
            }
        }

        /// <summary>
        /// The count of no. times the animation has been played.
        /// </summary>
        public int PlayCount
        {
            get { return mPlayCount; }
            set { mPlayCount = value; }
        }

        /// <summary>
        /// The next animation to play.
        /// </summary>
        public string NextAnimation
        {
            get { return mNextAnimation; }
            set { mNextAnimation = value; }
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="FirstFrame"></param>
        /// <param name="Frames"></param>
        public FrameAnimation(Rectangle FirstFrame, int Frames)
        {
            mRectInitialFrame = FirstFrame;
            mFrameCount = Frames;
        }

        /// <summary>
        /// 2nd Constructor.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Frames"></param>
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames)
        {
            mRectInitialFrame = new Rectangle(X, Y, Width, Height);
            mFrameCount = Frames;
        }

        /// <summary>
        /// 3rd Constructor
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Frames"></param>
        /// <param name="FrameLength"></param>
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength)
        {
            mRectInitialFrame = new Rectangle(X, Y, Width, Height);
            mFrameCount = Frames;
            mFrameLength = FrameLength;
        }

        /// <summary>
        /// 4rd Constructor.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Frames"></param>
        /// <param name="FrameLength"></param>
        /// <param name="nextAnimation"></param>
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength, string nextAnimation)
        {
            mRectInitialFrame = new Rectangle(X, Y, Width, Height);
            mFrameCount = Frames;
            mFrameLength = FrameLength;
            mNextAnimation = nextAnimation;
        }

        /// <summary>
        /// Update the game time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Update the time.
            mFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Reset and the animation if it is more.
            if (mFrameTimer > mFrameLength)
            {
                mFrameTimer = 0.0f;
                mCurrentFrame = (mCurrentFrame + 1) % mFrameCount;
                if (mCurrentFrame == 0)
                    mPlayCount = (int)MathHelper.Min(mPlayCount + 1, int.MaxValue);
            }
        }

        object ICloneable.Clone()
        {
            return new FrameAnimation(this.mRectInitialFrame.X, this.mRectInitialFrame.Y,
                                      this.mRectInitialFrame.Width, this.mRectInitialFrame.Height,
                                      this.mFrameCount, this.mFrameLength, mNextAnimation);
        }
    }
}
