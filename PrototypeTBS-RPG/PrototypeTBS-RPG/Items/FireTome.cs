﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class FireTome : Weapon
    {
        public FireTome(ContentManager content)
            : base(content.Load<Texture2D>("Items/FireTome"), "Fire", weaponType.elementalMagic, 4, 0, 90, 1, 2)
        {
            magicWeapon = true;
        }
    }
}
