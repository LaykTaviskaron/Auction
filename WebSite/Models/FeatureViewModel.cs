using System;
using System.Collections.Generic;

namespace WebSite.Models
{
    public class FeatureViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte CategoryId { get; set; }

        public List<string> PosibleValues { get; set; }
    }
}