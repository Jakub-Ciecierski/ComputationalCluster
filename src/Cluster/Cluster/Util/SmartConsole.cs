using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Util
{
    /// <summary>
    ///     Static methods for output
    /// </summary>
    public class SmartConsole
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        const string linePrefix = " << ";

        const string headerPrefix = "";

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        private SmartConsole() { }


        /*******************************************************************/
        /******************** PUBLIC / STATIC METHODS **********************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* STATIC METHODS **************************/
        /*******************************************************************/

        public static void PrintHeader(string header) 
        {
            Console.Write("\n" + headerPrefix + header + "\n");
        }

        public static void PrintLine(string line) 
        {
            Console.Write(linePrefix + line + "\n");
        }
    }
}
