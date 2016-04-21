using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class Tonic : Item
    {
        public int healAmount { get; private set; }

        public Tonic(Texture2D texture, string name, int healAmount)
            : base(texture, name)
        {
            usable = true;
            this.healAmount = healAmount;
        }

        public override void Use(Character character)
        {
            character.currentHp += healAmount;
            if (character.currentHp > character.hp)
                character.currentHp = character.hp;
        }
    }
}
