using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Specializations
{
    class Mage : Specialization
    {
        public Mage(ContentManager content) 
            : base(content.Load<Texture2D>("Sprites/Mage"), false, new List<weaponType>() {weaponType.elementalMagic},
            12, 2, 7, 6, 4, 3, 4, 7, 5)
        {
        }

        public override string ToString()
        {
            return "Mage";
        }
    }
}
