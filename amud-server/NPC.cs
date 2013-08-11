using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class NPC : Character
    {
        public string name { get; private set; }
        public string decription { get; private set; }

        public NPC(string name, string description, CharacterStats stats)
        {
            this.name = name;
            this.decription = decription;
            this.stats = stats;
        }
    }
}
