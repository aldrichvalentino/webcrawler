﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class Feeds
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public bool ContainsKeyword { get; set; }
        public string KeyWordSentence { get; set; }
    }
}