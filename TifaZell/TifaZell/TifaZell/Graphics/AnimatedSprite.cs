//  [10/1/2013 Chian Song]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//X.N.A
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TifaZell.Graphics
{
    class AnimatedSprite
    {
        Texture2D mTexture; //The texture
        bool mIsAnimating = true; //Is animating.
        Color mColorTint = Color.White; //Set the color.
        
        Vector2 mPosition = new Vector2(); //Screen position
        Vector2 mLastPosition = new Vector2(); //Last Screen Position
        
        //The dictionary holding the frame animation objects.
        Dictionary<string, FrameAnimation> mFrameAnimations = new Dictionary<string, FrameAnimation>();
        string mCurrentAnimation = null; //Which frame animation name we are playing.
       
        //True - sprite will auto rotate to align itself with angle difference between
        //       it's new position and it's previous position.
        bool mRotateByPosition = false;
        float mRotation = 0.0f; //How much to rotate the sprite by Radians.
        
        Vector2 mCenter; //Calculated center of the sprite.
        
        int mWidth; //Width of the sprite.
        int mHeight; //Height of the sprite.

        /// <summary>
        /// Representing the position of the sprite (Upper left corner pixel)
        /// </summary>
        public Vector2 Position
        {
            get {return mPosition;}
            set
            {
                mLastPosition = mPosition;
                mPosition = value;
                UpdateRotation();
            }
        }

        /// <summary>
        /// X position of upper left corner pixel.
        /// </summary>
        public int X
        {
            get {return (int)mPosition.X;}
            set
            {
                mLastPosition.X = mPosition.X;
                mPosition.X = value;
                UpdateRotation();
            }
        }

        /// <summary>
        /// Y position of upper left corner pixel.
        /// </summary>
        public int Y
        {
            get {return (int)mPosition.Y;}
            set
            {
                mLastPosition.Y = mPosition.Y;
                mPosition.Y = value;
                UpdateRotation();
            }
        }

        /// <summary>
        /// Width (Pixels) of sprite animation frames.
        /// </summary>
        public int Width
        {
            get {return mWidth;}
        }

        /// <summary>
        /// Height (Pixels) of sprite animation frames.
        /// </summary>
        public int Height
        {
            get {return mHeight;}
        }

        /// <summary>
        /// If true, the sprite will auto rotate in direction of position.
        /// -> or <-
        /// </summary>
        public bool AutoRotate
        {
            get {return mRotateByPosition;}
            set {mRotateByPosition = value;}
        }

        /// <summary>
        /// the Rotation of the sprite frames.
        /// </summary>
        public float Rotation
        {
            get {return mRotation;}
            set {mRotation = value;}
        }

        /// <summary>
        /// The bounding box of the sprite.
        /// </summary>
        public Rectangle BoundingBox
        {
            get {return new Rectangle(X,Y,mWidth,mHeight);}
        }

        /// <summary>
        /// Texture of the sprite.
        /// </summary>
        public Texture2D Texture
        {
            get {return mTexture;}
        }

        /// <summary>
        /// True - Sprite is animating.
        /// False - will draw nothing unless drawn the first frame.
        /// </summary>
        public bool IsAnimating
        {
            get {return mIsAnimating;}
            set {mIsAnimating = value;}
        }

        /// <summary>
        /// FrameAnimation Object of the currently playing animation.
        /// </summary>
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if(!string.IsNullOrEmpty(mCurrentAnimation))
                    return mFrameAnimations[mCurrentAnimation];
                else
                    return null;
            }
        }

        /// <summary>
        /// String name of currently playing animation
        /// </summary>
        public string CurrentAnimation
        {
            get {return mCurrentAnimation;}
            set 
            {
                if(mFrameAnimations.ContainsKey(value))
                {
                    mCurrentAnimation = value;
                    mFrameAnimations[mCurrentAnimation].CurrentFrame = 0;
                    mFrameAnimations[mCurrentAnimation].PlayCount = 0;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Texture"></param>
        public AnimatedSprite(Texture2D Texture)
        {
            mTexture = Texture;
        }

        /// <summary>
        /// Updating the rotation
        /// </summary>
        void UpdateRotation()
        {
            if(mRotateByPosition)
            {
                mRotation = (float)Math.Atan2(mPosition.Y - mLastPosition.Y, mPosition.X- mLastPosition.X);
            }
        }

        /// <summary>
        /// Add Animation of the Sprite.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Frames"></param>
        /// <param name="FrameLength"></param>
        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength)
        {
            mFrameAnimations.Add(Name, new FrameAnimation(X,Y,Width,Height,Frames,FrameLength));
            mWidth = Width;
            mHeight = Height;
            mCenter = new Vector2(mWidth/2,mHeight/2);
        }

        /// <summary>
        /// Add next animation with next animation name.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Frames"></param>
        /// <param name="FrameLength"></param>
        /// <param name="NextAnimation"></param>
        public void AddAnimation(string Name, int X, int Y, int Width, int Height, 
                                 int Frames, float FrameLength, string NextAnimation)
        {
            mFrameAnimations.Add(Name, new FrameAnimation(X,Y,Width,Height,Frames,FrameLength,NextAnimation));
            mWidth = Width;
            mHeight = Height;
            mCenter = new Vector2(mWidth/2,mHeight/2);
        }

        /// <summary>
        /// Get animation frame by name or nothing.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public FrameAnimation GetAnimationByName(string Name)
        {
            if(mFrameAnimations.ContainsKey(Name))
                return mFrameAnimations[Name];
            else
                return null;
        }

        /// <summary>
        /// Move the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveBy(int x, int y)
        {
            mLastPosition = mPosition;
            mPosition.X += x;
            mPosition.Y += y;
            UpdateRotation();
        }

        /// <summary>
        /// Update the animation sprite.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Check if need to update
            if(mIsAnimating)
            {
                //If there is nothing.
                if(mCurrentAnimation == null)
                {
                    //If we have an animation associated with this sprite.
                    if(mFrameAnimations.Count > 0)
                    {
                        string[] keys = new string[mFrameAnimations.Count];
                        mFrameAnimations.Keys.CopyTo(keys,0);
                        mCurrentAnimation = keys[0];
                    }
                    else
                        return;
                }
            }

            //Update with animation.
            CurrentFrameAnimation.Update(gameTime);
            
            //Check if there is a "followup" animation named for this animation.
            if(!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
            {
                //If currently playing animation has completed a loop.
                if(CurrentFrameAnimation.PlayCount > 0)
                    //Set up the next animation.
                    CurrentAnimation = CurrentFrameAnimation.NextAnimation;
            }
        }

        /// <summary>
        /// Draw the Sprite.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="XOffset"></param>
        /// <param name="YOffset"></param>
        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            if(mIsAnimating)
            {
                spriteBatch.Draw(mTexture, (mPosition + new Vector2(XOffset, YOffset) + mCenter),
                                 CurrentFrameAnimation.FrameRectangle, mColorTint, mRotation,
                                 mCenter, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
