using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using clique.Calculate;
using clique.File;
using clique.Validate;

namespace clique;

public partial class MainWindow
{
    private TextBox[,] textBoxes;
    private Graph graph;
    
    public MainWindow()
    {
        InitializeComponent();
        Loaded += InitializeComboBox;
        ComboBoxSize.SelectionChanged += InitializeTemplateOfMatrix;
        StartFinding.Click += InitializeFinding;
        StartFinding.Click += ReadFileNameAndFolderName;
        ComboBoxFile.SelectionChanged += InitializeFolderPathField;
        ComboBoxFile.SelectionChanged += InitializeFileNameField;
        Exit.Click += CloseProgram;
    }

    private void InitializeComboBox(object sender, EventArgs e)
    {
        for (int i = 3; i <= 30; i++)
            ComboBoxSize.Items.Add(new ComboBoxItem { Content = i });
    }
    
    private void InitializeTemplateOfMatrix(object sender, EventArgs e)
    {
        int matrixSize = int.Parse(((ComboBoxItem)ComboBoxSize.SelectedItem).Content.ToString());
        textBoxes = new TextBox[matrixSize, matrixSize];
        
        MatrixContainer.Children.Clear();
        MatrixContainer.RowDefinitions.Clear();
        MatrixContainer.ColumnDefinitions.Clear();

        double cellSize = 30;
        for (int i = 0; i < matrixSize; i++)
        {
            MatrixContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellSize) });
            MatrixContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(cellSize) });
        }

        for (int i = 0; i < matrixSize; i++)
            for (int j = 0; j < matrixSize; j++)
            {
                TextBox textBox = new TextBox()
                {
                    Text = "0",
                    Width = cellSize,
                    Height = cellSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                
                Grid.SetRow(textBox, i);
                Grid.SetColumn(textBox, j);
                MatrixContainer.Children.Add(textBox);
        
                textBoxes[i, j] = textBox;

                textBox.TextChanged += (s, args) =>
                {
                    int row = Grid.GetRow((TextBox)s);
                    int column = Grid.GetColumn((TextBox)s);

                    if (int.TryParse(((TextBox)s).Text, out int value))
                        if (row == column && value == 1)
                            new ValidateAdjacencyMatrix(this).TextBox_DiagonalElementEqualsOne((TextBox)s);
                        else
                        {
                            textBoxes[row, column].Text = value.ToString();
                            if (row != column)
                                textBoxes[column, row].Text = value.ToString();
                        }
                };
        
                textBox.PreviewTextInput += new ValidateAdjacencyMatrix(this).TextBox_PreviewTextInput;
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            }
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            int row = Grid.GetRow(textBox);
            int column = Grid.GetColumn(textBox);

            switch (e.Key)
            {
                case Key.Up:
                    if (row > 0)
                    {
                        textBoxes[row - 1, column].Focus();
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    if (row < textBoxes.GetLength(0) - 1)
                    {
                        textBoxes[row + 1, column].Focus();
                        e.Handled = true;
                    }
                    break;
                case Key.Left:
                    if (column > 0)
                    {
                        textBoxes[row, column - 1].Focus();
                        e.Handled = true;
                    }
                    break;
                case Key.Right:
                    if (column < textBoxes.GetLength(1) - 1)
                    {
                        textBoxes[row, column + 1].Focus();
                        e.Handled = true;
                    }
                    break;
            }
        }
    }
    
    private void InitializeFolderPathField(object sender, EventArgs e)
    {
        if (ComboBoxFile.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedValue = selectedItem.Content.ToString();
            if (selectedValue == "Так")
            {
                TextBlock FolderPathLabel = new TextBlock
                {
                    Name = "FolderPathLabel",
                    Text = "Шлях до директорії",
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 210, 10, 0)
                };
                
                TextBox FolderPathTextBox = new TextBox
                {
                    Name = "FolderPathTextBox",
                    Margin = new Thickness(0, 230, 10, 0),
                    Width = 210,
                    Height = 20,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                
                ToolContainer.Children.Add(FolderPathLabel);
                ToolContainer.Children.Add(FolderPathTextBox);
            }
            else
            {
                RemoveElementByName("FolderPathLabel");
                RemoveElementByName("FolderPathTextBox");
            }
        }
    }

    private void InitializeFileNameField(object sender, EventArgs e)
    {
        if (ComboBoxFile.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedValue = selectedItem.Content.ToString();
            if (selectedValue == "Так")
            {
                TextBlock FileNameLabel = new TextBlock
                {
                    Name ="FileNameLabel",
                    Text = "Ім'я файлу",
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 260, 10, 0)
                };
                
                TextBox FileNameTextBox = new TextBox
                {
                    Name = "FileNameTextBox",
                    Margin = new Thickness(0, 280, 10, 0),
                    Width = 210,
                    Height = 20,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                
                ToolContainer.Children.Add(FileNameLabel);
                ToolContainer.Children.Add(FileNameTextBox);
            }
            else
            {
                RemoveElementByName("FileNameLabel");
                RemoveElementByName("FileNameTextBox");
            }
        }
    }
    
    private void InitializeFinding(object sender, EventArgs e)
    {
        string method = ((ComboBoxItem)ComboBoxMethod.SelectedItem).Content.ToString();
        graph = new Graph(this, textBoxes, method);
        graph.Calulate();
        if(graph.CorrectMatrix)
            ShowResult();
    }
    
    private void ReadFileNameAndFolderName(object sender, EventArgs e)
    {
        if (ComboBoxFile.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedValue = selectedItem.Content.ToString();
            if (selectedValue == "Ні")
                return;
            
            TextBox folderPathTextBox = ToolContainer.Children.OfType<TextBox>()
                    .FirstOrDefault(tb => tb.Name == "FolderPathTextBox");
            string folderPath = folderPathTextBox?.Text;

            TextBox fileNameTextBox = ToolContainer.Children.OfType<TextBox>()
                .FirstOrDefault(tb => tb.Name == "FileNameTextBox");
            string fileName = fileNameTextBox?.Text;

            WriteToFile writeToFile = new WriteToFile(folderPath, fileName, graph);

            writeToFile.Run(this);
        }
    }

    private void CloseProgram(object sender, EventArgs e)
    {
        Close();
    }

    private void ShowResult()
    {
        string toDoComplexity = ((ComboBoxItem)ComboBoxComplexity.SelectedItem).Content.ToString();
        bool showTime = toDoComplexity == "Так";
        
        ResultWindow resultWindow = new ResultWindow(this, graph, showTime);
        resultWindow.ShowDialog();
    }
    
    private void RemoveElementByName(string name)
    {
        UIElement elementToRemove = null;
        foreach (UIElement element in ToolContainer.Children)
            if (element is FrameworkElement frameworkElement && frameworkElement.Name == name)
            {
                elementToRemove = element;
                break;
            }
        
        if (elementToRemove != null)
            ToolContainer.Children.Remove(elementToRemove);
    }
}