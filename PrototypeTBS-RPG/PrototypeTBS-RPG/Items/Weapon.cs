﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public weaponType type { get; protected set; }
        public weaponType weakness { get; protected set; }
        public weaponType advantage { get; protected set; }

        public int damage { get; protected set; }
        public int crit { get; protected set; }
        public int minRange { get; protected set; }
        public int maxRange { get; protected set; }
    }
}
