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
            string sanitized = "";

            foreach (char c in message)
            {
                if (c != '\r' || c != '\n')
                {
                    sanitized += c;
                }
            }

            string []args = sanitized.Split(' ');

            Command command;

            commands.commandDict.TryGetValue(args[0].TrimEnd('\n', '\r'), out command);

            if (command != null)
            {
                command.method.Invoke(args, player);
            }
        }

    }
}
