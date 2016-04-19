using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototypeTBS_RPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 480;

        //Static Items
        public static Weapon IronSword;
        public static Weapon IronLance;
        public static Weapon IronAxe;
        public static Weapon IronBow;
        public static Weapon FireTome;
        public static Tonic HealTonic;

        //Static Specializations
        public static Specialization Knight;
        public static Specialization Archer;
        public static Specialization SpearFighter;
        public static Specialization Mage;

        //Static Characters
        public static Character Seth;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameScreen gameScreen;
        Screen currentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            graphics.ApplyChanges();
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

            //Assign static weapons, specs, & characters default values
            IronSword = new Weapon(Content.Load<Texture2D>("Items/IronSword"), "Iron Sword", weaponType.sword, 4, 0, 100, 1, 1, false);
            IronLance = new Weapon(Content.Load<Texture2D>("Items/IronLance"), "Iron Lance", weaponType.lance, 5, 0, 90, 1, 1, false);
            IronBow = new Weapon(Content.Load<Texture2D>("Items/IronBow"), "Iron Bow", weaponType.bow, 4, 0, 100, 2, 2, false);
            IronAxe = new Weapon(Content.Load<Texture2D>("Items/IronAxe"), "Iron Axe", weaponType.axe, 6, 0, 80, 1, 1, false);
            FireTome = new Weapon(Content.Load<Texture2D>("Items/FireTome"), "Fire", weaponType.elementalMagic, 4, 0, 90, 1, 2, true);
            HealTonic = new Tonic(Content);

            Knight = new Specialization(Content.Load<Texture2D>("Sprites/Knight"), "Knight", false,
                new List<weaponType>() { weaponType.sword }, 18, 6, 1, 4, 5, 4, 8, 3, 5);
            SpearFighter = new Specialization(Content.Load<Texture2D>("Sprites/SpearFighter"), "Spear Fighter", false,
                new List<weaponType>() { weaponType.lance }, 14, 7, 3, 6, 5, 7, 5, 3, 5);
            Archer = new Specialization(Content.Load<Texture2D>("Sprites/Archer"), "Archer", false,
                new List<weaponType>() { weaponType.bow }, 12, 7, 3, 7, 6, 6, 3, 8, 5);
            Mage = new Specialization(Content.Load<Texture2D>("Sprites/Mage"), "Mage", false,
                new List<weaponType>() { weaponType.elementalMagic }, 12, 2, 7, 6, 4, 3, 4, 7, 5);

            Seth = new Character(Content, "Seth", Knight, alliances.player, 5, 25, 12, 0, 8, 9, 8, 15, 2, 5,
                .7f, .5f, 0f, .3f, .35f, .3f, .7f, .15f, new List<Item>() { IronSword, HealTonic });


            gameScreen = new GameScreen(Content, new EventHandler(GameScreenEvent), "testLevel");
            currentScreen = gameScreen;
        }

        /// <summary>
        /// Unloadcontent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            currentScreen.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GameScreenEvent(object sender, EventArgs e)
        {
            gameScreen = sender as GameScreen;

            if (e is ProfileEventArgs)
            {
                currentScreen = new CharacterProfileScreen((e as ProfileEventArgs).character,
                    Content, new EventHandler(CharacterProfileScreenEvent));
            }
            else if (e is LevelUpEventArgs)
            {
                currentScreen = new LevelUpScreen((e as LevelUpEventArgs).character,
                    Content, new EventHandler(LevelUpScreenEvent));
            }
        }

        private void CharacterProfileScreenEvent(object sender, EventArgs e)
        {
            currentScreen = gameScreen;
        }

        private void LevelUpScreenEvent(object sender, EventArgs e)
        {
            currentScreen = gameScreen;
        }
    }
}
