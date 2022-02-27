using System;
using System.Collections.Generic;

namespace OraEmp.Domain.Entities
{
    public partial class JobHistory
    {
        public decimal EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string JobId { get; set; } = null!;
        public decimal? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Employees Employee { get; set; } = null!;
        public virtual Jobs Job { get; set; } = null!;
    }
}
