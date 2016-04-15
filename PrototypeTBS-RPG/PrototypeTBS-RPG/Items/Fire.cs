using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class Fire : Weapon
    {
        public Fire(ContentManager content)
            : base(content.Load<Texture2D>("Items/Fire"), "Fire", weaponType.elementalMagic, 4, 0, 90, 1, 2)
        {
            magicWeapon = true;
        }
    }
}
