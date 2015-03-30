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
    ///     Partial problems message is sent by the TM after dividing 
    ///     the problem into smaller partial problems. 
    ///     The data in it consists of two parts – common 
    ///     for all partial problems and specific for the given task. 
    ///     The same Partial Problems schema is used for the messages 
    ///     sent to be computed by the CN and to relay
    ///     information for synchronizing info with Backup CS.
    /// </summary>
    public partial class SolvePartialProblemsMessage : Message
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "SolvePartialProblems";

        private SolvePartialProblemsMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     The problem type name as given by TaskSolver and Client
        /// </param>
        /// <param name="id">
        ///     The ID of the problem instance assigned by the server
        /// </param>
        /// <param name="commonData">
        ///     The data to be sent to all Computational Nodes
        /// </param>
        /// <param name="solvingTimeout">
        ///     Optional time limit – set by Client (in ms)
        /// </param>
        /// <param name="partialProblems">
        ///     The partial problems to be computed
        /// </param>
        public SolvePartialProblemsMessage(string problemType, ulong id, byte[] commonData,
                                        ulong solvingTimeout, PartialProblem[] partialProblems)
        {
            ProblemType = problemType;
            Id = id;
            CommonData = commonData;
            SolvingTimeout = solvingTimeout;
            PartialProblems = partialProblems;
            SolvingTimeoutSpecified = true;
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
        public static SolvePartialProblemsMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SolvePartialProblemsMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (SolvePartialProblemsMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            SolvePartialProblemsMessage message = obj as SolvePartialProblemsMessage;


            return (Id == message.Id && ProblemType == message.ProblemType &&
                            SolvingTimeout == message.SolvingTimeout &&
                             Enumerable.SequenceEqual(CommonData, message.CommonData) &&
                            Enumerable.SequenceEqual(PartialProblems, message.PartialProblems));
        }
    }
}
