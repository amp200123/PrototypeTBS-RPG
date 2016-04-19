using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    class Specialization
    {
        public Texture2D sprite { get; protected set; }
        public List<weaponType> weaponProfs { get; protected set; }
        public List<specializations> promotions { get; protected set; }
        public bool isAdvancedSpec { get; protected set; }
        public string name { get; protected set; }

        //Base specialization stats
        public int hp { get; private set; }
        public int strength { get; private set; }
        public int magic { get; private set; }
        public int speed { get; private set; }
        public int skill { get; private set; }
        public int luck { get; private set; }
        public int defense { get; private set; }
        public int resistance { get; private set; }
        public int movement { get; private set; }

        public Specialization(Texture2D sprite, string name, bool isAdvanced, List<weaponType> weaponProfs, int hp,
            int strength, int magic, int speed, int skill, int luck, int defense, int resistance, int movement)
        {
            this.sprite = sprite;
            this.name = name;
            this.hp = hp;
            this.strength = strength;
            this.magic = magic;
            this.speed = speed;
            this.skill = skill;
            this.luck = luck;
            this.defense = defense;
            this.resistance = resistance;
            this.movement = movement;

            this.weaponProfs = weaponProfs;
            promotions = new List<specializations>();
        }

        public void Draw(SpriteBatch spritebatch, Vector2 position, Color color)
        {
            spritebatch.Draw(sprite, position, new Rectangle(0, 0, sprite.Width, sprite.Height), color, 0,
                    new Vector2(sprite.Width / 2, sprite.Height / 2), 1, SpriteEffects.None, 1);
        }
    }
}
