using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG.Specializations
{
    class Knight : Specialization
    {
        public Knight(ContentManager content)
            : base(content.Load<Texture2D>("Sprites/Knight"), false, new List<weaponType>() {weaponType.sword},
            18, 6, 1, 4, 5, 4, 8, 3, 5)
        {
        }

        public override string ToString()
        {
            return "Knight";
        }
    }
}
