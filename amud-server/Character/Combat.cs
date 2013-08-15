﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Combat
    {
        public bool isFighting;
        public Character target;

        private Character character;

        public Combat(Character character)
        {
            this.character = character;
            this.target = null;
        }

        public int attack(Character target)
        {
            Random random = new Random();
            int damageDone = 0;
            
            target.takeDamage(character, damageDone = random.Next(2, 20));

            return damageDone;
        }
    }
}