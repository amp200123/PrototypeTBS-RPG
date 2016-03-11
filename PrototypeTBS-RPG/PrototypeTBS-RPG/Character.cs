using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class Character
    {

        public Specialization spec { get; private set; }
        public List<Item> inventory { get; private set; }
        public string name { get; private set; }

        public Character(string name, Specialization spec)
        {
            this.spec = spec;
            this.name = name;
        }
    }
}
