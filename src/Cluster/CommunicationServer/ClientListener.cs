using Communication;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer
{
    public class ClientListener
    {
        public async void ListenForClients()
        {
            ClientTracker clientTracker = new ClientTracker();

            string host = "192.168.1.14";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            NetworkListener listener = new NetworkListener(address, port);
            listener.OpenConnection();

            while (true)
            {
                while (!listener.IsPending())
                {
                    Console.Write(">> No client wanting to join... \n");
                    Thread.Sleep(3000);
                }

                Console.Write(">> Accepting connected Client... \n");
                Socket socket = listener.GetAcceptedSocket();
                
                Console.Write(">> " + socket.AddressFamily + "\n");
                listener.Send(socket, "Response message: Gay");

                Message message = listener.Receive(socket);
                if (message.GetType() == typeof(RegisterMessage))
                {
                    Console.Write(">> " + message.ToString() + "\n");

                    Console.Write(">> Adding Node to List \n");
                    IPAddress clientAddress = IPAddress.Parse("192.168.1.14");

                    //await clientTracker.RegisterElement(message, clientAddress);

                    listener.Send(socket, "Response message: Gay");

                    Thread.Sleep(5000);
                }
            }
        }
    }
}
