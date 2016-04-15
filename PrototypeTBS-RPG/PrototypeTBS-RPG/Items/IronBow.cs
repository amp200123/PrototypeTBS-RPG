using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Items
{
    class IronBow : Weapon
    {

        public IronBow(ContentManager content)
            : base(content.Load<Texture2D>("Items/IronBow"), "Iron Bow", weaponType.bow, 4, 0, 100, 2, 2)
        {

        }
    }
}
