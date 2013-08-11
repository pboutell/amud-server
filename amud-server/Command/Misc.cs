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
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine();
            buffer.AppendLine(player.room.name);
            buffer.AppendLine(player.room.description);
            buffer.AppendLine();

            buffer.Append(player.room.listOtherPlayers(player));
            buffer.Append(player.room.listNPCs());

            buffer.AppendLine();
            buffer.AppendLine(player.room.exitsToString() );

            player.client.send(buffer.ToString());
        }

        private void doQuit(string[] args, Player player)
        {
            player.client.playing = false;
            player.client.sendToRest(player.name + " has left the game!\r\n");
            player.client.send("thank you, come again!");
            player.client.disconnect();
        }
    }
}
