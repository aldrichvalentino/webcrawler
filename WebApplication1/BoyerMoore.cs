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

        private static int[] lastOccurance()
        {
            int lo = new int[128];
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

        public static int boyerMooreSearch()
        {
            List<int> ret = new List<int>();
            int len_text = text.Length;
            int len_pattern = pattern.Length;

            int last_occurance[] = lastOccurance();
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
                    int skip_pattern = ending_pattern_it - last_occurance[text[begining_pattern_it + ending_pattern_it]]
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
            /*int i = lenPattern - 1;

            if (i > lenText)
                return -1;
            int j = lenPattern - 1;

            do
            {
                if (pattern[j] == text[i])
                {
                    if (j == 0)
                    {
                        //MATCH
                    }
                    else
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    int lo = last[text[i]];
                    i = i + lenPattern - Math.Min(j, 1 + lo);
                    j = lenPattern - 1;
                }
            } while (i <= lenText - 1);
            return -1;
            */
        }
    }
}