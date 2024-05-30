using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf10.Models;
using wpf10.ViewModels;

namespace wpf10
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public int DoctorID { get; }
        public Page1(DoctorViewModel selectedDoctor)
        {
            InitializeComponent();
            DoctorID = selectedDoctor.DoctorID;
            txtDoctorID.Text = DoctorID.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string patientName = txtPatientName.Text;
            DateTime appointmentDateTime = dpAppointmentDate.SelectedDate ?? DateTime.Now;
            int lastAppointmentId = 0;
            using (var context = new MedicalCenterContext())
            {
                if (context.Appointments.Any())
                {
                    lastAppointmentId = context.Appointments.Max(a => a.AppointmentID);
                }
            }
            int appointmentId = lastAppointmentId + 1;
            var appointment = new Appointment(patientName, DoctorID, appointmentDateTime, false);
            using (var context = new MedicalCenterContext())
            {
                context.Appointments.Add(appointment);
                context.SaveChanges();
            }
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
