using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace amud_server
{
    class NPC : Character
    {
        
        private Logger logger = new Logger();

        public NPC(string name, string description, CharacterStats stats)
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
            NPC dead = this;

            buffer.AppendFormat("\r\n{0}, has been struck down by {1}\r\n", description, combat.target.name);
            room.sendToRoom(buffer.ToString());

            buffer.Clear();
            buffer.AppendFormat("%rthe corpse of %W{0}", name);
            Item item = new Item("corpse", buffer.ToString(), 20, "none");
            room.addItem(item);

            combat.target = null;
            combat.isFighting = false;
            
            room.removeNPC(this);
            dead.room = null;

            while (!World.mobs.TryTake(out dead)) ;
        }
    }
}
