using System;
using System.Collections.Generic;

namespace OraEmp.Domain.Entities
{
    public partial class Regions
    {
        public Regions()
        {
            Countries = new HashSet<Countries>();
        }

        public decimal RegionId { get; set; }
        public string? RegionName { get; set; }

        public virtual ICollection<Countries> Countries { get; set; }
    }
}
