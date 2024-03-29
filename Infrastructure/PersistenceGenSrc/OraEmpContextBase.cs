﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OraEmp.Domain.Entities;

namespace OraEmp.Infrastructure.Persistence
{
    public partial class OraEmpContextBase : DbContext
    {
        public OraEmpContextBase(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Countries> Countries { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employees> Employees { get; set; } = null!;
        public virtual DbSet<JobHistory> JobHistory { get; set; } = null!;
        public virtual DbSet<Jobs> Jobs { get; set; } = null!;
        public virtual DbSet<Locations> Locations { get; set; } = null!;
        public virtual DbSet<Regions> Regions { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("HR")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("COUNTRY_C_ID_PK");

                entity.ToTable("COUNTRIES");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COUNTRY_ID")
                    .IsFixedLength();

                entity.Property(e => e.CountryName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("COUNTRY_NAME");

                entity.Property(e => e.RegionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REGION_ID");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Countries)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("COUNTR_REG_FK");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId)
                    .HasName("DEPT_ID_PK");

                entity.ToTable("DEPARTMENTS");

                entity.HasIndex(e => e.LocationId, "DEPT_LOCATION_IX");

                entity.Property(e => e.DepartmentId)
                    .HasPrecision(4)
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DEPARTMENT_NAME");

                entity.Property(e => e.LocationId)
                    .HasPrecision(4)
                    .HasColumnName("LOCATION_ID");

                entity.Property(e => e.ManagerId)
                    .HasPrecision(6)
                    .HasColumnName("MANAGER_ID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("DEPT_LOC_FK");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("DEPT_MGR_FK");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("EMP_EMP_ID_PK");

                entity.ToTable("EMPLOYEES");

                entity.HasIndex(e => e.DepartmentId, "EMP_DEPARTMENT_IX");

                entity.HasIndex(e => e.Email, "EMP_EMAIL_UK")
                    .IsUnique();

                entity.HasIndex(e => e.JobId, "EMP_JOB_IX");

                entity.HasIndex(e => e.ManagerId, "EMP_MANAGER_IX");

                entity.HasIndex(e => new { e.LastName, e.FirstName }, "EMP_NAME_IX");

                entity.Property(e => e.EmployeeId)
                    .HasPrecision(6)
                    .HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.CommissionPct)
                    .HasColumnType("NUMBER(2,2)")
                    .HasColumnName("COMMISSION_PCT");

                entity.Property(e => e.DepartmentId)
                    .HasPrecision(4)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.HireDate)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("HIRE_DATE");

                entity.Property(e => e.JobId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("JOB_ID");

                entity.Property(e => e.LastName)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.ManagerId)
                    .HasPrecision(6)
                    .HasColumnName("MANAGER_ID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NUMBER");

                entity.Property(e => e.Salary)
                    .HasColumnType("NUMBER(8,2)")
                    .HasColumnName("SALARY");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("EMP_DEPT_FK");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EMP_JOB_FK");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("EMP_MANAGER_FK");
            });

            modelBuilder.Entity<JobHistory>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.StartDate })
                    .HasName("JHIST_EMP_ID_ST_DATE_PK");

                entity.ToTable("JOB_HISTORY");

                entity.HasIndex(e => e.DepartmentId, "JHIST_DEPARTMENT_IX");

                entity.HasIndex(e => e.EmployeeId, "JHIST_EMPLOYEE_IX");

                entity.HasIndex(e => e.JobId, "JHIST_JOB_IX");

                entity.Property(e => e.EmployeeId)
                    .HasPrecision(6)
                    .HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.DepartmentId)
                    .HasPrecision(4)
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("DATE")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.JobId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("JOB_ID");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.JobHistory)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("JHIST_DEPT_FK");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.JobHistory)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("JHIST_EMP_FK");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobHistory)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("JHIST_JOB_FK");
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("JOB_ID_PK");

                entity.ToTable("JOBS");

                entity.Property(e => e.JobId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("JOB_ID");

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("JOB_TITLE");

                entity.Property(e => e.MaxSalary)
                    .HasPrecision(6)
                    .HasColumnName("MAX_SALARY");

                entity.Property(e => e.MinSalary)
                    .HasPrecision(6)
                    .HasColumnName("MIN_SALARY");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("LOC_ID_PK");

                entity.ToTable("LOCATIONS");

                entity.HasIndex(e => e.City, "LOC_CITY_IX");

                entity.HasIndex(e => e.CountryId, "LOC_COUNTRY_IX");

                entity.HasIndex(e => e.StateProvince, "LOC_STATE_PROVINCE_IX");

                entity.Property(e => e.LocationId)
                    .HasPrecision(4)
                    .HasColumnName("LOCATION_ID");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CITY");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COUNTRY_ID")
                    .IsFixedLength();

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.StateProvince)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("STATE_PROVINCE");

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("STREET_ADDRESS");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("LOC_C_ID_FK");
            });

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .HasName("REG_ID_PK");

                entity.ToTable("REGIONS");

                entity.Property(e => e.RegionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("REGION_NAME");
            });

            modelBuilder.HasSequence("DEPARTMENTS_SEQ").IncrementsBy(10);

            modelBuilder.HasSequence("EMPLOYEES_SEQ");

            modelBuilder.HasSequence("LOCATIONS_SEQ").IncrementsBy(100);

            OnModelCreatingPartial(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
