using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    enum weaponType
    {
        Lance,
        Sword,
        Axe,
        Bow
    }

    abstract class Weapon : Item
    {
        public weaponType type { get; protected set; }


    }
}
