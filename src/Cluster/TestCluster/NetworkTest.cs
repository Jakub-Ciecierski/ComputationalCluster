using Communication;
using Communication.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestCluster
{

    [TestClass]
    public class NetworkTest
    {
        [TestMethod]
        public void SocketCommunication_ServerSends_ClientReceives()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string localhost = "";
            foreach(IPAddress ip in host.AddressList)
                if(ip.AddressFamily.ToString() == "InterNetwork")
                    localhost = ip.ToString();

            // Create server
            IPAddress address = IPAddress.Parse(localhost);
            int port = 5555;

            NetworkServer server = new NetworkServer(address, port);
            server.OpenConnection();
            // connect client
            NetworkClient client = new NetworkClient(address, port);
            client.Connect();

            // Create message to send
            RegisterMessage expectedMessage = TestHelper.CreateRegisterMessage();
            string messageStr = expectedMessage.ToXmlString();
            
            // init sockets

            server.Send(client.Socket, expectedMessage);

            Message actualMessage = client.Receive();

            server.Close();
            client.Disconnect();
            
            Assert.AreEqual(expectedMessage, actualMessage);

        }
    }
}
