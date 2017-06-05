using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class FeedbackModel
    {
        public System.Guid Id { get; set; }

        [Display(Name = "Rate")]
        public byte Rate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Seller")]
        public Account User { get; set; }
    }
}