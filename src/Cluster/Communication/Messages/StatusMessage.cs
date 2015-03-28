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
    /// Status message is sent by TM, CN and Backup CS to CS at
    /// least as frequent as a timeout given in Register
    /// Response. In the Status message the component reports the state of its threads, what they are
    /// computing (unique problem instance id, task id within the 
    /// given problem instance, the type of problem instance) and for how long.
    /// </summary>
    public partial class StatusMessage
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME= "Status";

        private StatusMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     The ID of node (the one assigned by server)
        /// </param>
        /// <param name="statusThreads">
        ///     List of statuses for different threads
        /// </param>
        public StatusMessage(ulong id, StatusThread[] statusThreads) 
        {
            Id = id;
            Threads = statusThreads;
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
        public static StatusMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StatusMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (StatusMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            StatusMessage message = obj as StatusMessage;
            if (Id == message.Id && Enumerable.SequenceEqual(Threads, message.Threads))
                return true;
            return false;
        }
    }
}
