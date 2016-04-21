using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class Staff : Weapon
    {
        public int healAmount { get; private set; }

        public Staff(Texture2D texture, string name, int healAmount, int minRange, int maxRange)
            : base(texture, name, weaponType.staff, 0, 0, 100, minRange, maxRange, true)
        {
            this.healAmount = healAmount;
        }

        public void Heal(Character user, Character target)
        {
            target.currentHp += user.magic + healAmount;

            if (target.currentHp > target.hp)
                target.currentHp = target.hp;

            user.GiveExp(10 + (int)Math.Ceiling(healAmount / 10f));
        }
    }
}
