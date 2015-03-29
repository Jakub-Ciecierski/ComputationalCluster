using Communication;
using Communication.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network;
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

            NetworkListener server = new NetworkListener(address, port);
            server.OpenConnection();
            // connect client
            NetworkClient client = new NetworkClient(address, port);
            client.Connect();

            // Create message to send
            RegisterMessage expectedMessage = TestHelper.CreateRegisterMessage();
            string messageStr = expectedMessage.ToXmlString();
            
            // init sockets
            Socket socket = server.GetAcceptedSocket();
            client.StartSocket();
            server.Send(socket, messageStr);

            string xmlStr = client.Receive();

            RegisterMessage actualMessage = null;
            if (Message.GetName(xmlStr) == RegisterMessage.ELEMENT_NAME)
                actualMessage = RegisterMessage.Construct(xmlStr);

            server.Close();
            client.Disconnect();
            
            Assert.AreEqual(expectedMessage, actualMessage);

        }
    }
}
