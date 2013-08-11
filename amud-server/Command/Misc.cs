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
            player.client.send(player.room.name);
            player.client.send(player.room.description + "\r\n");
            player.client.send(player.room.exitsToString());

            foreach (Client c in player.client.clients)
            {
                if (c != player.client)
                {
                    c.send(c.player.name + " is standing here.");
                }
            }
        }

        private void doQuit(string[] args, Player player)
        {
            player.client.sendToRest(player.name + " has left the game!");
            player.client.send("thank you, come again!");
            player.client.disconnect();
        }
    }
}
