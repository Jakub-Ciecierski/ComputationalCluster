using Communication;
using Communication.Messages;
using Communication.Network;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
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

        public void PrintHelp()
        {
            Console.Write("Possible Messages: \n");
            Console.Write("\tstatus - Send status to server\n");
            Console.Write("\tderegister - Deregisters from the server\n");
            Console.Write("\tregister - Registers back to server\n");
            Console.Write("\tdisconnect - disconnects the connection \n");
            Console.Write("\tquit - Sends deregister and quits application \n");
        }
        public void SendStatusMessage()
        {
            Console.Write(" >> Sending status \n\n");
            client.Send(node.ToStatusMessage());
            Console.Write(" >> Waiting for response \n\n");
            Message message = client.Receive();
            Console.Write(" >> Message received: \n\n");
            Console.Write(message.ToString());
        }

        public void DeRegister()
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
          
            bool _continue = true;
            while (_continue)
            {
                string line = Console.ReadLine();
                switch (line)
                {
                    case "status":
                        if(isRegistered)
                            SendStatusMessage();
                        break;
                    case "deregister":
                        if (isRegistered)
                            DeRegister();
                        break;
                    case "register":
                        registerToServer();
                        break;
                    case "disconnect":
                        client.Disconnect();
                        break;
                    case "quit":
                        if (isRegistered)
                            DeRegister();
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
