using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doNorth(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.North))
            {
                player.room = player.room.exits[Direction.North];
                player.parser.parse("look");
            }
            else
            {
                player.sendToPlayer("You can't go that way!\r\n");
            }
        }

        private void doEast(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.East))
            {
                player.room = player.room.exits[Direction.East];
                player.parser.parse("look");
            }
            else
            {
                player.sendToPlayer("You can't go that way!\r\n");
            }
        }

        private void doSouth(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.South))
            {
                player.room = player.room.exits[Direction.South];
                player.parser.parse("look");
            }
            else
            {
                player.sendToPlayer("You can't go that way!\r\n");
            }
        }

        private void doWest(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.West))
            {
                player.room = player.room.exits[Direction.West];
                player.parser.parse("look");
            }
            else
            {
                player.sendToPlayer("You can't go that way!\r\n");
            }
        }
    }
}
