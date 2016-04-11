using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class InventoryBar : GameObject
    {
        public Item item { get; private set; }

        public bool isSelected = false;

        private Texture2D defaultTexture, selectedTexture;
        private SpriteFont font;
        private Texture2D itemBase;

        public InventoryBar(ContentManager content, Item item, Vector2 position)
            : base(content.Load<Texture2D>("Misc/InventoryBar"))
        {
            defaultTexture = content.Load<Texture2D>("Misc/InventoryBar");
            selectedTexture = content.Load<Texture2D>("Misc/InventoryBar-Selected");

            this.item = item;
            this.position = position;
            font = content.Load<SpriteFont>("Fonts/ProfileDescriptionFont");
            itemBase = content.Load<Texture2D>("Misc/ItemBase");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (isSelected)
                texture = selectedTexture;
            else texture = defaultTexture;

            base.Draw(spritebatch);

            if (item != null)
            {
                spritebatch.Draw(itemBase, position - new Vector2(110, 0), null, Color.White, 0,
                    new Vector2(itemBase.Width / 2, itemBase.Height / 2), 1, SpriteEffects.None, 0);
                spritebatch.Draw(item.texture, position - new Vector2(110, 0), null, Color.White, 0,
                    new Vector2(item.texture.Width / 2, item.texture.Height / 2), 1, SpriteEffects.None, 0); 
                spritebatch.DrawString(font, item.name, position - new Vector2(90, font.MeasureString(item.name).Y / 2),
                    Color.White);
            }
        }
    }
}
