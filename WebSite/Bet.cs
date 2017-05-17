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
    using WebSite.Models;

    public partial class Bet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bet()
        {
            this.Items = new HashSet<Item>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> ItemId { get; set; }
        public Nullable<System.Guid> BuyerId { get; set; }
        public Nullable<decimal> Amout { get; set; }
        public Nullable<byte> BetTypeId { get; set; }
    
        public virtual ApplicationUser Account { get; set; }
        public virtual Item Item { get; set; }
        public virtual BetType BetType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
    }
}
