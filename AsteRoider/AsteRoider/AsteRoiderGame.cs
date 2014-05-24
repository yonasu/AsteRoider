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

namespace AsteRoider
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AsteRoiderGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        Background background;
        BasicEffect effect;
        Effect effect2;
        //private Ship ship;
        private Ship[] ships;
        private const int NUMSHIPS = 1000;

        public AsteRoiderGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content"; 
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
            camera = new Camera(new Vector3(10.5f, 1.5f, 10.5f), 0, GraphicsDevice.Viewport.AspectRatio, 0.05f, 100f);
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            effect = new BasicEffect(GraphicsDevice);
            background = new Background(GraphicsDevice, Content.Load<Texture2D>("bakgrund2"));
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
            ships = new Ship[NUMSHIPS];
            Random rn = new Random();
            for (int x = 0; x < NUMSHIPS; x++) 
            {
                ships[x] = new Ship(this.GraphicsDevice, Content.Load<Texture2D>("textur"));
                ships[x].id = x;
                ships[x].Position = new Vector3(((float)rn.NextDouble() * 10), 0, ((float)rn.NextDouble() * 10));
            }

                //ship = new Ship(this.GraphicsDevice, Content.Load<Texture2D>("textur"));
            effect2 = Content.Load<Effect>(@"Effects/ShipEffect");
            // TODO: use this.Content to load your game content here
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
            //camera.Position.X
            // TODO: Add your update logic here
            for (int x = 0; x < NUMSHIPS; x++)
            {
                ships[x].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            //ship.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0,0,0));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            background.Draw(camera, effect);
            for (int x = 0; x < NUMSHIPS; x++) 
            {
                ships[x].Draw(camera, effect2);
            }
            //ship.Draw(camera, effect2);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
