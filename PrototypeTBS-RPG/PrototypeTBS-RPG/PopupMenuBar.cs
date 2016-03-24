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
        public bool isSelected;

        private Texture2D defaultTexture;
        private Texture2D selectedTexture;
        private SpriteFont font;
        private string text;

        public PopupMenuBar(ContentManager content, string text)
            : base(content.Load<Texture2D>("Misc/ManuBar"))
        {
            this.texture = texture;
            defaultTexture = texture;
            selectedTexture = content.Load<Texture2D>("Misc/MenuBar-Selected");
            font = content.Load<SpriteFont>("");
        }

        public override void Update(GameTime gameTime)
        {
            if (isSelected)
                texture = selectedTexture;
            else texture = defaultTexture;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            spritebatch.DrawString(font, text, 
                new Vector2(position.X - font.MeasureString(text).X / 2, position.Y - font.MeasureString(text).Y / 2),
                Color.Black);
        }
    }
}
