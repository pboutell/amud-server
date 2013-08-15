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

        public void attack(Character target)
        {
            target.takeDamage(character, 20);
        }
    }
}