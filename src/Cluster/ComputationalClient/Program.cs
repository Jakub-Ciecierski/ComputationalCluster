using Communication;
using Communication.Messages;
using Communication.Network.Client;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SolveRequestMessage solveRequestMessage;
            string problemType="";
            byte[] data = new byte[1];
            ulong solvingTimeout = 0;
            ulong id=0;
            /************ Setup connection ************/
            string host = "192.168.1.11";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            Console.Write(" >> I'm a  CompuationalClient \n");
            NetworkClient client = new NetworkClient(address, port);

            /*************** Register *****************/
            Console.Write(" >>Type in a file path:\n");
            String filePath = Console.ReadLine();
            loadDataFromDisc(filePath,problemType,data,solvingTimeout,id);

        //      ConsoleManager consoleManager = new ConsoleManager(client);
        //    consoleManager.StartConsole();

        }

        private static void loadDataFromDisc(String filePath, string problemType, byte[] data, ulong solvingTimeout, ulong id)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            if (Path.GetExtension(filePath) == "vrp")
            {
                problemType = "DVRP";
            }
            else
            {
                Console.WriteLine(">> Unsupported problem type. Please load a problem with one of the following problem types: \n *DVRP");
            }
            
            data = Convert.ToByte(


            
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
