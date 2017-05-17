using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class ItemsViewModel
    {
        public List<Item> Suggested { get; set; }

        public List<Item> MostRecent { get; set; }
    }
}