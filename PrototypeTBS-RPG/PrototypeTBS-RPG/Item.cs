using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    abstract class Item
    {
        public Texture2D texture { get; protected set; }
        public string name { get; protected set; }
        public bool usable { get; protected set; }

        public Item(Texture2D texture, string name)
        {
            this.texture = texture;
            this.name = name;
            this.usable = false;
        }

        public virtual void Use(Character character)
        {
        }
    }
}
