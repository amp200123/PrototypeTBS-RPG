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
        public Character character { get; private set; }

        public bool isSelected = false;

        private Texture2D defaultTexture, selectedTexture;
        private SpriteFont font;
        private SpriteFont subFont;
        private Texture2D itemBase;
        private int invPos;

        public InventoryBar(ContentManager content, Character character, Item item, Vector2 position, int invPos)
            : base(content.Load<Texture2D>("Misc/InventoryBar"))
        {
            defaultTexture = content.Load<Texture2D>("Misc/InventoryBar");
            selectedTexture = content.Load<Texture2D>("Misc/InventoryBar-Selected");

            this.item = item;
            this.character = character;
            this.position = position;
            this.invPos = invPos;

            font = content.Load<SpriteFont>("Fonts/ItemDescriptionFont");
            subFont = content.Load<SpriteFont>("Fonts/SubItemDescriptionFont");
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
                    Color.Black);

                if (item is Weapon)
                {
                    Weapon weapon = item as Weapon;

                    string text = "Ak|" + weapon.damage + "| Hit|" + weapon.accuracy + "| Crt|" + weapon.crit + "| Rng|";

                    if (weapon.minRange != weapon.maxRange)
                        text += weapon.minRange + "-" + weapon.maxRange + "|";
                    else text += weapon.maxRange + "|";

                    spritebatch.DrawString(subFont, text, position - new Vector2(-135 + subFont.MeasureString(text).X,
                        subFont.MeasureString(text).Y / 2), Color.Black);

                    if (character.equipedWeapon == weapon && invPos == 0)
                        spritebatch.DrawString(subFont, "E", position - new Vector2(130,
                            subFont.MeasureString("E").Y / 2), Color.Black);
                }
            }
        }
    }
}
