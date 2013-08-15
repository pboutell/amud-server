using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class CharacterStats
    {
        public int maxHealth { get; set; }
        public int health { get; set; }

        public int maxMana { get; set; }
        public int mana { get; set; }

        public CharacterStats(int maxHealth, int maxMana)
        {
            this.maxHealth = maxHealth;
            this.maxMana = maxMana;

            this.health = maxHealth;
            this.mana = maxMana;
        }
    }
}
