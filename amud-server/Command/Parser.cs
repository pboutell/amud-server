using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace amud_server
{
    public class Parser
    {
        private Player player;
        private Commands commands = new Commands();

        public Parser(Player player)
        {
            this.player = player;
        }

        public void parse(string message)
        {
            Commands commands = new Commands();
            string []args = message.Split(' ');
            args[0] = args[0].TrimEnd('\r', '\n');

            Command command = commands.lookupCommand(args[0]);

            if (command != null)
                command.dispatch(args, player);
            else
                player.client.send("Don't know how to do " + args[0] + "\r\n");
        }
    }
}
