using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrototypeTBS_RPG.Specializations;

namespace PrototypeTBS_RPG.Characters
{
    class DefaultEnemy : Character
    {
        public DefaultEnemy(ContentManager content, specializations spec, int level, List<Item> inventory = null)
            : base(content, "Enemy", alliances.enemy)
        {
            this.level = level;

            switch (spec)
            {
                case (specializations.archer):
                    this.spec = new Archer(content);

                    break;

                case (specializations.spearFighter):
                    this.spec = new SpearFighter(content);

                    break;

                default: // Knight
                    this.spec = new Knight(content);

                    break;
            }

            // Get default stats from Specialization

            hp = (int)Math.Round(level * (this.spec.hp / 20f) + this.spec.hp);
            strength = (int)Math.Round(level * (this.spec.strength / 10f) + this.spec.strength);
            magic = (int)Math.Round(level * (this.spec.magic / 10f) + this.spec.magic);
            strength = (int)Math.Round(level * (this.spec.strength / 10f) + this.spec.strength);
            speed = (int)Math.Round(level * (this.spec.speed / 10f) + this.spec.speed);
            skill = (int)Math.Round(level * (this.spec.skill / 10f) + this.spec.skill);
            luck = (int)Math.Round(level * (this.spec.luck / 10f) + this.spec.luck);
            defence = (int)Math.Round(level * (this.spec.defence / 10f) + this.spec.defence);
            resistance = (int)Math.Round(level * (this.spec.resistance / 10f) + this.spec.resistance);
            movement = this.spec.movement;

            currentHp = hp;

            //Inventory handling

            this.inventory = inventory;

            if (this.inventory == null)
                this.inventory = new List<Item>();
            else if (this.inventory.Count > 0 && this.inventory[0] is Weapon)
                Equip(this.inventory[0] as Weapon);
        }
    }
}
