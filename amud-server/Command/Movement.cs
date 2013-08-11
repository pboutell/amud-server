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
                player.room.removePlayer(player);
                player.room.exits[Direction.North].addPlayer(player);
                player.parser.parse("look");
            }
            else
            {
                player.client.send("You can't go that way!\r\n");
            }
        }

        private void doEast(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.East))
            {
                player.room.removePlayer(player);
                player.room.exits[Direction.East].addPlayer(player);
                player.parser.parse("look");
            }
            else
            {
                player.client.send("You can't go that way!\r\n");
            }
        }

        private void doSouth(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.South))
            {
                player.room.removePlayer(player);
                player.room.exits[Direction.South].addPlayer(player);
                player.parser.parse("look");
            }
            else
            {
                player.client.send("You can't go that way!\r\n");
            }
        }

        private void doWest(string[] args, Player player)
        {
            if (player.room.hasExit(Direction.West))
            {
                player.room.removePlayer(player);
                player.room.exits[Direction.West].addPlayer(player);
                player.parser.parse("look");
            }
            else
            {
                player.client.send("You can't go that way!\r\n");
            }
        }
    }
}
