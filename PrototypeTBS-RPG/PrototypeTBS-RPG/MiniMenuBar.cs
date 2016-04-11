using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class MiniMenuBar : GameObject
    {
        public bool isSelected
        {
            get
            {
                return texture == selectedTexture;
            }

            set
            {
                if (value == true)
                    texture = selectedTexture;
                else texture = defaultTexture;
            }
        }

        private EventHandler menuEvent;
        private Texture2D defaultTexture;
        private Texture2D selectedTexture;
        private SpriteFont font;
        private string text;

        public MiniMenuBar(ContentManager content, string text, EventHandler menuEvent)
            : base(content.Load<Texture2D>("Misc/MiniMenuBar"))
        {
            this.menuEvent = menuEvent;
            this.text = text;
            this.texture = texture;
            defaultTexture = texture;
            selectedTexture = content.Load<Texture2D>("Misc/MiniMenuBar-Selected");
            font = content.Load<SpriteFont>("Fonts/MiniMenuBarFont");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            spritebatch.DrawString(font, text, 
                new Vector2(position.X - font.MeasureString(text).X / 2, position.Y - font.MeasureString(text).Y / 2),
                Color.Black);
        }

        public void MenuEvent()
        {
            menuEvent.Invoke(this, new EventArgs());
        }
    }
}
