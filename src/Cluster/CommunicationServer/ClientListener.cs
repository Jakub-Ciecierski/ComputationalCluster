using Communication;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections;
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
        List<Socket> sockets = new List<Socket>();

        public async void ListenForClients()
        {
            ClientTracker clientTracker = new ClientTracker();

            string host = "192.168.1.14";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            NetworkServer server = new NetworkServer(address, port);
            server.OpenConnection();

            while (true)
            {
                // should hang
                ArrayList socketsToRead = server.SelectForRead();

                foreach (Socket socket in socketsToRead)
                {
                    Message message = server.Receive(socket);
                    Console.Write("Message from: ");
                    Console.Write((socket.RemoteEndPoint as IPEndPoint).Address + ":" + (socket.RemoteEndPoint as IPEndPoint).Port + "\n\n");
                    Console.Write(message.ToString() + "\n\n");

                    Communication.MessageComponents.BackupCommunicationServer backupServer = new Communication.MessageComponents.BackupCommunicationServer("tcp", 5);

                    if (message.GetType() == typeof(RegisterMessage))
                    {
                        Console.Write(" >> Adding Node to List \n\n");
                        sockets.Add(socket);

                        IPAddress clientAddress = IPAddress.Parse("192.168.1.14");
                        await clientTracker.RegisterElement((RegisterMessage)message, clientAddress);
                        
                        ulong id = 1;
                        uint timeout = 100;
                        RegisterResponseMessage response = new RegisterResponseMessage(id, timeout, backupServer);

                        server.Send(socket, response);

                        Console.Write(" >> Sent a response \n\n");
                    }
                    else
                    {
                        NoOperationMessage response = new NoOperationMessage(backupServer);
                        server.Send(socket, response);
                        Console.Write(" >> Sent a NoOperation Message \n");
                    }
                }
            }
        }
    }
}
