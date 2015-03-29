using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication.MessageComponents;
using Communication.Messages;
using Communication;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace TestCluster
{
    [TestClass]
    public class CommunicationTest
    {
        [TestMethod]
        public void Parse_RegisterMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\Register.xml");

            RegisterType type = RegisterType.TaskManager;
            byte threads = 3;
            string[] problems = {"TSP","GraphColoring"};

            RegisterMessage register = new RegisterMessage(type, threads, problems);
            register.Deregister = true;
            register.Id = 5;
            string actualXmlStr = register.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_NoOperationMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\NoOperation.xml");

            BackupCommunicationServer backupServer1
                = new BackupCommunicationServer("192.168.1.0", 80);

            BackupCommunicationServer backupServer2
                = new BackupCommunicationServer("192.168.1.0", 80);

            BackupCommunicationServer[] backupServers =
            {
                backupServer1, backupServer2
            };

            NoOperationMessage noOperation = new NoOperationMessage(backupServers);

            string actualXmlStr = noOperation.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_DivideProblemMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\DivideProblem.xml");

            string problemType = "TSP";
            ulong problemId = 12;
            byte[] data = {
                             1,2,4,6,5,4,32,3
                          };
            ulong nodesCount = 16;
            ulong nodeId = 8;
            DivideProblemMessage message = new DivideProblemMessage(
                    problemType, problemId, data, nodesCount, nodeId );
     
 
            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_RegisterResponseMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\RegisterResponse.xml");

            ulong id = 9;
            uint timeout = 11111;
            BackupCommunicationServer[] backupServers= 
            {
                new BackupCommunicationServer("192.168.1.10", 80),
                new BackupCommunicationServer("192.168.1.11", 80),
            };

            RegisterResponseMessage message = new RegisterResponseMessage(id, timeout, backupServers);

            //message.ToXmlFile(path);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_SolutionRequestMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolutionRequest.xml");

            ulong id = 9;

            SolutionRequestMessage message = new SolutionRequestMessage(id);

            //message.ToXmlFile(path);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }


        [TestMethod]
        public void Parse_SolutionsMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\Solutions.xml");

            // construct fields for SolutionsMessage
            string problemType = "TSP";
            ulong id = 12;
            byte[] commonData = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };

            // fields for Solution1
            ulong taskId1 = 123;
            bool timeoutOccured1 = false;
            SolutionsSolutionType typeField1 = SolutionsSolutionType.Final;
            ulong computationsTime1 = 12334;
            byte[] data1 = {24,252,6,43,57,88};
            Solution solution1 = new Solution(taskId1, timeoutOccured1, typeField1,computationsTime1,data1);

            // fields for Solution2
            ulong taskId2 = 321;
            bool timeoutOccured2 = true;
            SolutionsSolutionType typeField2 = SolutionsSolutionType.Ongoing;
            ulong computationsTime2 = 43321;
            byte[] data2 = {24,25,6,3,7,8};
            Solution solution2 = new Solution(taskId2, timeoutOccured2, typeField2, computationsTime2, data2);

            Solution[] solutions = { solution1, solution2 };

            SolutionsMessage message = new SolutionsMessage(problemType, id, commonData, solutions);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_SolvePartialProblemMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolvePartialProblems.xml");

            string problemType = "TSP";
            ulong id = 12;
            byte[] commonData = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };
            ulong solvingTimeout = 1000;

            // fields for PartialProblem1
            ulong taskId1 = 123;
            byte[] data1 = { 24, 252, 6, 43, 57, 88 };
            ulong nodeId1= 1;
            PartialProblem partialProblem1 = new PartialProblem(taskId1, data1, nodeId1);

            // fields for PartialProblem2
            ulong taskId2 = 321;
            byte[] data2 = { 24, 252, 6, 43, 57, 88 };
            ulong nodeId2 = 2;
            PartialProblem partialProblem2 = new PartialProblem(taskId2, data2, nodeId2);

            PartialProblem[] partialProblems = { partialProblem1, partialProblem2 };

            SolvePartialProblemsMessage message = new SolvePartialProblemsMessage(problemType, id, commonData, 
                                            solvingTimeout, partialProblems);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_SolveRequestMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolveRequest.xml");

            string problemType = "TSP";
            ulong id = 12;
            byte[] data = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };
            ulong solvingTimeout = 1000;

            SolveRequestMessage message = new SolveRequestMessage(problemType, data,
                                            solvingTimeout, id);

            message.ToXmlFile(path);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_SolveRequestResponseMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolveRequestResponse.xml");

            ulong id = 12;

            SolveRequestResponseMessage message = new SolveRequestResponseMessage(id);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_StatusMessage_XMLString()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\Status.xml");

            ulong id = 12;

            StatusThread statusThread1 = new StatusThread(StatusThreadState.Idle);
            StatusThread statusThread2 = new StatusThread(StatusThreadState.Idle);

            ulong howLong = 1000;
            ulong problemInstanceId = 2;
            ulong taskId = 1;
            string problemType = "TCP";

            StatusThread statusThread3 = new StatusThread(StatusThreadState.Busy, howLong, problemInstanceId, taskId, problemType);

            StatusThread[] statusThreads = { statusThread1, statusThread2, statusThread3 };

            StatusMessage message = new StatusMessage(id, statusThreads);

            string actualXmlStr = message.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }

        [TestMethod]
        public void Parse_XMLString_StatusMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\Status.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            StatusMessage actualMessage = null;

            if(name == StatusMessage.ELEMENT_NAME)
                actualMessage = StatusMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            ulong id = 12;

            StatusThread statusThread1 = new StatusThread(StatusThreadState.Idle);
            StatusThread statusThread2 = new StatusThread(StatusThreadState.Idle);

            ulong howLong = 1000;
            ulong problemInstanceId = 2;
            ulong taskId = 1;
            string problemType = "TCP";

            StatusThread statusThread3 = new StatusThread(StatusThreadState.Busy, howLong, problemInstanceId, taskId, problemType);

            StatusThread[] statusThreads = { statusThread1, statusThread2, statusThread3 };

            StatusMessage expectedMessage = new StatusMessage(id, statusThreads);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_SolveRequestResponseMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolveRequestResponse.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            SolveRequestResponseMessage actualMessage = null;

            if (name == SolveRequestResponseMessage.ELEMENT_NAME)
                actualMessage = SolveRequestResponseMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            ulong id = 12;

            SolveRequestResponseMessage expectedMessage = new SolveRequestResponseMessage(id);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_SolveRequestMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolveRequest.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            SolveRequestMessage actualMessage = null;

            if (name == SolveRequestMessage.ELEMENT_NAME)
                actualMessage = SolveRequestMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            string problemType = "TSP";
            ulong id = 12;
            byte[] data = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };
            ulong solvingTimeout = 1000;

            SolveRequestMessage expectedMessage = new SolveRequestMessage(problemType, data,
                                            solvingTimeout, id);
            

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_SolvePartialProblemsMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolvePartialProblems.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            SolvePartialProblemsMessage actualMessage = null;

            if (name == SolvePartialProblemsMessage.ELEMENT_NAME)
                actualMessage = SolvePartialProblemsMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            string problemType = "TSP";
            ulong id = 12;
            byte[] commonData = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };
            ulong solvingTimeout = 1000;

            // fields for PartialProblem1
            ulong taskId1 = 123;
            byte[] data1 = { 24, 252, 6, 43, 57, 88 };
            ulong nodeId1 = 1;
            PartialProblem partialProblem1 = new PartialProblem(taskId1, data1, nodeId1);

            // fields for PartialProblem2
            ulong taskId2 = 321;
            byte[] data2 = { 24, 252, 6, 43, 57, 88 };
            ulong nodeId2 = 2;
            PartialProblem partialProblem2 = new PartialProblem(taskId2, data2, nodeId2);

            PartialProblem[] partialProblems = { partialProblem1, partialProblem2 };

            SolvePartialProblemsMessage expectedMessage = new SolvePartialProblemsMessage(problemType, id, commonData,
                                            solvingTimeout, partialProblems);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_SolutionMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\Solutions.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            SolutionsMessage actualMessage = null;

            if (name == SolutionsMessage.ELEMENT_NAME)
                actualMessage = SolutionsMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            // construct fields for SolutionsMessage
            string problemType = "TSP";
            ulong id = 12;
            byte[] commonData = { 1, 23, 25, 2, 5, 5, 5, 5, 2, 26, 87 };

            // fields for Solution1
            ulong taskId1 = 123;
            bool timeoutOccured1 = false;
            SolutionsSolutionType typeField1 = SolutionsSolutionType.Final;
            ulong computationsTime1 = 12334;
            byte[] data1 = { 24, 252, 6, 43, 57, 88 };
            Solution solution1 = new Solution(taskId1, timeoutOccured1, typeField1, computationsTime1, data1);

            // fields for Solution2
            ulong taskId2 = 321;
            bool timeoutOccured2 = true;
            SolutionsSolutionType typeField2 = SolutionsSolutionType.Ongoing;
            ulong computationsTime2 = 43321;
            byte[] data2 = { 24, 25, 6, 3, 7, 8 };
            Solution solution2 = new Solution(taskId2, timeoutOccured2, typeField2, computationsTime2, data2);

            Solution[] solutions = { solution1, solution2 };

            SolutionsMessage expectedMessage = new SolutionsMessage(problemType, id, commonData, solutions);


            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_SolutionRequestMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\SolutionRequest.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            SolutionRequestMessage actualMessage = null;

            if (name == SolutionRequestMessage.ELEMENT_NAME)
                actualMessage = SolutionRequestMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            ulong id = 9;

            SolutionRequestMessage expectedMessage = new SolutionRequestMessage(id);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_RegisterResponseMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\RegisterResponse.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            RegisterResponseMessage actualMessage = null;

            if (name == RegisterResponseMessage.ELEMENT_NAME)
                actualMessage = RegisterResponseMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            ulong id = 9;
            uint timeout = 11111;
            BackupCommunicationServer[] backupServers = 
            {
                new BackupCommunicationServer("192.168.1.10", 80),
                new BackupCommunicationServer("192.168.1.11", 80),
            };

            RegisterResponseMessage expectedMessage = new RegisterResponseMessage(id, timeout, backupServers);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_DivideProblemMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\DivideProblem.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            DivideProblemMessage actualMessage = null;

            if (name == DivideProblemMessage.ELEMENT_NAME)
                actualMessage = DivideProblemMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            string problemType = "TSP";
            ulong problemId = 12;
            byte[] data = {
                             1,2,4,6,5,4,32,3
                          };
            ulong nodesCount = 16;
            ulong nodeId = 8;
            DivideProblemMessage expectedMessage = new DivideProblemMessage(
                    problemType, problemId, data, nodesCount, nodeId);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_RegisterMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\Register.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            RegisterMessage actualMessage = null;

            if (name == RegisterMessage.ELEMENT_NAME)
                actualMessage = RegisterMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            RegisterType type = RegisterType.TaskManager;
            byte threads = 3;
            string[] problems = { "TSP", "GraphColoring" };

            RegisterMessage expectedMessage = new RegisterMessage(type, threads, problems);
            expectedMessage.Deregister = true;
            expectedMessage.Id = 5;

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Parse_XMLString_NoOperationMessage()
        {
            /*********** Actual message ***********/
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\NoOperation.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xmlStr = xmlDoc.OuterXml;

            string name = Message.GetMessageName(xmlStr);
            NoOperationMessage actualMessage = null;

            if (name == NoOperationMessage.ELEMENT_NAME)
                actualMessage = NoOperationMessage.Construct(xmlStr);

            /*********** Expected message ***********/
            BackupCommunicationServer backupServer1
               = new BackupCommunicationServer("192.168.1.0", 80);

            BackupCommunicationServer backupServer2
                = new BackupCommunicationServer("192.168.1.0", 80);

            BackupCommunicationServer[] backupServers =
            {
                backupServer1, backupServer2
            };

            NoOperationMessage expectedMessage = new NoOperationMessage(backupServers);

            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
