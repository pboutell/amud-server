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
            StringBuilder buffer = new StringBuilder();

            if (player.room.hasExit(direction))
            {
                player.room.removePlayer(player);
                buffer.AppendFormat("\r\n{0} exits the room to the {1}.\r\n", 
                                    player.name, Direction.directionToName(direction));
                player.room.sendToRestRoom(buffer.ToString(), player);

                buffer.Clear();
                player.room.exits[direction].addPlayer(player);
                buffer.AppendFormat("\r\n{0} has entered the room from the {1}.\r\n", 
                                    player.name, Direction.directionToName(Direction.oppositeExit(direction)));
                player.room.sendToRestRoom(buffer.ToString(), player);
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public bool walk(int direction, NPC npc)
        {
            StringBuilder buffer = new StringBuilder();

            if (npc.room.hasExit(direction))
            {
                buffer.AppendFormat("\r\n{0} exits the room to the {1}.\r\n", 
                                    npc.description, Direction.directionToName(direction));
                npc.room.sendToRoom(buffer.ToString());
                npc.room.removeNPC(npc);
                npc.room.exits[direction].addNPC(npc);

                buffer.Clear();
                buffer.AppendFormat("\r\n{0} has entered the room from the {1}.\r\n",
                                    npc.description, 
                                    Direction.directionToName(Direction.oppositeExit(direction)));
                npc.room.sendToRoom(buffer.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
