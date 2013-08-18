using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    partial class Commands
    {
        private void doSay(string[] args, Player player)
        {
            StringBuilder text = new StringBuilder();

            if (args.Length == 0)
            {
                player.client.send("say what?\r\n");
            }

            foreach (string s in args.Skip(1))
            {
                text.Append(s);
                text.Append(" ");
            }

            player.client.send("%Byou say \"%W" + text.ToString().Trim() + ".%B\"%x\r\n");
            player.room.sendToRestRoom("\n%B" + player.name + " says \"%W" + text.ToString().Trim() + ".%B\"%x\r\n", player);
        }

        private void doChat(string[] args, Player player)
        {
            StringBuilder text = new StringBuilder();

            if (args.Length == 0)
            {
                player.client.send("chat what?\r\n");
            }

            foreach (string s in args.Skip(1))
            {
                text.Append(s);
                text.Append(" ");
            }

            player.client.sendToAll("%W[%Cchat %y" + player.name + "%w:%W]%x \"" + text.ToString().Trim() + "%x.\"\r\n");
        }
    }
}
