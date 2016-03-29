using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PrototypeTBS_RPG.Specializations;

namespace PrototypeTBS_RPG
{
    class CharacterProfileScreen : Screen
    {
        private SpriteFont descriptionFont;
        private SpriteFont largeFont;
        private Character character;

        public CharacterProfileScreen(Character character, ContentManager content, EventHandler screenEvent)
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
            //TODO: add background or something to style it up

            DrawText(spritebatch, largeFont, character.name, new Vector2(Game1.WINDOW_WIDTH / 2, 40));
            DrawText(spritebatch, descriptionFont, "Lvl " + character.level + " " + character.spec.ToString(),
                new Vector2(Game1.WINDOW_WIDTH / 2, 80));

        }

        private void DrawText(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 pos)
        {
            spritebatch.DrawString(font, text,
                new Vector2(pos.X - font.MeasureString(text).X / 2, pos.Y - font.MeasureString(text).Y / 2),
                Color.Black);
        }
    }
}
