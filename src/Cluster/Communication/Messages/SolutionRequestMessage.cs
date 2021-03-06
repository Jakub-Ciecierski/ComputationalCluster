﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Communication.Messages
{
    /// <summary>
    /// Solution Request message is sent from the CC in order to check whether 
    /// the cluster has successfully computed the solution.
    /// It allows CC to be shut down and disconnected 
    /// from server during computations.
    /// </summary>
    public partial class SolutionRequestMessage
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "SolutionRequest";

        private SolutionRequestMessage() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        ///     the ID of the problem instance assigned by the server
        /// </param>
        public  SolutionRequestMessage(ulong id) 
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
        public static SolutionRequestMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SolutionRequestMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (SolutionRequestMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            SolutionRequestMessage message = obj as SolutionRequestMessage;


            return (Id == message.Id);
        }
    }
}
