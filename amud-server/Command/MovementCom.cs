using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doWalk(string[] args, Player player)
        {
            Movement movement = new Movement();

            if (movement.walk(Direction.shortDirectionToInt(args[0].TrimEnd('\r', '\n')), player))
            {
                player.parser.parse("look");
            }
            else
            {
                player.client.send("You can't go that way!");
            }
        }
    }
}
