//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSite
{
    using System;
    using System.Collections.Generic;
    
    public partial class Feedback
    {
        public System.Guid Id { get; set; }
        public byte Rate { get; set; }
        public string Description { get; set; }
        public System.Guid UserId { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
