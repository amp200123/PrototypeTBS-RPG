using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PrototypeTBS_RPG
{
    class PopupMenuBar : GameObject
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

        public PopupMenuBar(ContentManager content, string text, EventHandler menuEvent)
            : base(content.Load<Texture2D>("Misc/MenuBar"))
        {
            this.menuEvent = menuEvent;
            this.text = text;
            this.texture = texture;
            defaultTexture = texture;
            selectedTexture = content.Load<Texture2D>("Misc/MenuBar-Selected");
            font = content.Load<SpriteFont>("Fonts/MenuBarFont");
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
