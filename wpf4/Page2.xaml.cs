using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using static laba4sem2.laba611;

namespace laba4sem2
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public string lastTheme = "Theme3.xaml";
        public string nexttheme = "Theme1.xaml";
        Stack<Action> undoStack = new Stack<Action>();
        Stack<Action> redoStack = new Stack<Action>();
        Stack<Properties> properties = new Stack<Properties>();
        Stack<Properties> Redoproperties = new Stack<Properties>();
        List<string> modelList = new List<string> { "Все модели" };
        UserControl1 userControl1;
        UserControl612 userControl612;
        laba611 aba611;
        public int Amount=-1;
        public string MemoryType="";
        public int Quantity =0;
        public Page2()
        {
            StartUpPageTwo();
            InitializeComponent();
            var exampleLol = gpuList;
            userControl1 = new UserControl1();
            userControl612 = new UserControl612();
            aba611 = new laba611();
            this.UndoRedo.Children.Add(userControl1);
            this.Laba7.Children.Add(userControl612);
            this.Laba7.Children.Add(aba611);
            aba611.ValueChanged += laba611_ValueChanged;
            userControl612.TunnelingValueChanged += UserControl612_TunnelingValueChanged;
            userControl1.UserControlButtonClicked += new EventHandler(UndoClick);
            userControl1.UserControlRedoClicked += new EventHandler(RedoClick);
            CommandBindings.Add(new CommandBinding(CustomCommands.SetToZeroCommand, SetToZeroExecuted));
            modelList.AddRange(exampleLol.Select(gpu => gpu.Model).Distinct());
            LViewParts.ItemsSource = exampleLol;
            ComboType.ItemsSource = modelList;
            ComboType.SelectedIndex = 0;
            CheckActual.IsChecked = true;
            TBoxMin.Text = "0";
            TBoxMax.Text = "0";
            Properties properties1 = new Properties(gpuList, modelList, 0,true,"0","0","");
            this.ClosePage2();
            undoStack.Push(()=>ClosePage2());
        }
        public class Properties
        {
            public List<GPU> gPUs = new List<GPU>();
            public List<string> models = new List<string>();
            public int Combo;
            public bool isChecked;
            public string Min;
            public string Max;
            public string Search;
            public Properties(List<GPU> gPUs, List<string> models, int combo, bool ischecked, string min, string max, string search)
            {
                this.gPUs = gPUs;
                this.models = models;
                Combo = combo;
                isChecked = ischecked;
                Min = min;
                Max = max;
                Search = search;
            }
        }
        List<GPU> gpuList = new List<GPU>();
        private void SetToZeroExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Amount = 0;
            UpdateCatalog();
        }
        private void StartUpPageTwo()
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
        public void ClosePage2()
        {
            NavigationService?.Navigate(new Page1());
        }

        public void UpdateCatalog()
        {
            gpuList.Clear();
            LViewParts.ItemsSource = gpuList;
            StartUpPageTwo();
            var Curent = gpuList.ToList();


            if (ComboType.SelectedIndex > 0)
            {
                Curent = Curent.Where(p => p.Model.Contains(ComboType.SelectedItem.ToString())).ToList();
            }

            Curent = Curent.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (CheckActual.IsChecked.Value)
            {
                Curent = Curent.Where(p => p.Amount > 0).ToList();
            }
            bool Converted = false;
            int MinValue;
            int MaxValue;
            Converted = Int32.TryParse(TBoxMax.Text,out MaxValue);
            Converted = Int32.TryParse(TBoxMin.Text,out MinValue);
            if(Converted == true && MaxValue > MinValue)
            {
                Curent = Curent.Where(p => p.Cost >= MinValue && p.Cost <= MaxValue).ToList();
            }
            if(Amount !=0 && Amount !=-1)
            {
                Curent = Curent.Where(p=> p.Amount == Amount).ToList();
            }
            if (MemoryType != "")
            {
                Curent = Curent.Where(p=>p.MemoryType == MemoryType).ToList();
            }
            LViewParts.ItemsSource = Curent;
            int x = ComboType.SelectedIndex;
            bool y = CheckActual.IsChecked.Value;
            string s = TBoxMin.Text;
            string v = TBoxMax.Text;
            string o = TBoxSearch.Text;
            Properties properties1 = new Properties(gpuList, modelList,x, y,s, v, o); 
            properties.Push(properties1);
            undoStack.Push(() => RevertCatalog());
        }
        
        public void RecoverCatalog() {
            if (Redoproperties.Count > 0)
            {
                var lol = Redoproperties.Pop();
                int x = ComboType.SelectedIndex;
                bool y = CheckActual.IsChecked.Value;
                string s = TBoxMin.Text;
                string v = TBoxMax.Text;
                string o = TBoxSearch.Text;
                Properties properties1 = new Properties(gpuList, modelList, x, y, s, v, o);
                properties.Push(properties1);
                var exampleLol = lol.gPUs;
                modelList = lol.models;
                LViewParts.ItemsSource = exampleLol;
                ComboType.ItemsSource = modelList;
                ComboType.SelectedIndex = lol.Combo;
                CheckActual.IsChecked = lol.isChecked;
                TBoxMin.Text = lol.Min;
                TBoxMax.Text = lol.Max;
                TBoxSearch.Text = lol.Search;
                UpdateCatalog();
            }
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBoxSearch.Text.Length >= 2)
            {
                UpdateCatalog();
            }
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCatalog();
        }

        private void CheckActual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCatalog();
        }

        private void CheckActual_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCatalog();
        }
        public void UndoClick(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                var backMethod = undoStack.Pop();
                backMethod();
            }
        }
        public void RedoClick(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                var forwardMethod = redoStack.Pop();
                forwardMethod();
            }
        }
        private void TBoxMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBoxMax.Text.Length > 2 || TBoxMin.Text.Length > 2)
            {
                UpdateCatalog();
            }
        }
        public void RevertCatalog()
        {
            if (properties.Count > 0)
            {
                var lol = properties.Pop();
                int x = ComboType.SelectedIndex;
                bool y = CheckActual.IsChecked.Value;
                string s = TBoxMin.Text;
                string v = TBoxMax.Text;
                string o = TBoxSearch.Text;
                Properties properties1 = new Properties(gpuList, modelList, x, y, s, v, o);
                Redoproperties.Push(properties1);
                var exampleLol = lol.gPUs;
                modelList = lol.models;
                LViewParts.ItemsSource = exampleLol;
                ComboType.ItemsSource = modelList;
                ComboType.SelectedIndex = lol.Combo;
                CheckActual.IsChecked = lol.isChecked;
                TBoxMin.Text = lol.Min;
                TBoxMax.Text = lol.Max;
                TBoxSearch.Text = lol.Search;
                var Curent = exampleLol;

                Curent = Curent.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

                if (CheckActual.IsChecked.Value)
                {
                    Curent = Curent.Where(p => p.Amount > 0).ToList();
                }
                LViewParts.ItemsSource = Curent;
                redoStack.Push(() => RecoverCatalog());

            }
        }

        private void LViewParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LViewParts.SelectedItem != null)
                {
                GPU selectedItem = LViewParts.SelectedItem as GPU; 
                Page3 page3 = new Page3(selectedItem);
                NavigationService.Navigate(page3);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page4 page4 = new Page4();
            NavigationService.Navigate(page4);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Equals(Application.Current.Resources.MergedDictionaries[0].Source, new System.Uri("Strings.ru-RU.xaml", System.UriKind.Relative)))
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.en-EN.xaml", System.UriKind.Relative) });
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.ru-RU.xaml", System.UriKind.Relative) });
            }
            if (Equals(Application.Current.Resources.MergedDictionaries[0].Source, new System.Uri("Theme1.xaml", System.UriKind.Relative)))
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Theme1.xaml", System.UriKind.Relative) });
            }
            else if (Equals(Application.Current.Resources.MergedDictionaries[0].Source, new System.Uri("Theme2.xaml", System.UriKind.Relative)))
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Theme2.xaml", System.UriKind.Relative) });
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Theme3.xaml", System.UriKind.Relative) });
            }
            NavigationService.Refresh();
            undoStack.Push(() => UndoLanguage());
        }
        public void UndoLanguage()
        {
            if (Equals(Application.Current.Resources.MergedDictionaries[0].Source, new System.Uri("Strings.ru-RU.xaml", System.UriKind.Relative)))
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.en-EN.xaml", System.UriKind.Relative) });
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.ru-RU.xaml", System.UriKind.Relative) });
            }
            NavigationService.Refresh();
            redoStack.Push(() => RedoLanguage());
        }
        public void RedoLanguage()
        {
            if (Equals(Application.Current.Resources.MergedDictionaries[0].Source, new System.Uri("Strings.ru-RU.xaml", System.UriKind.Relative)))
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.en-EN.xaml", System.UriKind.Relative) });
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.ru-RU.xaml", System.UriKind.Relative) });
            }
            NavigationService.Refresh();
            undoStack.Push(() => UndoLanguage());
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                lastTheme = nexttheme;
                string themeName = button.Content.ToString();
                string themePath = $"{themeName}.xaml";
                nexttheme = themePath;
                ResourceDictionary newTheme = new ResourceDictionary();
                newTheme.Source = new Uri(themePath, UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.en-EN.xaml", System.UriKind.Relative) });
                undoStack.Push(() => UndoTheme());
                NavigationService.Refresh();

            }
        }
        public void UndoTheme()
        {
            ResourceDictionary newTheme = new ResourceDictionary();
            newTheme.Source = new Uri(lastTheme, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(newTheme);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.en-EN.xaml", System.UriKind.Relative) });
            redoStack.Push(() => RedoTheme());
        }
        public void RedoTheme()
        {
            ResourceDictionary newTheme = new ResourceDictionary();
            newTheme.Source = new Uri(nexttheme, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(newTheme);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri("Strings.en-EN.xaml", System.UriKind.Relative) });
            undoStack.Push(() => UndoTheme());
        }


        private void laba611_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is laba611 control)
            {
                if(Int32.TryParse(control.quantity, out int newValue))
                Amount = (int)newValue;
                UpdateCatalog();
            }
        }

        private void UserControl612_TunnelingValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is UserControl612 control)
            {
                string newValue = control.VideoMemoryType;
                MemoryType = newValue;
                UpdateCatalog();
            }
        }


    }
}
