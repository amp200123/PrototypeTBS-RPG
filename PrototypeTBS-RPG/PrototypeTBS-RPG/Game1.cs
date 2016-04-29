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

        //Save data
        public static List<Character> playerParty;
        public static int currentLevel = 1;

        //Static Items
        public static Weapon IronSword;
        public static Weapon SteelSword;
        public static Weapon IronLance;
        public static Weapon SteelLance;
        public static Weapon IronAxe;
        public static Weapon SteelAxe;
        public static Weapon IronBow;
        public static Weapon SteelBow;
        public static Weapon FireTome;
        public static Weapon LightningTome;
        public static Weapon CorruptTome;
        public static Staff HealStaff;
        public static Tonic HealTonic;
        public static Tonic Elixir;
        public static Weapon LightLance;

        //Static Specializations
        public static Specialization Knight;
        public static Specialization SpearFighter;
        public static Specialization Warrior;
        public static Specialization Archer;
        public static Specialization Swordsman;
        public static Specialization Cavalier;
        public static Specialization Mage;
        public static Specialization Priest;
        public static Specialization Shaman;
        public static Specialization Cleric;
        public static Specialization FalcoKnight;

        //Static Characters
        public static Character Seth;
        public static Character Clarisa;
        public static Character Violet;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MainMenuScreen mainMenuScreen;
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
            SteelSword = new Weapon(Content.Load<Texture2D>("Items/SteelSword"), "Steel Sword", weaponType.sword, 6, 0, 95, 1, 1, false);
            IronLance = new Weapon(Content.Load<Texture2D>("Items/IronLance"), "Iron Lance", weaponType.lance, 5, 0, 90, 1, 1, false);
            SteelLance = new Weapon(Content.Load<Texture2D>("Items/SteelLance"), "Steel Lance", weaponType.lance, 7, 0, 85, 1, 1, false);
            IronAxe = new Weapon(Content.Load<Texture2D>("Items/IronAxe"), "Iron Axe", weaponType.axe, 6, 0, 80, 1, 1, false);
            SteelAxe = new Weapon(Content.Load<Texture2D>("Items/SteelAxe"), "Steel Axe", weaponType.axe, 8, 0, 75, 1, 1, false);
            IronBow = new Weapon(Content.Load<Texture2D>("Items/IronBow"), "Iron Bow", weaponType.bow, 4, 0, 100, 2, 2, false);
            SteelBow = new Weapon(Content.Load<Texture2D>("Items/SteelBow"), "Steel Bow", weaponType.bow, 6, 0, 100, 2, 2, false);
            FireTome = new Weapon(Content.Load<Texture2D>("Items/FireTome"), "Fire", weaponType.elementalMagic, 5, 0, 90, 1, 2, true);
            LightningTome = new Weapon(Content.Load<Texture2D>("Items/LightningTome"), "Lighting", weaponType.lightMagic, 4, 0, 100, 1, 2, true);
            CorruptTome = new Weapon(Content.Load<Texture2D>("Items/CorruptTome"), "Corrupt", weaponType.darkMagic, 6, 0, 70, 1, 2, true);
            HealStaff = new Staff(Content.Load<Texture2D>("Items/Heal"), "Heal", 10, 1, 1);
            HealTonic = new Tonic(Content.Load<Texture2D>("Items/Tonic"), "Tonic", 10);
            Elixir = new Tonic(Content.Load<Texture2D>("Items/Elixir"), "Elixir", 1000);
            LightLance = new Weapon(Content.Load<Texture2D>("Items/LightLance"), "Light Lance", weaponType.lance, 3, 5, 100, 1, 1, false);

            Knight = new Specialization(Content.Load<Texture2D>("Sprites/Knight"), "Knight", false,
                new List<weaponType>() { weaponType.sword }, 16, 6, 1, 3, 5, 4, 8, 3, 5);
            SpearFighter = new Specialization(Content.Load<Texture2D>("Sprites/SpearFighter"), "Spear Fighter", false,
                new List<weaponType>() { weaponType.lance }, 14, 7, 4, 6, 5, 7, 5, 3, 5);
            Warrior = new Specialization(Content.Load<Texture2D>("Sprites/Warrior"), "Warrior", false,
                new List<weaponType>() { weaponType.axe }, 18, 7, 0, 3, 2, 1, 6, 2, 5);
            Archer = new Specialization(Content.Load<Texture2D>("Sprites/Archer"), "Archer", false,
                new List<weaponType>() { weaponType.bow }, 11, 7, 3, 7, 6, 6, 3, 8, 5);
            Swordsman = new Specialization(Content.Load<Texture2D>("Sprites/Swordsman"), "Swordsman", false,
                new List<weaponType>() { weaponType.sword }, 11, 6, 2, 8, 6, 9, 4, 5, 5);
            Cavalier = new Specialization(Content.Load<Texture2D>("Sprites/Cavalier"), "Cavalier", false,
                new List<weaponType>() { weaponType.sword, weaponType.lance }, 15, 6, 1, 7, 6, 5, 6, 2, 6);
            Mage = new Specialization(Content.Load<Texture2D>("Sprites/Mage"), "Mage", false,
                new List<weaponType>() { weaponType.elementalMagic }, 12, 2, 7, 6, 4, 5, 3, 7, 5);
            Priest = new Specialization(Content.Load<Texture2D>("Sprites/Priest"), "Priest", false,
                new List<weaponType>() { weaponType.lightMagic }, 11, 0, 7, 5, 7, 7, 2, 8, 5);
            Shaman = new Specialization(Content.Load<Texture2D>("Sprites/Shaman"), "Shaman", false,
                new List<weaponType>() { weaponType.darkMagic }, 14, 4, 6, 4, 6, 4, 4, 6, 5);
            Cleric = new Specialization(Content.Load<Texture2D>("Sprites/Cleric"), "Cleric", false,
                new List<weaponType>() { weaponType.staff }, 12, 3, 7, 4, 3, 7, 2, 7, 5);
            FalcoKnight = new Specialization(Content.Load<Texture2D>("Sprites/FalcoKnight"), "FalcoKnight", false,
                new List<weaponType>() { weaponType.lance }, 12, 5, 4, 9, 7, 7, 3, 8, 7, true);

            Seth = new Character(Content, "Seth", Knight, alliances.player, 5, 25, 12, 0, 6, 9, 8, 15, 2, 5,
                .7f, .5f, 0f, .3f, .35f, .3f, .65f, .15f, new List<Item>() { SteelSword, HealTonic });
            Clarisa = new Character(Content, "Clarisa", Cleric, alliances.player, .4f, .1f, .8f, .25f, .4f, .6f, .2f, .7f, 
                new List<Item>() { HealStaff, Elixir, HealTonic });
            Violet = new Character(Content, "Violet", FalcoKnight, alliances.player, .65f, .6f, .2f, .8f, .55f, .8f, .45f, .75f,
                new List<Item>() { LightLance, HealTonic });


            //Temp
            playerParty = new List<Character>() { Seth, Clarisa, Violet };


            mainMenuScreen = new MainMenuScreen(Content, new EventHandler(MainMenuScreenEvent));
            gameScreen = new GameScreen(Content, new EventHandler(GameScreenEvent), "testLevel");

            currentScreen = mainMenuScreen;
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

        private void MainMenuScreenEvent(object sender, EventArgs e)
        {
            switch (mainMenuScreen.selectedOption)
            {
                case 1: //Play
                    currentScreen = new PreGameScreen(Content, new EventHandler(PreGameScreenEvent), gameScreen);
                    break;
                case 2: //Options

                    break;
                case 3: //Exit
                    Environment.Exit(1);
                    break;
            }
        }

        private void PreGameScreenEvent(object sender, EventArgs e)
        {
            if ((currentScreen as PreGameScreen).game != null)
            {
                gameScreen = (currentScreen as PreGameScreen).game;
                currentScreen = gameScreen;
            }
            else currentScreen = mainMenuScreen;
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
