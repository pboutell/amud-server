using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Character
    {
        public CharacterStats stats;
        public Inventory items = new Inventory();

        public string name { get; set; }
        public string description { get; set; }

        public void takeDamage(int damage)
        {
            stats.health -= damage;
        }
    }
}
