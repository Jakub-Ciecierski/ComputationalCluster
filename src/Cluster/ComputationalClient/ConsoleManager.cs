using Communication;
using Communication.Messages;
using Communication.Network;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalClient
{
    public class ConsoleManager
    {
        private NetworkClient client;

        public ConsoleManager(NetworkClient client)
        {
            this.client = client;
        }

        public void PrintHelp()
        {
            Console.Write(" >> Possible Messages: \n");
            Console.Write(StatusMessage.ELEMENT_NAME + "\n");
            Console.Write("De" + RegisterMessage.ELEMENT_NAME + "\n");
            Console.Write("quit \n");
        }

        public void StartConsole()
        {
            Console.Write(" >> Send Message \n");

            bool _continue = true;
            while (_continue)
            {
                string line = Console.ReadLine();
                switch (line)
                {
                    case "Status":
                        break;
                    case "quit":
                        _continue = false;
                        break;
                    default:
                        PrintHelp();
                        break;

                }
            }

            Console.Write(" >> Shutting down console... \n");
        }
    }
}
