using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//TifaZell
using TifaZell.GameSystem;

namespace TifaZell
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZellTifaGame : Microsoft.Xna.Framework.Game
    {
        //NWindow Title Name
        string mWindowTitle = "Tifa Zell Project";

        //Graphics Manager.
        GraphicsDeviceManager mGraphicsManager;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //MainGame mGame = ; //Main Game.

        public ZellTifaGame()
        {
            mGraphicsManager = new GraphicsDeviceManager(this);
            mGraphicsManager.IsFullScreen = false;

            mGraphicsManager.PreferredBackBufferWidth = 1280;
            mGraphicsManager.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            //mGame = new MainGame();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            MainGame.LoadContent(this, spriteBatch);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MainGame.Update(gameTime);
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            MainGame.Draw(gameTime);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
