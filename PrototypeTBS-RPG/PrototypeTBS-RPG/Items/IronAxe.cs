using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class IronAxe : Weapon
    {
        public IronAxe(ContentManager content)
            : base(content.Load<Texture2D>("Items/IronAxe"), "Iron Axe", weaponType.axe, 6, 0, 80, 1, 1)
        {

        }
    }
}
