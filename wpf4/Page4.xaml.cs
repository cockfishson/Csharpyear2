using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        List<GPU> gpuList = new List<GPU>();
        private void StartUpPageFour()
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
        public Page4()
        {
            InitializeComponent();
            StartUpPageFour();
            DataContext = new GPUViewModel();
        }
        public class GPUViewModel
        {
            public string EditedName { get; set; }
            public int EditedCost { get; set; }
            public int EditedFrequency { get; set; }
            public int EditedMemory { get; set; }
            public string EditedMemoryType { get; set; }
            public int EditedBit { get; set; }
            public string Image { get; set; }
            public int EditedRating { get; set; }
            public int EditedAmount { get; set; }
            public string EditedProducer { get; set; }
            public string EditedModel { get; set; }
            public string EditedCompanyAmdOrIntel { get; set; }
            public string EditedDescription { get; set; }
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            GPUViewModel viewModel = (GPUViewModel)DataContext;
            GPU gpu = new GPU
            (
                viewModel.EditedCost,
                viewModel.EditedFrequency,
                viewModel.EditedMemory,
                viewModel.EditedMemoryType,
                viewModel.EditedBit,
                viewModel.EditedRating,
                viewModel.EditedAmount,
                viewModel.EditedName,
                viewModel.Image,
                viewModel.EditedDescription,
                viewModel.EditedModel,
                viewModel.EditedProducer,
                viewModel.EditedCompanyAmdOrIntel
            );
            gpuList.Add(gpu);
            if (gpu.Name != null && gpu.Producer != null)
            {
                string fileName = "gpus.json";
                string jsonString = JsonSerializer.Serialize(gpuList);
                File.WriteAllText(fileName, jsonString);

            }
            Page2 page2 = new Page2();
            NavigationService.Navigate(page2);
        }
    }
}
