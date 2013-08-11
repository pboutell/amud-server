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

            player.client.send("\r\nyou say \"" + text.ToString().Trim() + ".\"\r\n");

            foreach (Player p in player.room.players)
            {
                if (p != player)
                    p.client.send("\r\n\n" + player.name + " says \"" + text.ToString().Trim() + ".\"\r\n");
            }
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

            player.client.sendToAll("[chat " + player.name + ":] \"" + text.ToString().Trim() + ".\"\r\n");
        }
    }
}
