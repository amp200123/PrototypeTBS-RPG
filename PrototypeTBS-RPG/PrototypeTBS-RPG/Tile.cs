using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PrototypeTBS_RPG
{

    class Tile : GameObject
    {
        public int defense { get; private set; }
        public int health { get; private set; }
        public int movement { get; private set; }

        public Character charOnTile;
        public bool isSelected;

        private Texture2D selectedTile;

        public Tile(ContentManager content, Texture2D texture, int defense, int health, int movement) : base(texture)
        {
            this.defense = defense;
            this.health = health;
            this.movement = movement;

            selectedTile = content.Load<Texture2D>("Misc/SelectedTile");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            if (isSelected)
            {
                spritebatch.Draw(selectedTile, position, new Rectangle(0, 0, selectedTile.Width, selectedTile.Height),
                    Color.White, rotation, new Vector2(Width / 2, Height / 2), scale, SpriteEffects.None, 1);
            }

            if (charOnTile != null)
                charOnTile.Draw(spritebatch, position);
        }
    }
}
