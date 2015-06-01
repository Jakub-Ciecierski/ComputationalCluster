using Cluster.Client;
using Cluster.Client.Messaging;
using Communication;
using Communication.Messages;
using Communication.Network.TCP;
using ComputationalClient.MessageCommunication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterType type = RegisterType.ComputationalClient;
            byte parallelThreads = 5;
            string[] problems = { "DVRP" };
            SolveRequestMessage solveRequestMessage = new SolveRequestMessage(); ;


            NetworkNode node = new NetworkNode(type, parallelThreads, problems) { Timeout = 4 };
            //NetworkNode node = new NetworkNode();
            /************ Setup connection ************/
            //string host = "169.254.80.80";
            string host = "169.254.80.80";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            Console.Write(" >> I'm a  ComputationalClient \n");
            NetworkClient client = new NetworkClient(address, port);

            /*************** Register *****************/
            Console.Write(" >>Type in a file path:\n");
            String filePath = Console.ReadLine();
            solveRequestMessage = loadDataFromDisc(filePath);

            /******  setup logic modules *****************/
            SystemTracker systemTracker = new SystemTracker(node);

            MessageHandler messageHandler = new MessageHandler(systemTracker, client);
            MessageProcessor messageProcessor = new MessageProcessor(messageHandler, client, node);

            node.MessageProcessor = messageProcessor;

            /************ send solve request *****************/
            client.Connect();
            Console.Write(" >> Sending Solve Request message... \n\n");

            ClientCompuatationsCheckTimer clientComputationsCheckTimer = new ClientCompuatationsCheckTimer(messageProcessor, systemTracker, solveRequestMessage.Id);
            messageHandler.clientComputationsCheckTimer = clientComputationsCheckTimer;

            messageProcessor.Communicate(solveRequestMessage);

           //KeepAliveTimer keepAliveTimer = new KeepAliveTimer(messageProcessor, systemTracker);
            /************ Start Logic modules ************/
           // clientComputationsCheckTimer.Start();

            Object mutex = new Object();
            // TODO Thread pool waiting

            lock (mutex)
            {
                Monitor.Wait(mutex);
            }
            //ConsoleManager consoleManager = new ConsoleManager(client);
            //consoleManager.StartConsole();

        }

        private static SolveRequestMessage loadDataFromDisc(String filePath)
        {
            SolveRequestMessage solveRequestMessage;
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            string problemType="";
            byte[] data;
            streamReader.Close();

            String extension = Path.GetExtension(filePath);

            if (extension == ".vrp")
            {
                problemType = "DVRP";
            }
            else
            {
                Console.WriteLine(">> Unsupported problem type. Please load a problem with one of the following problem types: \n *DVRP");
                return null;
            }

            data = GetBytes(filePath);
            solveRequestMessage = new SolveRequestMessage(problemType, data);
            Console.WriteLine(">>success");
            return solveRequestMessage;
        }

        private static void registerToServer(NetworkClient client, NetworkNode node)
        {
            Console.Write(" >> Press enter to connect to server... \n");
            Console.ReadLine();

            Console.Write(" >> Client connecting to server... " + client.Address + ":" + client.Port + "\n");
            Console.Write(" >> \n\n");
            client.Connect();

            RegisterMessage registerMessage = node.ToRegisterMessage();

            Console.Write(" >> Sending Register message... \n\n");
            client.Send(registerMessage);

            Console.Write(" >> Waiting for response... \n\n");
            Message response = client.Receive();

            Console.Write(" >> The response is: \n\n" + response.ToString() + "\n\n");
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
