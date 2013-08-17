using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Stats
    {
        public int maxHealth { get; private set; }
        public int health { get; set; }

        public int maxMana { get; private set; }
        public int mana { get; set; }

        public Stats(int maxHealth, int maxMana)
        {
            this.maxHealth = maxHealth;
            this.maxMana = maxMana;

            this.health = maxHealth;
            this.mana = maxMana;
        }
    }
}
