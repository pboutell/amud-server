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

        public void takeDamage(int damage)
        {
            stats.health -= damage;
        }
    }
}
