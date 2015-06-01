using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.Messages;
using System.Net;
using CommunicationServer.Communication;
using Communication.Network.TCP;
using Communication.MessageComponents;
using CommunicationServer.MessageCommunication;
using System.Threading;
using Cluster.Client;
using Cluster.Util.Client;

namespace CommunicationServer
{
    public class Server
    {
        public const int PRIMARY_PORT = 8090;

        public static bool primaryMode = false;

        static void Main(string[] args)
        {
            ModeTracker modeTracker = new ModeTracker();

            IPAddress address, masterAddress;
            int port, masterPort;

            // Set up configuration
            string inputLine = "";
            foreach (string arg in args)
                inputLine += arg + " ";

            InputParser inputParser = new InputParser(inputLine);
            inputParser.ParseInput();

            address = getIPAddress();
            port = inputParser.Port;

            // If address was given 
            if (inputParser.MasterAddress == null)
            {
                primaryMode = true;

                modeTracker.InitiatePrimary(address, port);
            }
            else 
            {
                primaryMode = false;
                masterAddress = inputParser.MasterAddress;
                masterPort = inputParser.MasterPort;

                modeTracker.InitiateBackup(address, port, masterAddress, masterPort);
            }
        }

        private static IPAddress getIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                    return IPAddress.Parse(localIP);
                }
            }
            return IPAddress.Parse(localIP);
        }

        private static void consoleInput()
        {

        }

    }
}
