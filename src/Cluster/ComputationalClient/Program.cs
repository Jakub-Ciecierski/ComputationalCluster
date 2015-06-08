using Cluster.Benchmarks;
using Cluster.Client;
using Cluster.Client.Messaging;
using Cluster.Util;
using Cluster.Util.Client;
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
    public class Program
    {
        /// <summary>
        ///     How often client asks for solution. in seconds
        /// </summary>
        static uint CLIENT_REQUEST_FREQUENCY = 4;
        public static bool doWork = true;
        static void Main(string[] args)
        {
            RegisterType type = RegisterType.ComputationalClient;
            byte parallelThreads = 5;
            string[] problems = { "DVRP" };

            SolveRequestMessage solveRequestMessage = new SolveRequestMessage();

            string inputLine = "";
            foreach (string arg in args)
                inputLine += arg + " ";

            InputParser inputParser = new InputParser(inputLine);
            inputParser.ParseInput();

            IPAddress address = inputParser.Address;
            int port = inputParser.Port;

            NetworkNode node = new NetworkNode(type, parallelThreads, problems) { Timeout = CLIENT_REQUEST_FREQUENCY };



            SmartConsole.PrintLine("ComputationalClient starting work", SmartConsole.DebugLevel.Advanced);

            NetworkClient client = new NetworkClient(address, port);

            for (; ; )
            {
                /*************** Register *****************/

                doWork = true;

                SmartConsole.PrintLine("Type in a file path", SmartConsole.DebugLevel.Advanced);
                String filePath = Console.ReadLine();
                solveRequestMessage = loadDataFromDisc(filePath);

                /******  setup logic modules *****************/
                SystemTracker systemTracker = new SystemTracker(node);
                MessageHandler messageHandler = new MessageHandler(systemTracker, client);
                MessageProcessor messageProcessor = new MessageProcessor(messageHandler, client, node);
                KeepAliveTimer keepAliveTimer = new KeepAliveTimer(messageProcessor, systemTracker);

                messageHandler.keepAliveTimer = keepAliveTimer;

                node.MessageProcessor = messageProcessor;

                /************ send solve request *****************/
                client.Connect();

                messageProcessor.Communicate(solveRequestMessage);

                while (doWork)
                {
                    Thread.Sleep(1000);
                }

                /*Object mutex = new Object();

                lock (mutex)
                {
                    Monitor.Wait(mutex);
                }*/
            }
        }

        public static SolveRequestMessage loadDataFromDisc(String filePath)
        {
            SolveRequestMessage solveRequestMessage;
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            VRPParser benchmark = new VRPParser(text);

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

            data = DataSerialization.ObjectToByteArray(benchmark);
        //    data = GetBytes(filePath);
            solveRequestMessage = new SolveRequestMessage(problemType, data);
            Console.WriteLine(" >> Success");
            return solveRequestMessage;
        }

        public static VRPParser getBenchmark(String filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            VRPParser benchmark = new VRPParser(text);
            return benchmark;
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
