using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace clique.Validate
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Window MainWindow, string errorMessage)
        {
            InitializeComponent();
            ErrorMessage = errorMessage;
            ErrorMessageTextBlock.Text = ErrorMessage;
            Owner = MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            OkButton.Click += Close;
        }

        public string ErrorMessage { get; private set; }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}