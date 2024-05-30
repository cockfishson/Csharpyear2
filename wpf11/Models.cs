using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wpf10.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }

    public class DoctorCategory
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }
        public string FullName { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual DoctorCategory Category { get; set; }

        public byte[] Photo { get; set; }
    }

    public class Specialization
    {
        [Key]
        public int SpecializationID { get; set; }
        public string SpecializationName { get; set; }
    }

    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        public string PatientName { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        public DateTime AppointmentDateTime { get; set; }
        public bool IsCancelled { get; set; }
        public Appointment(string patientName, int doctorID, DateTime appointmentDateTime, bool isCancelled)
        {
            PatientName = patientName;
            DoctorID = doctorID;
            AppointmentDateTime = appointmentDateTime;
            IsCancelled = isCancelled;
        }
        public Appointment()
        {

        }
    }
}