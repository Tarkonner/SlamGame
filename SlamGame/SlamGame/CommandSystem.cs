using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlamGame
{
    public class CommandSystem
    {
        public CommandSystem()
        {
            Console.WriteLine("Input running");
        }

        public async Task RunAsync()
        {
            await Task.Run(() =>
            {
                while (Program.gameLoopRunning)
                {
                    var input = Console.ReadLine();

                    if (input == null)
                        continue;

                    CommandList(input);
                }

                Console.WriteLine("Command input loop stopped.");
            });
        }

        public void CommandList(string input)
        {
            switch (input)
            {
                case "Close":
                    Program.gameLoopRunning = false;
                    break;
                default:
                    Console.WriteLine("Command not found.");
                    break;

            }
        }
    }
}
