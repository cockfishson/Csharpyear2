using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace laba4sem2
{
    public partial class laba611 : UserControl
    {
        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register("quantity", typeof(string), typeof(laba611),
                new FrameworkPropertyMetadata("0", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnQuantityChanged), new CoerceValueCallback(CoerceQuantity)));

        public string quantity { 
            get { string kek = (string)GetValue(QuantityProperty); if (string.IsNullOrEmpty(kek) == false) { return (string)GetValue(QuantityProperty); } else { return "14"; } }
            set { bool lol; int lmao; lol = Int32.TryParse(value, out lmao); if (lol == true) { Quantity = lmao; } else { Quantity = 14; } }
        }
        public int Quantity;

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(laba611));

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private static object CoerceQuantity(DependencyObject d, object value)
        {
            if (value is string strValue && string.IsNullOrEmpty(strValue))
                return "0";
            int Lol;
            bool kek = Int32.TryParse(value.ToString(), out Lol);
            if (kek == false) return "0";
            else
            {
                if (Lol < 0)
                    return "0";
                return value;
            }
        }

        private static void OnQuantityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            laba611 control = (laba611)d;
            if (e.NewValue is string && string.IsNullOrEmpty((string)e.NewValue) != true)
            {
                if (int.TryParse((string)e.NewValue, out int newValue))
                {
                    if (newValue > 0 && newValue < int.MaxValue)
                    {
                        RoutedEventArgs args = new RoutedEventArgs(ValueChangedEvent, newValue);
                        control.RaiseEvent(args);
                    }
                }
            }
        }

        public static class CustomCommands
        {
            public static readonly RoutedUICommand SetToZeroCommand = new RoutedUICommand(
                "Set to Zero",
                "SetToZero",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                new KeyGesture(Key.D, ModifierKeys.Control)
                }
            );
        }

        public laba611()
        {
            InitializeComponent();
            this.DataContext = this;

        }
    }
}