//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_Api_EPamphalet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Choice
    {
        public int Choices_Id { get; set; }
        public Nullable<int> FKCustomer_id { get; set; }
        public Nullable<int> FKVendor_id { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
