using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageComponents;
using System.Xml.Serialization;
using System.IO;

namespace Communication.Messages
{
    /// <summary>
    /// No Operation message is sent by the CS in response to Status messages.
    /// It is used to inform the components about the current list of backup servers.
    /// It could be send in conjunction with other messages.
    /// </summary>
    public partial class NoOperationMessage
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "NoOperation";

        private NoOperationMessage() { }

        public NoOperationMessage(BackupCommunicationServer backupServer)
        {
            BackupCommunicationServer[] backupServers =
            {
                backupServer
            };

            BackupCommunicationServers = backupServers;
        }

        public NoOperationMessage(BackupCommunicationServer[] backupServers)
        {
            BackupCommunicationServers = backupServers;
        }

        /// <summary>
        ///     Construct an object message from input xml
        /// </summary>
        /// <param name="xmlString">
        ///     xml for which the object should be created
        /// </param>
        /// <returns>
        ///     Deserialized object
        /// </returns>
        public static NoOperationMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NoOperationMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (NoOperationMessage)serializer.Deserialize(strReader);
        }
        public override bool Equals(object obj)
        {
            NoOperationMessage message = obj as NoOperationMessage;

            return (Enumerable.SequenceEqual(BackupCommunicationServers, message.BackupCommunicationServers));
        }
    }
}
