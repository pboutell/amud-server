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
            player.bufferToSend(player.room.name);
            player.bufferToSend(player.room.description + "\r\n");

            foreach (Player p in player.room.players)
            {
                if (p != player)
                {
                    player.bufferToSend(p.name + " is standing here.");
                }
            }

            player.bufferToSend("");
            player.bufferToSend(player.room.exitsToString() );
            player.sendBuffer();
            
        }

        private void doQuit(string[] args, Player player)
        {
            player.client.playing = false;
            player.client.sendToRest(player.name + " has left the game!");
            player.client.send("thank you, come again!");
            player.client.disconnect();
        }
    }
}
