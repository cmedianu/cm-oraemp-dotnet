using System;
using System.Collections.Generic;

namespace OraEmp.Domain.Entities
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employees>();
            JobHistory = new HashSet<JobHistory>();
        }

        public decimal DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public decimal? ManagerId { get; set; }
        public decimal? LocationId { get; set; }

        public virtual Locations? Location { get; set; }
        public virtual Employees? Manager { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<JobHistory> JobHistory { get; set; }
    }
}
