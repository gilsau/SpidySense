//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpidySense.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class WebUrl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebUrl()
        {
            this.WebUrlFields = new HashSet<WebUrlField>();
        }
    
        public int Id { get; set; }
        public int WebsiteId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string RowSelector { get; set; }
    
        public virtual Website Website { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebUrlField> WebUrlFields { get; set; }
    }
}