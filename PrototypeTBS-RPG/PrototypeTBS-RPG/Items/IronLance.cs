using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class IronLance : Weapon
    {
        public IronLance(ContentManager content)
            : base(content.Load<Texture2D>("Weapons/IronLance"), "Iron Lance", weaponType.lance, 5, 0, 90, 1, 1)
        {

        }
    }
}
