using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class Tonic : Item
    {
        public Tonic(ContentManager content)
            : base(content.Load<Texture2D>("Items/Tonic"), "Tonic")
        {
            usable = true;
        }

        public override void Use(Character character)
        {
            character.currentHp += 10;
            if (character.currentHp > character.hp)
                character.currentHp = character.hp;
        }
    }
}
