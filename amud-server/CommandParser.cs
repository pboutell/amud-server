using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string []args = message.Split(' ');
            Command command;

            commands.commandDict.TryGetValue(args[0].TrimEnd('\r', '\0'), out command);

            if (command != null)
            {
                command.method.Invoke(args, player);
            }
        }
    }
}
