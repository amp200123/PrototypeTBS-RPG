using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG.Specializations
{
    class Archer : Specialization
    {
        public Archer(ContentManager content) 
            : base(content.Load<Texture2D>("Sprites/Archer"), false, new List<weaponType>() {weaponType.bow},
            11, 7, 3, 7, 6, 6, 3, 6, 5)
        {
        }

        public override string ToString()
        {
            return "Archer";
        }
    }
}
