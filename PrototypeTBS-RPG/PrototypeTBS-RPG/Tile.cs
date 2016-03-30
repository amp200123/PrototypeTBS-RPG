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
        public Character charOnTile 
        {
            get
            {
                return character;
            }

            set
            {
                if (value != null)
                {
                    if (value.tile != null)
                        value.tile.charOnTile = null;

                    if (character != null)
                    {
                        character.tile = null;
                    }

                    character = value;
                    character.tile = this;
                }
                else
                {
                    character = null;
                }
            }
        }

        public bool isSelected;
        public bool attackable;
        public bool movable;

        private Character character;
        private Texture2D selectedTile;
        private Texture2D attackableTile;
        private Texture2D movableTile;

        public Tile(ContentManager content, Texture2D texture, int defense, int health, int movement) : base(texture)
        {
            this.defense = defense;
            this.health = health;
            this.movement = movement;

            selectedTile = content.Load<Texture2D>("Tiles/SelectedTile");
            attackableTile = content.Load<Texture2D>("Tiles/AttackableTile");
            movableTile = content.Load<Texture2D>("Tiles/MovableTile");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);


            if (attackable)
            {
                spritebatch.Draw(attackableTile, position, new Rectangle(0, 0, attackableTile.Width, attackableTile.Height),
                    Color.White, rotation, new Vector2(Width / 2, Height / 2), scale, SpriteEffects.None, 1);
            }

            if (movable)
            {
                spritebatch.Draw(movableTile, position, new Rectangle(0, 0, movableTile.Width, movableTile.Height),
                    Color.White, rotation, new Vector2(Width / 2, Height / 2), scale, SpriteEffects.None, 1);
            }

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
