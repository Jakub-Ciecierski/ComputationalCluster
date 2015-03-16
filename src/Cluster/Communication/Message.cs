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
    }
}
