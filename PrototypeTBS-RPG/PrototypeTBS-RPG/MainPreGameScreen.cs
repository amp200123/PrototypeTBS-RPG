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
    class MainPreGameScreen : Screen
    {
        public int selectedOption { get; private set; }

        private Rectangle startRec, selectCharRec, backRec;
        private SpriteFont optionsFont;

        private MouseState oldMouseState = Mouse.GetState();

        public MainPreGameScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            optionsFont = content.Load<SpriteFont>("Fonts/MenuBarFont");

            //Default selected option is 0; none
            selectedOption = 0;

            startRec = new Rectangle((int)(Game1.WINDOW_WIDTH / 2) - (int)(optionsFont.MeasureString("Start").X / 2),
                200 - (int)(optionsFont.MeasureString("Start").Y / 2), (int)optionsFont.MeasureString("Start").X,
                (int)optionsFont.MeasureString("Start").Y);
            selectCharRec = new Rectangle((int)(Game1.WINDOW_WIDTH / 2) - (int)(optionsFont.MeasureString("Select Characters").X / 2),
                250 - (int)(optionsFont.MeasureString("Select Characters").Y / 2), (int)optionsFont.MeasureString("Select Characters").X,
                (int)optionsFont.MeasureString("Select Characters").Y);
            backRec = new Rectangle((int)(Game1.WINDOW_WIDTH / 2) - (int)(optionsFont.MeasureString("Back").X / 2),
                300 - (int)(optionsFont.MeasureString("Back").Y / 2), (int)optionsFont.MeasureString("Back").X,
                (int)optionsFont.MeasureString("Back").Y);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            if (startRec.Contains(newMouseState.Position))
                selectedOption = 1;
            else if (selectCharRec.Contains(newMouseState.Position))
                selectedOption = 2;
            else if (backRec.Contains(newMouseState.Position))
                selectedOption = 3;
            else selectedOption = 0;


            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released
                && selectedOption != 0)
                screenEvent.Invoke(this, new EventArgs());

            oldMouseState = newMouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            Color startColor = Color.Black;
            Color selectCharColor = Color.Black;
            Color backColor = Color.Black;

            switch (selectedOption)
            {
                case 1:
                    startColor = Color.Red;
                    break;
                case 2:
                    selectCharColor = Color.Red;
                    break;
                case 3:
                    backColor = Color.Red;
                    break;
            }

            spritebatch.DrawString(optionsFont, "Start", startRec.Location.ToVector2(), startColor);
            spritebatch.DrawString(optionsFont, "Select Characters", selectCharRec.Location.ToVector2(), selectCharColor);
            spritebatch.DrawString(optionsFont, "Back", backRec.Location.ToVector2(), backColor);
        }
    }
}
