using System.Windows;


namespace clique.Validate
{
    public partial class ErrorWindow
    {
        public string ErrorMessage { get; private set; }
        
        public ErrorWindow(Window MainWindow, string errorMessage)
        {
            InitializeComponent();
            ErrorMessage = errorMessage;
            ErrorMessageTextBlock.Text = ErrorMessage;
            Owner = MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            OkButton.Click += Close;
        }
        
        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}