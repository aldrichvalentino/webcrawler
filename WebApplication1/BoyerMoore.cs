using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class BoyerMoore
    {
        private string pattern;
        private string text;

        public BoyerMoore(string patternInput, string textInput)
        {
            pattern = patternInput;
            text = textInput;
        }

        private int[] lastOccurance()
        {
            int[] lo = new int[128];
            for (int i = 0; i < 128; i++)
            {
                lo[i] = -1;
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                lo[pattern[i]] = i;
            }
            return lo;
        }

        public int[] boyerMooreSearch()
        {
            List<int> ret = new List<int>();
            int len_text = text.Length;
            int len_pattern = pattern.Length;

            int[] last_occurance = lastOccurance();
            int begining_pattern_it = 0;

            while (begining_pattern_it <= len_text - len_pattern)
            {
                int ending_pattern_it = len_pattern - 1;

                while (ending_pattern_it >= 0 && pattern[ending_pattern_it] == text[begining_pattern_it + ending_pattern_it])
                {
                    ending_pattern_it--;
                }
                if (ending_pattern_it >= 0) //still need to compare
                {
                    int skip_pattern = ending_pattern_it - last_occurance[text[begining_pattern_it + ending_pattern_it]];
                    begining_pattern_it += Math.Max(1, skip_pattern);
                }
                else
                {
                    ret.Add(begining_pattern_it); //Matched pattern
                    if ((begining_pattern_it + len_pattern) >= len_text)
                    {
                        begining_pattern_it += 1;
                    }
                    else
                    {
                        int skip_pattern = len_pattern - last_occurance[text[begining_pattern_it + len_pattern]];
                        begining_pattern_it += skip_pattern;
                    }
                }
            }

            return ret.ToArray();
        }

        public String getBoyerMooreResult()
        {
            int[] resultArray = boyerMooreSearch();
            int result = -1;
            foreach (int it in resultArray)
            {
                result = it;
            }
            int leftOffset = 20;
            int rightOffset = 20;
            if (result == -1)
            {
                return "not found";
            }
            else
            {
                char[] temp = new char[leftOffset + rightOffset + 1];
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

                for (int k = i; k <= j; k++)
                {
                    temp[k - i] = text[k];
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