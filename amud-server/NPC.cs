using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class NPC : Character
    {
        public NPC(string name, string description, CharacterStats stats)
        {
            this.name = name;
            this.description = description;
            this.stats = stats;
        }
    }
}
