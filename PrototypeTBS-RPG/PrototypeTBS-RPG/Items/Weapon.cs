using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG
{
    enum weaponType
    {
        lance,
        sword,
        axe,
        bow,
        lightMagic,
        elementalMagic,
        darkMagic,
        staff,

        none
    }

    abstract class Weapon : Item
    {
        public Texture2D texture { get; protected set; }
        public weaponType type { get; protected set; }
        public weaponType weakness { get; protected set; }
        public weaponType advantage { get; protected set; }

        public string name { get; protected set; }
        public int damage { get; protected set; }
        public int crit { get; protected set; }
        public int accuracy { get; protected set; }
        public int minRange { get; protected set; }
        public int maxRange { get; protected set; }

        public Weapon(Texture2D texture, string name, weaponType type, int damage, int crit, int accuracy, int minRange, int maxRange)
        {
            this.texture = texture;
            this.damage = damage;
            this.crit = crit;
            this.accuracy = accuracy;
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.type = type;

            switch (type)
            {
                case (weaponType.lance):
                    advantage = weaponType.sword;
                    weakness = weaponType.axe;
                    break;
                case (weaponType.sword):
                    advantage = weaponType.axe;
                    weakness = weaponType.lance;
                    break;
                case (weaponType.axe):
                    advantage = weaponType.lance;
                    weakness = weaponType.sword;
                    break;
                case (weaponType.bow):
                    advantage = weaponType.none;
                    weakness = weaponType.none;
                    break;
                case (weaponType.lightMagic):
                    advantage = weaponType.darkMagic;
                    weakness = weaponType.elementalMagic;
                    break;
                case (weaponType.elementalMagic):
                    advantage = weaponType.lightMagic;
                    weakness = weaponType.darkMagic;
                    break;
                case (weaponType.darkMagic):
                    advantage = weaponType.elementalMagic;
                    weakness = weaponType.lightMagic;
                    break;
                case (weaponType.staff):
                    advantage = weaponType.none;
                    weakness = weaponType.none;
                    break;
            }
        }
    }
}
