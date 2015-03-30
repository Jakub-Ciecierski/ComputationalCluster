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
using System.Collections;

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

            // Create message to send
            RegisterMessage expectedMessage = TestHelper.CreateRegisterMessage();
            string messageStr = expectedMessage.ToXmlString();

            // Create server
            IPAddress address = IPAddress.Parse(localhost);
            int port = 5555;

            NetworkServer server = new NetworkServer(address, port);
            server.Open();
            server.StartListeningForClients();
            
            // connect client
            NetworkClient client = new NetworkClient(address, port);
            client.Connect();

            client.Send(expectedMessage);

            ArrayList sockets = new ArrayList();

            while ((sockets = server.SelectForRead()).Count == 0) ;

            Socket socket = (Socket)sockets[0];
            Message actualMessage = server.Receive(socket);

            server.Close();
            client.Disconnect();
            
            Assert.AreEqual(expectedMessage, actualMessage);

        }
    }
}
