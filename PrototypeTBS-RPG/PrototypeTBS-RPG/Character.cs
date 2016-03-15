using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG
{
    class Character
    {
        public Random random;
        public Specialization spec { get; private set; }
        public List<Item> inventory { get; private set; }
        public Weapon equipedWeapon { get; private set; }
        public string name { get; private set; }
        public int level { get; private set; }
        public int exp { get; private set; }


        //Stats
        public int hp { get; private set; }
        public int strength { get; private set; }
        public int magic { get; private set; }
        public int speed { get; private set; }
        public int skill { get; private set; }
        public int luck { get; private set; }
        public int defence { get; private set; }
        public int resistance { get; private set; }
        public int movement { get; private set; }

        //Growth rates
        public int hpChance { get; private set; }
        public int attackChance { get; private set; }
        public int magicChance { get; private set; }
        public int speedChance { get; private set; }
        public int skillChance { get; private set; }
        public int luckChance { get; private set; }
        public int defenceChance { get; private set; }
        public int resistanceChance { get; private set; }


        public Character(string name, Specialization spec) //Level 1 char
        {
            random = new Random();
            this.spec = spec;
            this.name = name;

            level = 1;
            hp = spec.hp;
            strength = spec.strength;
            magic = spec.magic;
            speed = spec.hp;
            skill = spec.skill;
            luck = spec.luck;
            defence = spec.defence;
            resistance = spec.resistance;

        }

        public void Draw(SpriteBatch spritebatch, Vector2 position)
        {
            spec.Draw(spritebatch, position);
        }

        private void LevelUp()
        {
            if (exp >= 100)
            {
                level++;

                if (random.Next(100) < hpChance)
                {
                    hp++;
                }
                if (random.Next(100) < attackChance)
                {
                    strength++;
                }
                if (random.Next(100) < magicChance)
                {
                    magic++;
                }
                if (random.Next(100) < speedChance)
                {
                    speed++;
                }
                if (random.Next(100) < skillChance)
                {
                    skill++;
                }
                if (random.Next(100) < luckChance)
                {
                    luck++;
                }
                if (random.Next(100) < defenceChance)
                {
                    defence++;
                }
                if (random.Next(100) < resistanceChance)
                {
                    resistance++;
                }
            }
        }
    }
}
