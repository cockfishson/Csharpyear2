using System;
using System.Windows;
using System.Windows.Controls;

namespace laba4sem2
{
    public partial class UserControl612 : UserControl
    {
        public static readonly DependencyProperty VideoMemoryTypeProperty =
        DependencyProperty.Register("VideoMemoryType", typeof(string), typeof(UserControl612),
        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        new PropertyChangedCallback(OnVideoMemoryTypeChanged), null));

        public string VideoMemoryType
        {
            get { return (string)GetValue(VideoMemoryTypeProperty); }
            set { SetValue(VideoMemoryTypeProperty, value); }
        }

        public static readonly RoutedEvent TunnelingValueChangedEvent =
            EventManager.RegisterRoutedEvent("TunnelingValueChanged", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(UserControl612));

        public event RoutedEventHandler TunnelingValueChanged
        {
            add { AddHandler(TunnelingValueChangedEvent, value); }
            remove { RemoveHandler(TunnelingValueChangedEvent, value); }
        }

        private static void OnVideoMemoryTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl612 control = (UserControl612)d;
            string newValue = (string)e.NewValue;
            if (newValue != "Wpf kakashka")
            {
                RoutedEventArgs args = new RoutedEventArgs(TunnelingValueChangedEvent, newValue);
                control.RaiseEvent(args);
            }
        }

        public UserControl612()
        {
            InitializeComponent();
        }
    }
}