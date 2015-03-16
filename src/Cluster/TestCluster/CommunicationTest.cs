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
        public void Parse_MessageObject_XMLFile()
        {
            RegisterType type = RegisterType.TaskManager;
            byte threads = 3;
            string[] problems = {"TSP","GraphColoring"};

            RegisterMessage register = new RegisterMessage(type, threads, problems);
            register.Deregister = true;
            register.Id = 5;
            string actualXmlStr = register.ToXmlString();

            XmlDocument xmlDoc = new XmlDocument();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xml_samples\register.xml");
            xmlDoc.Load(path);
            string expectedXmlStr = xmlDoc.OuterXml;

            Assert.AreEqual(expectedXmlStr, actualXmlStr);
        }
    }
}
