using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doLook(string[] args, Player player)
        {
            player.sendToPlayer(player.room.name);
            player.sendToPlayer(player.room.description + "\r\n");
            player.sendToPlayer(player.room.exitsToString());

            foreach (Player p in player.players)
            {
                if (p != player)
                {
                    player.sendToPlayer(p.name + " is standing here.");
                }
            }
        }

        private void doQuit(string[] args, Player player)
        {
            player.sendToRest(player.name + " has left the game!");
            player.sendToPlayer("thank you, come again!");
            player.disconnect();
        }
    }
}
