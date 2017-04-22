using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class KMP
    {
        private string pattern;
        private string text;

        private void longestPrefix(int[] lp, int len)
        {
            int lp[0] = 0;
            int i = 1;
            int j = 0;

            while (i < len)
            {
                if (pattern[i] == pattern[j]) //match
                {
                    lp[i] = j + 1;
                    i++;
                    j++;
                }
                else
                {
                    if (j > 0)
                    {
                        j = lp[j - 1];
                    }
                    else
                    {
                        lp[i] = 0;
                        i++;
                    }
                }
            }
        }

        public KMP(string _pattern, string _text)
        {
            pattern = _pattern;
            text = _text;
        }

        public void KMPsearch()
        {
            int i = 0; //iterator pattern
            int j = 0; //iterator text

            int lenText = text.Length;
            int lenPattern = pattern.Length;
            int[] lp = new int[lenPattern];

            longestPrefix(int lp, lenPattern);

            while (j < lenText)
            {
                if (pattern[i] == text[j])
                {
                    i++;
                    j++;
                }

                if (i == lenPattern) //complete matched string
                {
                    i = lp[i - 1];
                    /*
                     * <what to do?>
                     * 
                     * */
                }
                else
                {
                    if (j < lenText && pattern[i] != text[j])
                    {
                        if (i > 0) //matched before
                        {
                            i = lp[i - 1];
                        }
                        else //i = 0; hasn't matched
                        {
                            j++;
                        }
                    }
                }

            }
        }

    }
}