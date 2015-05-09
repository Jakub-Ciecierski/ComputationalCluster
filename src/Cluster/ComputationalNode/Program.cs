using Communication.Network;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;
using Cluster.Util.Client;
using ComputationalNode.MessageCommunication;
using Cluster.Client.Messaging;
using Cluster.Client;

namespace ComputationalNode
{
    class Program
    {
        static void Main(string[] args)
        {
            
            /************ Create node object ************/
            RegisterType type = RegisterType.ComputationalNode;
            byte parallelThreads = 5;
            string[] problems = { "DVRP" };

            //NetworkNode node = new NetworkNode(type, parallelThreads, problems);
            NetworkNode node = new NetworkNode();

            /************ Setup connection ************/
            string inputLine = "";
            foreach(string arg in args)
                inputLine += arg + " ";

            InputParser inputParser = new InputParser(inputLine);
            inputParser.ParseInput();

            IPAddress address = inputParser.Address;
            int port = inputParser.Port;

            Console.Write("I'm a " + node.Type + "\n\n");
            NetworkClient client = new NetworkClient(address, port);

            /************ Setup Logic modules ************/

            // system tracker
            SystemTracker systemTracker = new SystemTracker(node);

            MessageHandler messageHandler = new MessageHandler(systemTracker, client);

            MessageProcessor messageProcessor = new MessageProcessor(messageHandler, client);


            /************ Init all threads ************/
            // TODO

            /************ Register ************/
            client.Connect();
            Console.Write(" >> Sending Register message... \n\n");
            messageProcessor.Communicate(node.ToRegisterMessage());

            KeepAliveTimer keepAliveTimer = new KeepAliveTimer(messageProcessor, systemTracker);
            /************ Start Logic modules ************/
            keepAliveTimer.Start();

            Object mutex = new Object();
            //for (; ; ) ;
            // TODO Thread pool waiting
            
            lock (mutex)
            {
                Monitor.Wait(mutex);
            }
            
        }
    }
}
