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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public int DoctorID { get; }
        public Window1(DoctorViewModel selectedDoctor)
        {
            InitializeComponent();
            DoctorID = selectedDoctor.DoctorID;
            txtDoctorID.Text = DoctorID.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string patientName = txtPatientName.Text;
            DateTime appointmentDateTime = dpAppointmentDate.SelectedDate ?? DateTime.Now;
            using (var context = new MedicalCenterContext())
            {
                var appointment = new Appointment(patientName, DoctorID, appointmentDateTime, false);
                context.Appointments.Add(appointment);
                context.SaveChanges();
            }
            RaiseClickEvent();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public static readonly RoutedEvent ClickEvent =
         EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
         typeof(RoutedEventHandler), typeof(Window1));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected virtual void RaiseClickEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(Window1.ClickEvent); ;
            RaiseEvent(args);
        }
    }
}
