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
        //One more here

        none
    }

    abstract class Specialization
    {
        public List<weaponType> weaponProfs { get; protected set; }
        public List<specializations> promotions { get; protected set; }

    }
}
