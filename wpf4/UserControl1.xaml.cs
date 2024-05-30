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

namespace laba4sem2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public event EventHandler UserControlButtonClicked;
        public event EventHandler UserControlRedoClicked;
        private void OnUserControlButtonClick()
        {
            if (UserControlButtonClicked != null)
            {
                UserControlButtonClicked(this, EventArgs.Empty);
            }
        }

        private void OnUserRedoButtonClick()
        {
            if (UserControlRedoClicked != null)
            {
                UserControlRedoClicked(this, EventArgs.Empty);
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            OnUserControlButtonClick();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            OnUserRedoButtonClick();
        }
    }
}
