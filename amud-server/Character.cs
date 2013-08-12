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
        public string name { get; set; }
        public string description { get; set; }

        public void takeDamage(int damage)
        {
            stats.health -= damage;
        }
    }
}
