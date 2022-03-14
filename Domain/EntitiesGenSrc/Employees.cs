using System;
using System.Collections.Generic;

namespace OraEmp.Domain.Entities
{
    public partial class Employees
    {
        public Employees()
        {
            Departments = new HashSet<Department>();
            InverseManager = new HashSet<Employees>();
            JobHistory = new HashSet<JobHistory>();
        }

        public decimal EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string JobId { get; set; } = null!;
        public decimal? Salary { get; set; }
        public decimal? CommissionPct { get; set; }
        public decimal? ManagerId { get; set; }
        public decimal? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Jobs Job { get; set; } = null!;
        public virtual Employees? Manager { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Employees> InverseManager { get; set; }
        public virtual ICollection<JobHistory> JobHistory { get; set; }
    }
}
