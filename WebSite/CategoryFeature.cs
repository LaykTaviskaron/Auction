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
    
    public partial class CategoryFeature
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoryFeature()
        {
            this.Specifications = new HashSet<Specification>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<byte> CategoryId { get; set; }
        public string PosibleValues { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Specification> Specifications { get; set; }
    }
}
