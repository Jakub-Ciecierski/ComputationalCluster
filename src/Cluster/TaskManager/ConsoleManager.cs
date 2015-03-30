using Communication;
using Communication.Messages;
using Communication.Network;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class ConsoleManager
    {
        private NetworkClient client;
        private NetworkNode node;

        public ConsoleManager(NetworkClient client, NetworkNode node)
        {
            this.client = client;
            this.node = node;
        }

        public void PrintHelp()
        {
            Console.Write(" >> Possible Messages: \n");
            Console.Write(StatusMessage.ELEMENT_NAME + "\n");
            Console.Write("De" + RegisterMessage.ELEMENT_NAME + "\n");
            Console.Write("quit \n");
        }
        public void SendStatusMessage()
        {
            Console.Write(" >> Sending status \n");
            client.Send(node.ToStatusMessage());
            Console.Write(" >> Waiting for response \n");
            Message message = client.Receive();
            Console.Write(" >> Message received: \n");
            Console.Write("\t" + message.ToString());
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
                        SendStatusMessage();
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
