using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpf10
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
        public class Command
        {
            public ICommand CancelAppointmentCommand { get; }

            public Command()
            {
                CancelAppointmentCommand = new RelayCommand(CancelAppointment);
            }

            private void CancelAppointment(object parameter)
            {
                if (parameter is string appointmentIdString && int.TryParse(appointmentIdString, out int appointmentId))
                {
                    using (var context = new MedicalCenterContext())
                    {
                        var appointment = context.Appointments.Find(appointmentId);
                        if (appointment != null)
                        {
                            appointment.IsCancelled = true;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }

