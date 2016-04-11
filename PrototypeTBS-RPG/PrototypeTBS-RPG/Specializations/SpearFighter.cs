using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG.Specializations
{
    class SpearFighter : Specialization
    {
        public SpearFighter(ContentManager content) 
            : base(content.Load<Texture2D>("Sprites/SpearFighter"), false, new List<weaponType>() { weaponType.lance }, 
            14, 7, 3, 6, 5, 7, 5, 3, 5)
        {
        }

        public override string ToString()
        {
            return "Spear Fighter";
        }
    }
}
