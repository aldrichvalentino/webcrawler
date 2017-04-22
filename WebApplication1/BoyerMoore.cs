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

        public void boyerMooreSearch()
        {

        }
    }
}