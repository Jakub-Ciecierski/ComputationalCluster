using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;
using Communication.Messages;
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
            //noOperation.ToXmlFile(path);
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
    }
}
