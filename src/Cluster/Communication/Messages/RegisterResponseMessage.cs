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
    /// Register Response message is sent as a response to the 
    /// Register message giving back the component its
    /// unique ID and informing how often it should sent the Status message.
    /// </summary>
    public partial class RegisterResponseMessage
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "RegisterResponse";

        private RegisterResponseMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     The ID assigned by the Communication Server
        /// </param>
        /// <param name="timeout">
        ///     The communication timeout configured on Communication Server
        /// </param>
        /// <param name="backupServer">
        ///     Backup server
        /// </param>
        public RegisterResponseMessage(ulong id, uint timeout, BackupCommunicationServer backupServer)
        {
            Id = id;
            Timeout = timeout;

            BackupCommunicationServer[] backupServers = {backupServer};

            BackupCommunicationServers = backupServers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     The ID assigned by the Communication Server
        /// </param>
        /// <param name="timeout">
        ///     The communication timeout configured on Communication Server
        /// </param>
        /// <param name="backupServer">
        ///     The list of Backup servers
        /// </param>
        public RegisterResponseMessage(ulong id, uint timeout, BackupCommunicationServer[] backupServers)
        {
            Id = id;
            Timeout = timeout;

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
        public static RegisterResponseMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RegisterResponseMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (RegisterResponseMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            RegisterResponseMessage message = obj as RegisterResponseMessage;

            return (Id == message.Id && Enumerable.SequenceEqual(BackupCommunicationServers, message.BackupCommunicationServers));
        }
    }
}
