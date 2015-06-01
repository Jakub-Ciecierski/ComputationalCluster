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
        public enum DebugLevel
        {
            Basic,
            Advanced
        };

        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        public const DebugLevel DEBUG_LEVEL = DebugLevel.Basic;

        const string linePrefix = " >> ";

        const string headerPrefix = "*********************** ";
        const string headerSuffix = " ***********************";

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
            Console.Write("\n\n" + headerPrefix + header + headerSuffix + "\n\n");
        }

        public static void PrintLine(string line, DebugLevel debugLevel) 
        {
            if (debugLevel >= DEBUG_LEVEL )
                Console.Write(linePrefix + line + "\n");
        }
    }
}
