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

//TifaZell Stuff
using TifaZell.Graphics;

namespace TifaZell.GameSystem.GameObjects
{
    public class MainCharacter
    {
        private int mHealth; //the health of character.
        private Texture2D mTexture; //The texture of the character.
        private Vector2 mPosition; //Position of character.
        private AnimatedSprite mSprite; //The animated sprite.

        /// <summary>
        /// Setup the main character.
        /// </summary>
        public MainCharacter()
        {
            mHealth = 10;
            mTexture = MainGame.mGame.Content.Load<Texture2D>("Textures/MainCharacter");
            mSprite = new AnimatedSprite(mTexture);
            mPosition = new Vector2();

            mSprite.AddAnimation("Move", 0, 192, 32, 64, 6, 0.25f);
            mSprite.CurrentAnimation = "Move";
        }

        /// <summary>
        /// Drawing the Main Character.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            mSprite.Draw(MainGame.mSpriteBatch, 0, 0);
            //MainGame.mSpriteBatch.Draw(mTexture, mPosition, Color.White);
        }

        /// <summary>
        /// Updating Loop.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            mSprite.Update(gameTime);
        }

        //=================//
        //Set/Get Accessory//
        //=================//
        /// <summary>
        /// 2D Vector Position
        /// </summary>
        public Vector2 Positon
        {
            set 
            { 
                mPosition = value;
                mSprite.Position = mPosition;
            }
            get { return mPosition; }
        }
    }
}
