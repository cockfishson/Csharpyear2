using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf10.Models;
using System.Data.Entity;

namespace wpf10
{
    public class MedicalCenterContext : DbContext
    {
        public MedicalCenterContext() : base("name=MedicalCenter")
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DoctorCategory> DoctorCategories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
