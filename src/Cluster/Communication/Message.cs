using Communication.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Communication
{
    public abstract class Message
    {
        /// <summary>
        ///     Serializes this object to xml string.
        ///     Used in ToXmlString()
        /// </summary>
        /// <param name="encoding">
        ///     Encoding for the xml
        /// </param>
        /// <returns>
        ///     xml in string
        /// </returns>
        private string serializeToXmlString(Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());

            // create a MemoryStream here, we are just working
            // exclusively in memory
            Stream stream = new MemoryStream();

            // The XmlTextWriter takes a stream and encoding
            // as one of its constructors
            XmlTextWriter xtWriter = new XmlTextWriter(stream, encoding);

            serializer.Serialize(xtWriter, this);

            xtWriter.Flush();

            // go back to the beginning of the Stream to read its contents
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            // read back the contents of the stream and supply the encoding
            StreamReader reader = new StreamReader(stream, encoding);

            string xmlStr = reader.ReadToEnd();

            xtWriter.Close();

            return xmlStr;
        }

        private static string concCompabilityIssuesStringWojtekIsGay(string xmlFile)
        {
            const string encoding = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            const string _namespace = "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.mini.pw.edu.pl/ucc/\"";

            if (xmlFile.Contains(encoding))
                return xmlFile;
            string totalFile = "";

            // find first '>'
            totalFile += encoding;

            int index = xmlFile.IndexOf('>');
            string part_before_namespace = xmlFile.Substring(0, index);
            part_before_namespace += " " + _namespace;
            string part_after_namespace = xmlFile.Substring(index, xmlFile.Length - index);

            part_before_namespace += part_after_namespace;

            totalFile += part_before_namespace;

            return totalFile;
        }

        /// <summary>
        /// Serializes this object to xml string with defualt UTF8 encoding
        /// </summary>
        /// <returns> Xml in string </returns>
        public string ToXmlString()
        {
            return serializeToXmlString(Encoding.UTF8);
        }

        /// <summary>
        /// Serializes this object to xml string with given encoding
        /// </summary>
        /// <param name="encoding">
        ///     Encoding for the xml file
        /// </param>
        /// <returns> 
        ///     Xml in string
        /// </returns>
        public string ToXmlString(Encoding encoding)
        {
            return serializeToXmlString(encoding);
        }

        /// <summary>
        ///     Creates a xml file from this object
        /// </summary>
        /// <param name="filepath">
        ///     The path to the xml file to be created
        /// </param>
        public void ToXmlFile(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            TextWriter textWriter = new StreamWriter(filepath);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        /// <summary>
        ///     Reads xml's header and returns its name (name of the first element)
        /// </summary>
        /// <param name="xmlString">
        ///     The xml to be parsed
        /// </param>
        /// <returns>
        ///     name of the xml
        /// </returns>
        public static string GetMessageName(string xmlString)
        {
            if (xmlString.Equals(""))
                return "";
            // Read the first element
            XmlReader reader = XmlReader.Create(new StringReader(xmlString));
            while (reader.NodeType != XmlNodeType.Element)
                reader.Read();
            string name = reader.Name;

            return name;
        }

        /// <summary>
        ///     Constructs message by given xml string
        /// </summary>
        /// <param name="xmlString">
        ///     xml in string 
        /// </param>
        /// <returns>
        ///     Message constructed by the xml
        /// </returns>
        public static Message Construct(string xmlString)
        {
            xmlString = concCompabilityIssuesStringWojtekIsGay(xmlString);

            if (GetMessageName(xmlString) == RegisterMessage.ELEMENT_NAME)
                return RegisterMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == DivideProblemMessage.ELEMENT_NAME)
                return DivideProblemMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == NoOperationMessage.ELEMENT_NAME)
                return NoOperationMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == RegisterResponseMessage.ELEMENT_NAME)
                return RegisterResponseMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == SolutionRequestMessage.ELEMENT_NAME)
                return SolutionRequestMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == SolutionsMessage.ELEMENT_NAME)
                return SolutionsMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == SolvePartialProblemsMessage.ELEMENT_NAME)
            {
                try
                {
                    //return SolvePartialProblemsMessage.Construct(xmlString);
                }
                catch (InvalidOperationException e) { return null; }
                Console.WriteLine("******** SOLVE PARTIAL PROBLEM MESSAGE **************");
                return SolvePartialProblemsMessage.Construct(xmlString);
            }
            if (GetMessageName(xmlString) == SolveRequestMessage.ELEMENT_NAME)
                return SolveRequestMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == SolveRequestResponseMessage.ELEMENT_NAME)
                return SolveRequestResponseMessage.Construct(xmlString);
            if (GetMessageName(xmlString) == StatusMessage.ELEMENT_NAME)
                return StatusMessage.Construct(xmlString);
            return null;
        }
    }
}
