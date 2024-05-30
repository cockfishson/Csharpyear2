using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace laba4sem2
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
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
    public partial class Page3 : Page, INotifyPropertyChanged
    {
        private bool _isEditing;
        private GPU _gpu;
        List<GPU> gpuList = new List<GPU>();
        private void StartUpPageThree()
        {
            FileInfo fileInfo = new FileInfo("gpus.json");
            if (fileInfo.Length > 0)
            {
                Console.WriteLine("File is not empty.");
                foreach (string line in File.ReadLines("gpus.json"))
                {
                    List<GPU> deserializedGPU = JsonSerializer.Deserialize<List<GPU>>(line);
                    gpuList.AddRange(deserializedGPU);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private string _editedName;
        public string EditedName
        {
            get => _editedName;
            set
            {
                _editedName = value;
                OnPropertyChanged(nameof(EditedName));
            }
        }
        private string _editedDescription;
        public string EditedDescription
        {
            get => _editedName;
            set
            {
                _editedDescription = value;
                OnPropertyChanged(nameof(EditedDescription));
            }
        }
        public string Image;
        private int _editedCost;
        public int EditedCost
        {
            get => _editedCost;
            set
            {
                _editedCost = value;
                OnPropertyChanged(nameof(EditedCost));
            }
        }

        private int _editedFrequency;
        public int EditedFrequency
        {
            get => _editedFrequency;
            set
            {
                _editedFrequency = value;
                OnPropertyChanged(nameof(EditedFrequency));
            }
        }

        private string _editedMemoryType;
        public string EditedMemoryType
        {
            get => _editedMemoryType;
            set
            {
                _editedMemoryType = value;
                OnPropertyChanged(nameof(EditedMemoryType));
            }
        }

        private int _editedMemory;
        public int EditedMemory
        {
            get => _editedMemory;
            set
            {
                _editedMemory = value;
                OnPropertyChanged(nameof(EditedMemory));
            }
        }

        private int _editedBit;
        public int EditedBit
        {
            get => _editedBit;
            set
            {
                _editedBit = value;
                OnPropertyChanged(nameof(EditedBit));
            }
        }

        private int _editedRating;
        public int EditedRating
        {
            get => _editedRating;
            set
            {
                _editedRating = value;
                OnPropertyChanged(nameof(EditedRating));
            }
        }

        private int _editedAmount;
        public int EditedAmount
        {
            get => _editedAmount;
            set
            {
                _editedAmount = value;
                OnPropertyChanged(nameof(EditedAmount));
            }
        }

        private string _editedProducer;
        public string EditedProducer
        {
            get => _editedProducer;
            set
            {
                _editedProducer = value;
                OnPropertyChanged(nameof(EditedProducer));
            }
        }

        private string _editedModel;
        public string EditedModel
        {
            get => _editedModel;
            set
            {
                _editedModel = value;
                OnPropertyChanged(nameof(EditedModel));
            }
        }

        private string _editedCompanyAmdOrIntel;
        public string EditedCompanyAmdOrIntel
        {
            get => _editedCompanyAmdOrIntel;
            set
            {
                _editedCompanyAmdOrIntel = value;
                OnPropertyChanged(nameof(EditedCompanyAmdOrIntel));
            }
        }
        public GPU GPU
        {
            get => _gpu;
            set
            {
                _gpu = value;
                OnPropertyChanged(nameof(GPU));
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public bool IsReadOnly => !IsEditing;

        public string ButtonText => IsEditing ? "Сохранить" : "Изменить";

        public ICommand ToggleEditingCommand { get; }
        public Page3(GPU parsedGpu)
        {
            InitializeComponent();
            StartUpPageThree();
            GPU = parsedGpu;
            ToggleEditingCommand = new RelayCommand(ToggleEditing);
            ResetEditedValues();
            DataContext = this;
        }


        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Page2 page2 = new Page2();
            NavigationService.Navigate(page2);
        }
        private void ToggleEditing(object obj)
        {
            IsEditing = !IsEditing;

            if (!IsEditing)
            {
                int x = 0;
                for (int i = 0; i < gpuList.Count; i++) 
                { 
                    if (gpuList[i].Name == GPU.Name) 
                    {
                        x = i;
                    }
                }
                GPU.Name = EditedName;
                GPU.Cost = EditedCost;
                GPU.Frequency = EditedFrequency;
                GPU.Memory = EditedMemory;
                GPU.MemoryType = EditedMemoryType;
                GPU.Bit = EditedBit;
                GPU.Rating = EditedRating;
                GPU.Amount = EditedAmount;
                GPU.Producer = EditedProducer;
                GPU.Model = EditedModel;
                GPU.CompanyAmdOrIntel = EditedCompanyAmdOrIntel;
                GPU.Description = EditedDescription;
                gpuList[x] = GPU;
                string fileName = "gpus.json";
                string jsonString = JsonSerializer.Serialize(gpuList);
                File.WriteAllText(fileName, jsonString);
                StartUpPageThree();
                ResetEditedValues();
            }
        }

        private void ResetEditedValues()
        {
            EditedName = GPU.Name;
            EditedCost = GPU.Cost;
            EditedFrequency = GPU.Frequency;
            EditedMemory = GPU.Memory;
            EditedMemoryType = GPU.MemoryType;
            EditedBit = GPU.Bit;
            EditedRating = GPU.Rating;
            EditedAmount = GPU.Amount;
            EditedProducer = GPU.Producer;
            this.Image = GPU.Img;
            EditedModel = GPU.Model;
            EditedCompanyAmdOrIntel = GPU.CompanyAmdOrIntel;
            EditedDescription = GPU.Description;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            for (int i = 0; i < gpuList.Count; i++)
            {
                if (gpuList[i].Name == GPU.Name)
                {
                    x = i;
                }
            }
            gpuList.RemoveAt(x);
            string fileName = "gpus.json";
            string jsonString = JsonSerializer.Serialize(gpuList);
            File.WriteAllText(fileName, jsonString);
            Page2 page2 = new Page2();
            NavigationService.Navigate(page2);
        }
    }
}
