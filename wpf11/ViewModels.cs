using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wpf10.Models;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Windows.Input;


namespace wpf10.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<DoctorViewModel> DoctorsList { get; set; }

        private DoctorViewModel _selectedDoctor;
        public DoctorViewModel SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged("SelectedDoctor");
                if (_selectedDoctor != null)
                {
                    FetchAppointmentsForDoctor(_selectedDoctor.DoctorID);
                }
            }
        }

        private void FetchAppointmentsForDoctor(int doctorId)
        {
            using (var context = new MedicalCenterContext())
            {
                var appointments = context.Appointments
                    .Where(a => a.DoctorID == doctorId)
                    .ToList();

                SelectedDoctor.Appointments = new ObservableCollection<Appointment>(appointments);
            }
        }

        public MainViewModel()
        {
            using (var context = new MedicalCenterContext())
            {
                var doctors = context.Doctors
                   .Include(d => d.Department)
                   .Include(d => d.Category)
                   .ToList();
                DoctorsList = new ObservableCollection<DoctorViewModel>(doctors.Select(d => new DoctorViewModel(d)));
            }
        }
    }


    public class DoctorViewModel : ViewModelBase
    {
        private Doctor _doctor;
        private ObservableCollection<Appointment> _appointments;
        public ICommand CancelAppointmentCommand { get; }
        public DoctorViewModel(Doctor doctor)
        {
            _doctor = doctor;
            Appointments = new ObservableCollection<Appointment>();
            CancelAppointmentCommand = new Command().CancelAppointmentCommand;
        }

        public int DoctorID
        {
            get => _doctor.DoctorID;
            set
            {
                _doctor.DoctorID = value;
                OnPropertyChanged();
            }
        }

        public string FullName
        {
            get => _doctor.FullName;
            set
            {
                _doctor.FullName = value;
                OnPropertyChanged();
            }
        }

        public Department Department
        {
            get => _doctor.Department;
            set
            {
                _doctor.Department = value;
                OnPropertyChanged();
            }
        }

        public DoctorCategory Category
        {
            get => _doctor.Category;
            set
            {
                _doctor.Category = value;
                OnPropertyChanged();
            }
        }

        public byte[] Photo
        {
            get => _doctor.Photo;
            set
            {
                _doctor.Photo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Appointment> Appointments
        {
            get { return _appointments; }
            set
            {
                _appointments = value;
                OnPropertyChanged();
            }
        }
    }
}
