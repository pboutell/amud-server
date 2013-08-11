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
                player.client.send("Dig where?\r\n");
                return;
            }

            int direction = Direction.directionToNumber(args[1]);

            if (direction >= 0 && direction < 4)
            {
                if (!player.room.newExit(direction))
                {
                    player.client.send("Can't dig that way!\r\n");
                }
                else
                {
                    player.client.send("Dug " + args[1] + "\r\n");
                }
            }
            else
            {
                player.client.send("I don't know how to dig that direction!\r\n");
            }
        }
    }
}
