//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntrySlipDetail
    {
        public int id { get; set; }
        public Nullable<int> entrySlipId { get; set; }
        public Nullable<int> productId { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<int> quantity { get; set; }
    
        public virtual EntrySlip EntrySlip { get; set; }
        public virtual Product Product { get; set; }
    }
}
