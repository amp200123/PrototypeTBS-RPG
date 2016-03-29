using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG
{
    enum alliances
    {
        player,
        enemy
    }

    class Character
    {
        public Random random;
        public Tile tile;
        public alliances alliance { get; private set; }
        public Specialization spec { get; private set; }
        public List<Item> inventory { get; private set; }
        public Weapon equipedWeapon { get; private set; }
        public string name { get; private set; }
        public int level { get; private set; }
        public int exp { get; private set; }
        public bool active { get; private set; }


        //Stats
        public int currentHp
        {
            get;

            set
            {
                currentHp = value;
                if (currentHp < 0)
                    currentHp = 0;
            }
        }
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


        public Character(string name, Specialization spec, alliances alliance) //Level 1 char
        {
            random = new Random();
            inventory = new List<Item>();

            this.alliance = alliance;
            this.spec = spec;
            this.name = name;

            active = true;
            level = 1;
            hp = spec.hp;
            strength = spec.strength;
            magic = spec.magic;
            speed = spec.hp;
            skill = spec.skill;
            luck = spec.luck;
            defence = spec.defence;
            resistance = spec.resistance;

            currentHp = hp;
        }

        public void Draw(SpriteBatch spritebatch, Vector2 position)
        {
            if (active)
                spec.Draw(spritebatch, position, Color.White);
            else spec.Draw(spritebatch, position, Color.Gray);
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

        public void Equip(Weapon weapon)
        {
            if (spec.weaponProfs.Contains(weapon.type) &&
                inventory.Contains(weapon))
            {
                equipedWeapon = weapon;
            }
        }

        public void Attack(Character enemy)
        {
            if (equipedWeapon != null)
            {
                //Find chance to hit enemy
                int hitRate = 100 + skill + (equipedWeapon.accuracy - 100) + luck / 4;
                int hitChance = hitRate - (enemy.speed + enemy.luck / 2);

                if (hitChance < 0)
                    hitChance = 0;
                else if (hitChance > 100)
                    hitChance = 100;

                if (random.Next(100) < hitChance)
                {
                    //Successful hit

                    //Find crit chance
                    int critChance = skill / 2 + 5 + equipedWeapon.crit - enemy.luck;
                    if (critChance < 0)
                        critChance = 0;

                    bool crit = random.Next(100) < critChance;

                    float weaponEffectiveness;

                    if (equipedWeapon.advantage == enemy.equipedWeapon.type)
                        weaponEffectiveness = 2;
                    else if (equipedWeapon.weakness == enemy.equipedWeapon.type)
                        weaponEffectiveness = 0.5f;
                    else weaponEffectiveness = 1;

                    int damage = (int)(strength + equipedWeapon.damage * weaponEffectiveness - enemy.defence);

                    if (crit)
                        damage *= 3;

                    enemy.currentHp -= damage;
                }
            }
        }
    }
}
