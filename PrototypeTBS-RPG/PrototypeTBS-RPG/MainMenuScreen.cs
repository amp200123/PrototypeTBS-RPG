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
        public int selectedOption { get; private set; }

        private Rectangle playRec, optionRec, exitRec;
        private SpriteFont optionsFont;

        private MouseState oldMouseState = Mouse.GetState();

        public MainMenuScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            optionsFont = content.Load<SpriteFont>("Fonts/MenuBarFont");

            //Default selected option is 0; none
            selectedOption = 0;

            playRec = new Rectangle((int)(Game1.WINDOW_WIDTH / 2) - (int)(optionsFont.MeasureString("Play").X / 2),
                200 - (int)(optionsFont.MeasureString("Play").Y / 2), (int)optionsFont.MeasureString("Play").X, 
                (int)optionsFont.MeasureString("Play").Y);
            optionRec = new Rectangle((int)(Game1.WINDOW_WIDTH / 2) - (int)(optionsFont.MeasureString("Options").X / 2),
                250 - (int)(optionsFont.MeasureString("Options").Y / 2), (int)optionsFont.MeasureString("Options").X,
                (int)optionsFont.MeasureString("Options").Y);
            exitRec = new Rectangle((int)(Game1.WINDOW_WIDTH / 2) - (int)(optionsFont.MeasureString("Exit").X / 2),
                300 - (int)(optionsFont.MeasureString("Exit").Y / 2), (int)optionsFont.MeasureString("Exit").X,
                (int)optionsFont.MeasureString("Exit").Y);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            if (playRec.Contains(newMouseState.Position))
                selectedOption = 1;
            else if (optionRec.Contains(newMouseState.Position))
                selectedOption = 2;
            else if (exitRec.Contains(newMouseState.Position))
                selectedOption = 3;
            else selectedOption = 0;

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released
                && selectedOption != 0)
                screenEvent.Invoke(this, new EventArgs());

            oldMouseState = newMouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            Color playColor = Color.Black;
            Color optionsColor = Color.Black;
            Color exitColor = Color.Black;

            switch (selectedOption)
            {
                case 1:
                    playColor = Color.Red;
                    break;
                case 2:
                    optionsColor = Color.Red;
                    break;
                case 3:
                    exitColor = Color.Red;
                    break;
            }

            spritebatch.DrawString(optionsFont, "Play", playRec.Location.ToVector2(), playColor);
            spritebatch.DrawString(optionsFont, "Options", optionRec.Location.ToVector2(), optionsColor);
            spritebatch.DrawString(optionsFont, "Exit", exitRec.Location.ToVector2(), exitColor);
        }
    }
}
