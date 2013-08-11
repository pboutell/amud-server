﻿using System;
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

        public CommandParser(Player player)
        {
            this.player = player;
        }

        public void parse(string message)
        {
            string []args = message.Split(' ');
            args[0] = args[0].TrimEnd('\r', '\n');
            
            Command command = lookupCommand(args[0]);

            if (command != null)
                command.method.Invoke(args, player);
            else
                player.client.send("Don't know how to do " + args[0] + "\r\n");
        }

        private Command lookupCommand(string search)
        {
            foreach (Command command in commands.all)
            {
                if (command.name.StartsWith(search) || command.name.Equals(search))
                {
                    return command;
                }
            }

            return null;
        }
    }
}
