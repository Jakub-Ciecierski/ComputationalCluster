using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cluster.Util.Client
{
    public class InputParser
    {
        private string input;

        private IPAddress address;

        public IPAddress Address
        { 
           get { return address; }
           set { address = value;} 
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private IPAddress masterAddress;

        public IPAddress MasterAddress
        {
            get { return masterAddress; }
            set { masterAddress = value; }
        }

        private int masterPort;

        public int MasterPort
        {
            get { return masterPort; }
            set { masterPort = value; }
        }

        public InputParser(string input)
        {
            this.input = input;

            Address = null;
            Port = 0;

            MasterAddress = null;
            MasterPort = 0;
        }

        public void ParseInput()
        {
            InputParser inputParser = new InputParser(input);

            string portPat = @"-port (\d+)";

            string portStr = "";
            Regex regexPort = new Regex(portPat);
            Match m = regexPort.Match(input);
            if (m.Success)
            {
                portStr = m.Groups[1].Value;
            }


            string addressPat = @"-address (\d.+)";

            string addressStr = "";
            Regex regexAddr = new Regex(addressPat);
            m = regexAddr.Match(input);
            if (m.Success)
            {
                addressStr = m.Groups[1].Value;
            }

            if (!addressStr.Equals("")) {
                addressStr = addressStr.Trim();
                Address = IPAddress.Parse(addressStr);
            }
            
            if(!portStr.Equals(""))
                Port = Int32.Parse(portStr);


            string masterPortPat = @"-mport (\d+)";

            string masterPortStr = "";
            Regex regexMasterPort = new Regex(masterPortPat);
            m = regexMasterPort.Match(input);
            if (m.Success)
            {
                masterPortStr = m.Groups[1].Value;
            }


            string masterAddressPat = @"-maddress (\d.+)";

            string masterAddressStr = "";
            Regex regexMasterAddr = new Regex(masterAddressPat);
            m = regexMasterAddr.Match(input);
            if (m.Success)
            {
                masterAddressStr = m.Groups[1].Value;
            }


            if (!masterAddressStr.Equals(""))
            {
                masterAddressStr = masterAddressStr.Trim();
                MasterAddress = IPAddress.Parse(masterAddressStr);
            }

            if (!masterPortStr.Equals(""))
                MasterPort = Int32.Parse(masterPortStr);
        }
    }
}
