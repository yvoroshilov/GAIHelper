//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GaiWcfService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shift
    {
        public int id { get; set; }
        public int responsible_id { get; set; }
        public System.DateTime start { get; set; }
        public System.DateTime end { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
