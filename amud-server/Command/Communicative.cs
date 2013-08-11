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
                player.sendToPlayer("say what?\r\n");
            }

            foreach (string s in args.Skip(1))
            {
                text.Append(s);
                text.Append(" ");
            }

            player.sendToPlayer("you say \"" + text.ToString().Trim() + ".\"\r\n");
            player.sendToRest(player.Name + " says \"" + text.ToString().Trim() + ".\"\r\n");
        }
    }
}
