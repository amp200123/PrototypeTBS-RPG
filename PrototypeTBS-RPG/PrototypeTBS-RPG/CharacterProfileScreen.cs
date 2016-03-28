using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
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

            descriptionFont = content.Load<SpriteFont>("ProfileDescriptionFont");
            largeFont = content.Load<SpriteFont>("ProfileLargeFont");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //TODO: add background or something to style it up

            //Find string represpentation of specialization
            string spec;
            if (character.spec is Knight) 
                spec = "Knight";
            else 
            {
                spec = "Unknown";
            }


            DrawText(spritebatch, largeFont, character.name, new Vector2(Game1.WINDOW_WIDTH / 2, 20));
            DrawText(spritebatch, descriptionFont, "Lvl " + character.level + " " + spec,
                new Vector2(Game1.WINDOW_WIDTH / 2, 35));

        }

        private void DrawText(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 pos)
        {
            spritebatch.DrawString(font, text,
                new Vector2(pos.X - font.MeasureString(text).X / 2, pos.Y - font.MeasureString(text).Y / 2),
                Color.Black);
        }
    }
}
