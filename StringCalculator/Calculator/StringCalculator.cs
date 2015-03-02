using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class StringCalculator
    {

        public StringCalculator()
        {
        }

        private bool isDelimiter(char c, List<char> delimiters) 
        {
            if (c.Equals(',') || c.Equals('\n'))
                return true;
            for (int i = 0; i < delimiters.Count; i++) 
            {
                if (delimiters.ElementAt(i).Equals(c))
                    return true;
            }
            return false;
        }

        public int run(string numberStr)
        {
            List<int> operands = new List<int>();
            List<char> delimiters = new List<char>();
            int pos = 0;
   
            string currentString = "";

            if (numberStr.Length > 1 && numberStr.Substring(0, 2).Equals("//"))
            {

                // Add new delimiters
                for (pos = 2; pos < numberStr.Length; pos++)
                {
                    char c = numberStr.ElementAt(pos);
                    int cNumber = Convert.ToInt32(c);

                    int n = 0;

                    if (c.Equals('-'))
                    {
                        string s1 = "";
                        s1 += numberStr.ElementAt(pos + 1);
                        if (int.TryParse(s1, out n))
                            break;
                    }

                    n = 0;
                    string s = "";
                    s += c;
                    bool result = int.TryParse(s, out n);
                    if (result || c.Equals('\n'))
                        break;
                    delimiters.Add(c);
                }

                if (delimiters.Count == 0)
                    pos = -1;
            }
            else { pos = -1; }
            
            /* SPLITING OF NUMBERS PROCESS*/
            for (int i = pos+1; i < numberStr.Length; i++) 
            {
                char c = numberStr.ElementAt(i);
                if (!isDelimiter(c,delimiters))
                {
                    currentString += c;                    
                }
                if (isDelimiter(c, delimiters)) 
                {
                    operands.Add(Convert.ToInt32(currentString));
                    currentString = "";
                }
                if (c.Equals('-') && !isDelimiter(c,delimiters))
                    throw new ArithmeticException();
            }
         
            // leftover string
            if(!currentString.Equals(""))
            {
                operands.Add(Convert.ToInt32(currentString));
            }
            /* END OF SPLITING OF NUMBERS PROCESS*/


            // compute the sum
            int sum = 0;
            for(int i = 0; i < operands.Count;i++)
            {
                int number = operands.ElementAt(i);
                if (number <= 1000)
                    sum += number;
            }

            return sum;

        }
    }
}
