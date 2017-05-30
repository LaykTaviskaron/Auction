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

        [Display(Name = "Рейтинг")]
        public Nullable<byte> Rate { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Продавець")]
        public Account User { get; set; }
    }
}