using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace clique.Validate;

public class ValidateAdjacencyMatrix
{
    public Window MainWindow { get; set; }
    
    public ValidateAdjacencyMatrix(Window MainWindow)
    {
        this.MainWindow = MainWindow;
    }
    
    public void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!IsNumber(e.Text) || (e.Text != "0" && e.Text != "1"))
        {
            e.Handled = true;
            TextBox textBox = sender as TextBox;
            if (textBox != null)
                textBox.Text = "0";
            
            MatrixError();
        }
    }

    private bool IsNumber(string text)
    {
        return int.TryParse(text, out _);
    }
    
    public void TextBox_DiagonalElementEqualsOne(TextBox textBox)
    {
        ErrorWindow errorWindow = new ErrorWindow(MainWindow,
            "Цикли у графі призводять до некоректної роботи алгоритмів");
        errorWindow.ShowDialog();
        textBox.Text = "0";
    }
    
    public void MatrixError()
    {
        ErrorWindow errorWindow1 = new ErrorWindow(MainWindow,
            "Матриця суміжності може містити лише елементи 0 чи 1");
        errorWindow1.ShowDialog();
    }
}