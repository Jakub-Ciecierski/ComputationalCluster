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
    /// Divide Problem is sent to TM to start the action of dividing the problem 
    /// instance to smaller tasks. TM is provided with information about 
    /// the computational power of the cluster in terms of total number
    /// of available threads. The same message is used to relay information
    /// for synchronizing info with Backup CS.
    /// </summary>
    public partial class DivideProblemMessage : Message
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "DivideProblem";

        private DivideProblemMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     the problem type name as given by TaskSolver and Client
        /// </param>
        /// <param name="problemId">
        ///     the ID of the problem instance assigned by the server
        /// </param>
        /// <param name="data">
        ///     the problem data
        /// </param>
        /// <param name="nodesCount">
        ///     the total number of currently available threads
        /// </param>
        /// <param name="nodeId">
        ///     the ID of the TM that is dividing the problem
        /// </param>
        public DivideProblemMessage(string problemType, ulong problemId, byte[] data, ulong nodesCount, ulong nodeId) 
        {
            ProblemType = problemType;
            Id = problemId;
            Data = data;
            ComputationalNodes = nodesCount;
            NodeID = nodeId;
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
        public static DivideProblemMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DivideProblemMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (DivideProblemMessage)serializer.Deserialize(strReader);
        }
        public override bool Equals(object obj)
        {
            DivideProblemMessage message = obj as DivideProblemMessage;

            return (Id == message.Id && ProblemType == message.ProblemType &&
                             Enumerable.SequenceEqual(Data, message.Data) &&
                             ComputationalNodes == message.ComputationalNodes &&
                             NodeID == message.NodeID);
        }
    }
}
