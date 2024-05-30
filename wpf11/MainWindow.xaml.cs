using System.Text;
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
using System.Data.SqlClient;
using wpf10.ViewModels;

namespace wpf10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window1 addAppointmentWindow;
        public MainWindow()
        {
            InitializeComponent();
            InitialiseData();
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel viewModel = DataContext as MainViewModel;
            if (viewModel == null || viewModel.SelectedDoctor == null)
            {
                MessageBox.Show("Please select a doctor before adding an appointment.");
                return;
            }
            addAppointmentWindow = new Window1(viewModel.SelectedDoctor);
            addAppointmentWindow.Click += ClickEventRaised;
            addAppointmentWindow.Show();
        }
        private void ClickEventRaised(object sender, RoutedEventArgs e)
        {
            InitialiseData();
        }
        void InitialiseData()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (var context = new MedicalCenterContext())
            {
                var doctorData = context.Doctors.ToList();

                doctors.AddRange(doctorData.Select(d => new Doctor
                {
                    DoctorID = d.DoctorID,
                    FullName = d.FullName,
                    Department = d.Department,
                    Category = d.Category,
                    Photo = d.Photo
                }));
            }

            DataContext = new MainViewModel();
        }
    }
}