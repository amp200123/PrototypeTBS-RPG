using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PrototypeTBS_RPG.Characters
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
        public bool canMove;
        public bool active;
        public alliances alliance { get; protected set; }
        public Specialization spec { get; protected set; }
        public List<Item> inventory { get; protected set; }
        public Weapon equipedWeapon { get; protected set; }
        public string name { get; protected set; }
        public int level { get; protected set; }
        public int exp { get; protected set; }


        //Stats
        public int currentHp
        {
            get { return CurrentHp; }

            set
            {
                CurrentHp = value;
                if (CurrentHp < 0)
                    CurrentHp = 0;
            }
        }
        public int hp { get; protected set; }
        public int strength { get; protected set; }
        public int magic { get; protected set; }
        public int speed { get; protected set; }
        public int skill { get; protected set; }
        public int luck { get; protected set; }
        public int defence { get; protected set; }
        public int resistance { get; protected set; }
        public int movement { get; protected set; }

        //Growth rates
        public int hpChance { get; protected set; }
        public int strengthChance { get; protected set; }
        public int magicChance { get; protected set; }
        public int speedChance { get; protected set; }
        public int skillChance { get; protected set; }
        public int luckChance { get; protected set; }
        public int defenceChance { get; protected set; }
        public int resistanceChance { get; protected set; }

        //Whether or not this character gained this stat last lvl up
        public bool gainedHp { get; protected set; }
        public bool gainedStrength { get; protected set; }
        public bool gainedMagic { get; protected set; }
        public bool gainedSpeed { get; protected set; }
        public bool gainedSkill { get; protected set; }
        public bool gainedLuck { get; protected set; }
        public bool gainedDefense { get; protected set; }
        public bool gainedResistance { get; protected set; }

        protected int CurrentHp;
        protected Texture2D grayHealth, redHealth;

        public Character(ContentManager content, string name, Specialization spec, alliances alliance) //Level 1 char
        {
            random = new Random();
            inventory = new List<Item>();

            grayHealth = content.Load<Texture2D>("Misc/GrayHealthBar");
            redHealth = content.Load<Texture2D>("Misc/RedHealthBar");

            this.alliance = alliance;
            this.spec = spec;
            this.name = name;

            canMove = true;
            active = true;
            level = 1;
            hp = spec.hp;
            strength = spec.strength;
            magic = spec.magic;
            speed = spec.speed;
            skill = spec.skill;
            luck = spec.luck;
            defence = spec.defence;
            resistance = spec.resistance;
            movement = spec.movement;

            currentHp = hp;
        }

        public Character(ContentManager content)
        {
            random = new Random();
            inventory = new List<Item>();

            grayHealth = content.Load<Texture2D>("Misc/GrayHealthBar");
            redHealth = content.Load<Texture2D>("Misc/RedHealthBar");

            canMove = true;
            active = true;
        }

        public void Draw(SpriteBatch spritebatch, Vector2 position)
        {
            if (active)
                spec.Draw(spritebatch, position, Color.White);
            else spec.Draw(spritebatch, position, Color.Gray);

            Vector2 hpPos = position + new Vector2(-redHealth.Width / 2, 13);

            spritebatch.Draw(grayHealth, hpPos, new Rectangle(0, 0, grayHealth.Width, grayHealth.Height),
                Color.White);

            Rectangle redSource = new Rectangle(0, 0, (int)(((float)currentHp / (float)hp) * redHealth.Width),
                redHealth.Height);
            spritebatch.Draw(redHealth, hpPos, redSource, Color.White);
        }

        public bool GiveExp(int xp)
        {
            if (alliance != alliances.player)
                return false;

            exp += xp;

            if (exp >= 100)
            {
                exp -= 100;
                level++;

                if (random.Next(100) < hpChance)
                {
                    hp++;
                    gainedHp = true;
                }
                else gainedHp = false;
                if (random.Next(100) < strengthChance)
                {
                    strength++;
                    gainedStrength = true;
                }
                else gainedStrength = false;
                if (random.Next(100) < magicChance)
                {
                    magic++;
                    gainedMagic = true;
                }
                else gainedMagic = false;
                if (random.Next(100) < speedChance)
                {
                    speed++;
                    gainedSpeed = true;
                }
                else gainedSpeed = false;
                if (random.Next(100) < skillChance)
                {
                    skill++;
                    gainedSkill = true;
                }
                else gainedSkill = false;
                if (random.Next(100) < luckChance)
                {
                    luck++;
                    gainedLuck = true;
                }
                else gainedLuck = false;
                if (random.Next(100) < defenceChance)
                {
                    defence++;
                    gainedDefense = true;
                }
                else gainedDefense = false;
                if (random.Next(100) < resistanceChance)
                {
                    resistance++;
                    gainedResistance = true;
                }
                else gainedResistance = false;

                return true;
            }

            return false;
        }

        public void Equip(Weapon weapon)
        {
            if (spec.weaponProfs.Contains(weapon.type) &&
                inventory.Contains(weapon))
            {
                equipedWeapon = weapon;

                int index = inventory.IndexOf(weapon);

                for (int i = index - 1; i >= 0; i--)
                {
                    inventory[i + 1] = inventory[i];
                }

                inventory[0] = weapon;
            }
        }
        public void Unequip(Weapon weapon)
        {
            if (weapon == equipedWeapon)
                equipedWeapon = null;
        }

        public void Discard(Item item)
        {
            if (inventory.Contains(item))
            {
                if (equipedWeapon == item)
                    equipedWeapon = null;

                inventory.Remove(item);
            }
        }

        /// <summary>
        /// Orders this unit to attack an enemy. Returns state of the enemy
        /// </summary>
        /// <param name="enemy">Enemy character to attack</param>
        /// <returns>Whether or not the enemy has been killed</returns>
        public bool Attack(Character enemy, int range)
        {
            if (equipedWeapon != null && range >= equipedWeapon.minRange && range <= equipedWeapon.maxRange)
            {
                //Find chance to hit enemy
                int hitRate = 100 + skill + (equipedWeapon.accuracy - 100) + (luck / 5)  - (enemy.luck / 5);
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

                    float weaponEffectiveness = 1;

                    if (enemy.equipedWeapon != null)
                    {
                        if (equipedWeapon.advantage == enemy.equipedWeapon.type)
                            weaponEffectiveness = 2;
                        else if (equipedWeapon.weakness == enemy.equipedWeapon.type)
                            weaponEffectiveness = 0.5f;
                    }

                    int damage;

                    if (equipedWeapon.magicWeapon)
                    {
                        damage = (int)(magic + equipedWeapon.damage * weaponEffectiveness - enemy.resistance - enemy.tile.defense);
                    }
                    else damage = (int)(strength + equipedWeapon.damage * weaponEffectiveness - enemy.defence - enemy.tile.defense);

                    if (crit)
                        damage *= 3;

                    enemy.currentHp -= damage;

                    return enemy.currentHp <= 0;
                }
            }

            return false;
        }
    }
}
