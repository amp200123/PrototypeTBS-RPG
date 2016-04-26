using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class CharacterBar : GameObject
    {
        public bool selected;

        public Character character { get; private set; }

        private SpriteFont font;

        public CharacterBar(ContentManager content, Character character) 
            : base(content.Load<Texture2D>("Misc/CharacterBar"))
        {
            this.character = character;
            font = content.Load<SpriteFont>("Fonts/ProfileLargeFont");
        }

        public CharacterBar(ContentManager content)
            : base(content.Load<Texture2D>("Misc/CharacterBar"))
        {
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            if (character != null)
            {
                Color color;
                if (selected)
                    color = Color.White;
                else color = Color.Gray;

                spritebatch.Draw(character.texture, position - new Vector2(texture.Width / 4, 0),
                    new Rectangle(0, 0, character.texture.Width, character.texture.Height), color, 0,
                    new Vector2(character.texture.Width / 2, character.texture.Height / 2), 1, SpriteEffects.None, 0);

                spritebatch.DrawString(font, character.name, position - new Vector2(0, font.MeasureString(character.name).Y / 2), Color.Black);
            }
        }
    }
}
