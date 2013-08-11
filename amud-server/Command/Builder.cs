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
            int direction = Direction.directionNumber(args[1]);

            if (args.Length == 1)
            {
                player.sendToPlayer("Dig where?\r\n");
                return;
            }

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

        public bool digRoom(int dirTo, Room from)
        {
            if (from.exits.ElementAtOrDefault(dirTo) != null)
            {
                return false;
            }
            else
            {
                Room newRoom = new Room("New Room", "This room has not been finished yet.");
                from.exits[dirTo] = newRoom;
                newRoom.exits[Direction.oppositeExit(dirTo)] = from;
                World.rooms.Add(newRoom);
                return true;
            }
        }

    }
}
