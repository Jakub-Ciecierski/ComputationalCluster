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
        private int port;

        public IPAddress Address
        { 
           get { return address; }
           set { address = value;} 
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public InputParser(string input)
        {
            this.input = input;
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

            addressStr = addressStr.Trim();

            Address = IPAddress.Parse(addressStr);

            Port = Int32.Parse(portStr);
        }
    }
}
