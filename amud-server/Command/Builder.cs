using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doDig(string[] args, Player player)
        {
            if (args.Length == 1)
            {
                player.sendToPlayer("Dig where?\r\n");
                return;
            }

            int direction = Direction.directionToNumber(args[1]);

            if (direction >= 0 && direction < 4)
            {
                if (!digRoom(direction, player.room))
                {
                    player.sendToPlayer("Can't dig that way!\r\n");
                }
                else
                {
                    player.sendToPlayer("Dug " + args[1] + "\r\n");
                }
            }
            else
            {
                player.sendToPlayer("Don't know how to dig that direction!\r\n");
            }
        }

        public bool digRoom(int direction, Room from)
        {
            if (from.hasExit(direction))
            {
                return false;
            }
            else
            {
                Room newRoom = new Room("New Room", "This room has not been finished yet.");
                from.exits[direction] = newRoom;
                newRoom.exits[Direction.oppositeExit(direction)] = from;
                World.rooms.Add(newRoom);
                return true;
            }
        }

    }
}
