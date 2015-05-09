using Cluster.Client;
using Communication;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Util.Client
{
    public class ConsoleManager
    {
        private NetworkClient client;
        private NetworkNode node;

        private bool isRegistered = false;

        public ConsoleManager(NetworkClient client, NetworkNode node)
        { 
            this.client = client;
            this.node = node;
        }

        private void printHelp()
        {
            Console.Write("Possible Messages: \n");
            Console.Write("\tstatus - Send status to server\n");
            Console.Write("\tderegister - Deregisters from the server\n");
            Console.Write("\tregister - Registers back to server\n");
            Console.Write("\tquit - Sends deregister and quits application, will crash \n");
        }
        private void sendStatusMessage()
        {
            Console.Write(" >> Sending status \n\n");
            client.Send(node.ToStatusMessage());
            Console.Write(" >> Waiting for response \n\n");
            Message message = client.Receive();
            Console.Write(" >> Message received: \n\n");
            Console.Write(message.ToString());
        }

        private void deRegister()
        {
            Console.Write(" >> Sending DeRegister Message \n\n");
            client.Send(node.ToDeregisterMessage());
            isRegistered = false;
        }

        private void registerToServer()
        {
            Console.Write(" >> Press enter to connect to server... \n\n");
            Console.ReadLine();

            Console.Write(" >> Client connecting to server... " + client.Address + ":" + client.Port + "\n\n");
            client.Connect();

            RegisterMessage registerMessage = node.ToRegisterMessage();

            Console.Write(" >> Sending Register message... \n\n");
            client.Send(registerMessage);

            Console.Write(" >> Waiting for response... \n\n");
            Message response = client.Receive();
            isRegistered = true;
            Console.Write(" >> The response is: \n\n" + response.ToString() + "\n\n");
        }

        public void StartConsole()
        {
            registerToServer();

            Console.Write(" >> Send Message \n");
            printHelp();
            bool _continue = true;
            while (_continue)
            {
                string line = Console.ReadLine();
                switch (line)
                {
                    case "status":
                        if(isRegistered)
                            sendStatusMessage();
                        break;
                    case "deregister":
                        if (isRegistered)
                            deRegister();
                        break;
                    case "register":
                        registerToServer();
                        break;
                    case "quit":
                        if (isRegistered)
                            deRegister();
                        _continue = false;
                        break;
                    default:
                        printHelp();
                        break;

                }
            }

            Console.Write(" >> Shutting down console... \n");
        }
    }
}
