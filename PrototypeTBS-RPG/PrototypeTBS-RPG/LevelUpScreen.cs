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
    class LevelUpScreen : Screen
    {
        private Character character;
        private SpriteFont descriptionFont;
        private SpriteFont largeFont;

        public LevelUpScreen(Character character, ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            this.character = character;

            descriptionFont = content.Load<SpriteFont>("Fonts/ProfileDescriptionFont");
            largeFont = content.Load<SpriteFont>("Fonts/ProfileLargeFont");
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().RightButton == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                screenEvent.Invoke(this, new EventArgs());
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            DrawText(spritebatch, largeFont, "LEVEL UP!", new Vector2(Game1.WINDOW_WIDTH / 2, 40));
            DrawText(spritebatch, largeFont, character.name, new Vector2(Game1.WINDOW_WIDTH / 2, 80));
        }
        private void DrawText(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 pos)
        {
            spritebatch.DrawString(font, text,
                new Vector2(pos.X - font.MeasureString(text).X / 2, pos.Y - font.MeasureString(text).Y / 2),
                Color.Black);
        }
    }
}
