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
    /// Solve Request message is sent by the CC to CS. 
    /// It gives the type of the problem instance to be solved,
    /// optionally the max time that could be used for computations 
    /// and the problem data in base64. The same
    /// message is used to relay information for synchronizing info with Backup CS.
    /// </summary>
    public partial class SolveRequestMessage  : Message
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "SolveRequest";

        private SolveRequestMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     The name of the type as given by TaskSolver
        /// </param>
        /// <param name="data">
        ///     The serialized problem data
        /// </param>
        /// <param name="solvingTimeout">
        ///     The optional time restriction for solving the problem (in ms)
        /// </param>
        /// <param name="id">
        ///     The ID of the problem instance assigned by the server
        /// </param>
        public SolveRequestMessage(string problemType, byte[] data,
                            ulong solvingTimeout, ulong id) 
        {
            ProblemType = problemType;
            Data = data;

            SolvingTimeout = solvingTimeout;
            SolvingTimeoutSpecified = true;

            Id = id;
            IdSpecified = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     The name of the type as given by TaskSolver
        /// </param>
        /// <param name="data">
        ///     The serialized problem data
        /// </param>
        /// <param name="solvingTimeout">
        ///     The optional time restriction for solving the problem (in ms)
        /// </param>
        public SolveRequestMessage(string problemType, byte[] data,
                            ulong solvingTimeout)
        {
            ProblemType = problemType;
            Data = data;

            SolvingTimeout = solvingTimeout;
            SolvingTimeoutSpecified = true;

            IdSpecified = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     The name of the type as given by TaskSolver
        /// </param>
        /// <param name="data">
        ///     The serialized problem data
        /// </param>
        public SolveRequestMessage(string problemType, byte[] data)
        {
            ProblemType = problemType;
            Data = data;
            SolvingTimeoutSpecified = false;

            IdSpecified = false;
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
        public static SolveRequestMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SolveRequestMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (SolveRequestMessage)serializer.Deserialize(strReader);
        }
        
        public override bool Equals(object obj)
        {
            SolveRequestMessage message = obj as SolveRequestMessage;

             
            return (Id == message.Id && ProblemType == message.ProblemType &&
                            SolvingTimeout == message.SolvingTimeout &&
                            Enumerable.SequenceEqual(Data, message.Data));
        }
    }
}
