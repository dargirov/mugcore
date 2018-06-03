﻿using System;

namespace MugStore.Web.ViewModels.Home
{
    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }

        public DateTime? LastModified { get; set; }

        public double? Priority { get; set; }

        public string Url { get; set; }
    }
}
