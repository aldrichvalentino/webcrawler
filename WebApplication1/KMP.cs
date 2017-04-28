/**
 * File : KMP.cs
 * Author : AAR
 * Aldrich Valentino H. - 13515081
 * Roland Hartanto - 13515107
 * M. Akmal Pratama - 13515135
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1
{
    public class KMP
    {
        private string pattern;
        private string text;

        private void longestPrefix(int[] lp, int len)
        {
            lp[0] = 0;
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

        public int KMPsearch()
        {
            int i = 0; //iterator pattern
            int j = 0; //iterator text

            int lenText = text.Length;
            int lenPattern = pattern.Length;
            int[] lp = new int[lenPattern];

            longestPrefix(lp, lenPattern);

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
                    return j - lenPattern + 1;
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
            return -1;
        }

        public String getKMPResult()
        {
            int result = KMPsearch();
            int leftOffset = 50;
            int rightOffset = 50;
            if (result == -1)
            {
                char[] temp = new char[9];
                temp[0] = 'n';
                temp[1] = 'o';
                temp[2] = 't';
                temp[3] = ' ';
                temp[4] = 'f';
                temp[5] = 'o';
                temp[6] = 'u';
                temp[7] = 'n';
                temp[8] = 'd';

                return new String(temp);
            }
            else
            {
                char[] temp = new char[leftOffset + rightOffset + 1 +8+ pattern.Length];
                for (int it = 0; it < temp.Length; it++)
                {
                    temp[it] = '\0';
                }
                int i = result - leftOffset;
                int j = result + rightOffset;

                if (i < 0)
                {
                    i = 0;
                }

                if (j > text.Length - 1)
                {
                    j = text.Length - 1;
                }
                /*
                for (int k = i; k <= j; k++)
                {
                    temp[k - i] = text[k];
                }*/
                for (int k = i; k < result - 1; k++)
                {
                    temp[k - i] = text[k];
                }
                temp[result - i - 1] = '<';
                temp[result + 1 - i - 1] = 'b';
                temp[result + 2 - i - 1] = '>';
                int l = result - 1;
                for (int k = result + 3 - 1; k < result + 3 - 1 + pattern.Length; k++)
                {
                    temp[k - i] = text[l];
                    l++;
                }
                temp[result + 3 + pattern.Length - i - 1] = '<';
                temp[result + 3 + pattern.Length - i + 1 - 1] = '/';
                temp[result + 3 + pattern.Length - i + 2 - 1] = 'b';
                temp[result + 3 + pattern.Length - i + 3 - 1] = '>';
                for (int k = result + 3 + pattern.Length - 1 + 4; k < j; k++)
                {
                    temp[k - i] = text[l];
                    l++;
                }
                if (i != 0)
                {
                    temp[0] = '.';
                    temp[1] = '.';
                    temp[2] = '.';
                }

                if (j != text.Length - 1)
                {
                    int k = temp.Length - 1;
                    while (k >= 0 && temp[k] == '\0') k--;
                    temp[k] = '.';
                    temp[k - 1] = '.';
                    temp[k - 2] = '.';
                }
                //String a = new String(temp);
                return new String(temp);
            }
        }


    }
}