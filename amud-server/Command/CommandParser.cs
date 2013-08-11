using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace amud_server
{
    class CommandParser
    {
        Player player;
        Commands commands = new Commands();

        public CommandParser(Player playerInput)
        {
            player = playerInput;
        }

        public void parse(string message)
        {
            Command toInvoke = null;
            string []args = message.Split(' ');
            args[0] = args[0].TrimEnd('\r', '\n');

            foreach (Command command in commands.all)
            {
                if (command.name.StartsWith(args[0]) || command.name.Equals(args[0]))
                {
                    toInvoke = command;
                    break;
                }
            }

            if (toInvoke != null)
                toInvoke.method.Invoke(args, player);
            else
                player.sendToPlayer("Don't know how to do " + args[0] + "\r\n");
        }

    }
}
