using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Movement
    {
        public bool walk(int direction, Player player)
        {
            if (player.room.hasExit(direction))
            {
                player.room.removePlayer(player);
                player.room.exits[direction].addPlayer(player);
                return true;
                
            }
            else
            {
                return false;
            }
        }
       
        public bool walk(int direction, NPC npc)
        {
            if (npc.room.hasExit(direction))
            {
                npc.room.removeNPC(npc);
                npc.room.exits[direction].addNPC(npc);
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}
