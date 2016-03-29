using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class Axe : Weapon
    {
        public Axe(Texture2D texture, string name, int damage, int crit, int accuracy)
            : base(texture, name, weaponType.axe, damage, crit, accuracy, 1, 1)
        {

        }
    }
}
