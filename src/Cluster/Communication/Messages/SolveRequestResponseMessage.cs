using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Communication.Messages
{
    /// <summary>
    /// Solve Request Response message is sent by CS to CC 
    /// as an answer for the Solve Request. It provides CC
    /// with unique identifier of the problem instance.
    /// </summary>
    public partial class SolveRequestResponseMessage : Message
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "SolveRequestResponse";

        private SolveRequestResponseMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     The ID of the problem instance assigned by the server
        /// </param>
        public SolveRequestResponseMessage(ulong id) 
        {
            Id = id;
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
        public static SolveRequestResponseMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SolveRequestResponseMessage));;
            StringReader strReader = new StringReader(xmlString);

            return (SolveRequestResponseMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            SolveRequestResponseMessage message = obj as SolveRequestResponseMessage;
            return Id == message.Id;
        }
    }
}
