using System;
using System.Collections.Generic;

namespace WebSite.Models
{
    public class FilterModel
    {
        public IEnumerable<byte> Categories { get; set; }

        public IEnumerable<string> Features { get; set; }
    }
}