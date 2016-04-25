using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class MainMenuScreen : Screen
    {
        private const int OPTIONS_COUNT = 3; // Play, Options, Exit

        public int selectedOption { get; private set; }

        private SpriteFont optionsFont;

        private KeyboardState oldState = Keyboard.GetState();

        public MainMenuScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            optionsFont = content.Load<SpriteFont>("Fonts/MenuBarFont");

            //Defualt selected option
            selectedOption = 0;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                selectedOption++;
                selectedOption %= OPTIONS_COUNT;
            }
            else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                selectedOption--;
                if (selectedOption < 0)
                    selectedOption = OPTIONS_COUNT - 1;
            }

            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                screenEvent.Invoke(this, new EventArgs());

            oldState = newState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            Color playColor = Color.Black;
            Color optionsColor = Color.Black;
            Color exitColor = Color.Black;

            switch (selectedOption)
            {
                case 0:
                    playColor = Color.Red;
                    break;
                case 1:
                    optionsColor = Color.Red;
                    break;
                case 2:
                    exitColor = Color.Red;
                    break;
            }

            spritebatch.DrawString(optionsFont, "Play", new Vector2(Game1.WINDOW_WIDTH / 2 - optionsFont.MeasureString("Play").X / 2,
                200 - optionsFont.MeasureString("Play").Y / 2), playColor);
            spritebatch.DrawString(optionsFont, "Options", new Vector2(Game1.WINDOW_WIDTH / 2 - optionsFont.MeasureString("Options").X / 2,
                250 - optionsFont.MeasureString("Options").Y / 2), optionsColor);
            spritebatch.DrawString(optionsFont, "Exit", new Vector2(Game1.WINDOW_WIDTH / 2 - optionsFont.MeasureString("Exit").X / 2,
                300 - optionsFont.MeasureString("Exit").Y / 2), exitColor);

        }
    }
}
