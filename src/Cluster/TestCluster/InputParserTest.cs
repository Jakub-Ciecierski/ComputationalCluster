using Cluster.Util.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestCluster
{
    [TestClass]
    public class InputParserTest
    {
        [TestMethod]
        public void Parse_InputLine_ServerAddress()
        {
            string expectedPortStr = "5555";
            string expectedAddressStr = "192.168.1.1";

            string inputLine = "-port " + expectedPortStr;
            inputLine += " -address " + expectedAddressStr;

            IPAddress expectedAddress = IPAddress.Parse(expectedAddressStr);
            int expectedPort = Int32.Parse(expectedPortStr);

            InputParser inputParser = new InputParser(inputLine);

            string portPat = @"-port (\d+)";
            string actualPortStr = "";
            Regex regexPort = new Regex(portPat);
            Match m = regexPort.Match(inputLine);
            if (m.Success)
            {
                actualPortStr = m.Groups[1].Value;
            }

            string addressPat = @"-address (\d.+)";
            string actualAddressStr = "";
            Regex regexAddr = new Regex(addressPat);
            m = regexAddr.Match(inputLine);
            if (m.Success)
            {
                actualAddressStr = m.Groups[1].Value;
            }

            IPAddress actualAddress = IPAddress.Parse(actualAddressStr);
            int actualPort = Int32.Parse(actualPortStr);

            Assert.AreEqual(expectedAddress, actualAddress);
            Assert.AreEqual(expectedPort, actualPort);
        }
    }
}
