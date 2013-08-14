using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doKill(string[] args, Player player) 
        {
            if (args.Length == 1)
            {
                player.client.send("Kill whom?");
                return;
            }

            NPC target = player.room.getNPCByName(args[1]);

            if (target != null)
            {
            }
            else
            {
                player.client.send("I do not see anybody by that name here.");
            }
        }
    }
}
