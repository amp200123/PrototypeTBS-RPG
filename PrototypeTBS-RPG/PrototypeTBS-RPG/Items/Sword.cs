using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG.Items
{
    class Sword : Weapon
    {
        public Sword(Texture2D texture, string name, int damage, int crit, int accuracy)
            : base(texture, name, weaponType.sword, damage, crit, accuracy, 1, 1)
        {

        }
    }
}
