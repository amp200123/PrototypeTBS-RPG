using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class TileBar : GameObject
    {
        private Tile tile;
        private SpriteFont font;
        private SpriteFont subFont;

        public TileBar(ContentManager content, Tile tile)
            : base(content.Load<Texture2D>("Misc/TileBar"))
        {
            this.tile = tile;
            font = content.Load<SpriteFont>("Fonts/ProfileDescriptionFont");
            subFont = content.Load<SpriteFont>("Fonts/ItemDescriptionFont");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            spritebatch.DrawString(font, tile.name,
                new Vector2(position.X - font.MeasureString(tile.name).X / 2, (position.Y - Height / 4) - font.MeasureString(tile.name).Y / 2),
                Color.Black);

            string text = "Dfs: " + tile.defense + " Hp: " + tile.health + " Mov: ";

            if (tile.accessible)
                text += tile.movement;
            else text += "n/a";

            spritebatch.DrawString(subFont, text,
                new Vector2(position.X - subFont.MeasureString(text).X / 2, (position.Y + Height / 4) - subFont.MeasureString(text).Y / 2),
                Color.Black);
        }
    }
}
