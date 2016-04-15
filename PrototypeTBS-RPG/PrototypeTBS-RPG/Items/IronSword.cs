using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class IronSword : Weapon
    {
        public IronSword(ContentManager content)
            : base(content.Load<Texture2D>("Items/IronSword"), "Iron Sword", weaponType.sword, 4, 0, 100, 1, 1)
        {

        }
    }
}
