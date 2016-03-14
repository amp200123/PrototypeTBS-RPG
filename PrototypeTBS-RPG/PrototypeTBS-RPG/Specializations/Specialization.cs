using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    enum specializations
    {
        //Basic
        knight,
        spearFighter,
        warrior,
        archer,
        swordsman,
        cavalier,
        cleric,
        priest,
        mage,
        shaman,
        falcoknight,
        dracoknight,
        //Advanced
        greatKnight,
        spearMaster, //More stuffs
        berzerker,
        hunter,
        saint,
        bishop,
        exorcist,
        sage,
        mageKnight,
        sorcerer,
        darkFalcon,
        dragonLord,
        //One more thing here

        none
    }

    abstract class Specialization
    {
        public List<weaponType> weaponProfs { get; protected set; }
        public List<specializations> promotions { get; protected set; }
        public bool isAdvancedSpec { get; protected set; }

        //Base specialization stats
        public int hp { get; private set; }
        public int strength { get; private set; }
        public int magic { get; private set; }
        public int speed { get; private set; }
        public int skill { get; private set; }
        public int luck { get; private set; }
        public int defence { get; private set; }
        public int resistance { get; private set; }
        public int movement { get; private set; }

    }
}
