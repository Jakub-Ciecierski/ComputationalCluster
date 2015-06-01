using Cluster.Util;
using Communication.MessageComponents;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cluster.Client.Messaging
{
    /// <summary>
    ///     Makes sure that keep alive messages are sent within the timout
    ///     and uses the MessageHandler to handle the requests.
    /// </summary>
    public class KeepAliveTimer
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private MessageProcessor messageProcessor;

        private ClientSystemTracker systemTracker;

        /// <summary>
        ///     Timer for sending messages
        /// </summary>
        private System.Timers.Timer timer;

        private bool isActive;

        /// <summary>
        ///     If turned to false, the Message processor is stoped
        /// </summary>
        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        /// <summary>
        ///     Creates KeepAliveTimer
        /// </summary>
        /// <param name="messageProcessor"></param>
        /// /// <param name="systemTracker"></param>
        public KeepAliveTimer(MessageProcessor messageProcessor, ClientSystemTracker systemTracker)
        {
            this.messageProcessor = messageProcessor;
            this.systemTracker = systemTracker;

            // TODO Magic numbers
            this.timer = new System.Timers.Timer((systemTracker.Node.Timeout * 1000) / 2);
            this.timer.Elapsed += keepAlive;

            Active = false;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void keepAlive(Object source, ElapsedEventArgs e)
        {
            SmartConsole.PrintLine("Sending Status message", SmartConsole.DebugLevel.Basic);

            try
            {
                messageProcessor.Communicate(systemTracker.Node.ToStatusMessage());
            }
            catch (SocketException excep)
            {
                SmartConsole.PrintLine("Lost connection with primary server, reconnecting to next backup...", SmartConsole.DebugLevel.Advanced);

                if (systemTracker.Node.BackupServers.Length == 0)
                {
                    SmartConsole.PrintLine("No other backup server avaiable", SmartConsole.DebugLevel.Advanced);
                    return;
                }
                BackupCommunicationServer bserver = systemTracker.Node.BackupServers[0];

                // connect to next backup server.
                messageProcessor.client.Address = IPAddress.Parse(bserver.address);
                messageProcessor.client.Port = bserver.port;
            }
        }


        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Starts the message processor
        /// </summary>
        public void Start()
        {
            timer.Enabled = true;
        }

        /// <summary>
        ///     Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Enabled = false;
        }
    }
}
