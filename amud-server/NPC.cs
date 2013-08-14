using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class NPC : Character
    {
        

        public NPC(string name, string description, CharacterStats stats, Room room)
        {
            this.name = name;
            this.description = description;
            this.stats = stats;

            World.mobs.Add(this);
        }

        public void update()
        {
            if (combat.target != null && combat.isFighting == true && combat.target.room == room)
            {
                combat.attack(combat.target);
            }

            if (stats.health <= 0)
            {
                die();
            }
        }
        public void die()
        {
            StringBuilder buffer = new StringBuilder();

            room.removeNPC(this);
            buffer.AppendFormat("%rHere lies the corpse of %W{0}", description);
            room.addItem(new Item("corpse", buffer.ToString(), 20, "none"));
            

            combat.target = null;
            combat.isFighting = false;

            //room = null;
        }
    }
}
