using System.Windows;
using System.Windows.Media;
using clique.DrawGraph;

namespace clique.Calculate
{
    public partial class ResultWindow
    {
        public Graph graph { get; private set; }

        public ResultWindow(Window MainWindow, Graph graph)
        {
            this.graph = graph;
            this.Owner = MainWindow;
            this.Content = mainGrid;
            this.Foreground = Brushes.White;
            this.Owner = MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            InitializeResult();
            CloseButton.Click += Close;
            ShowCliqueButton.Click += ShowClique;
        }

        private void InitializeResult()
        {
            int count = graph.Clique.Length;
            for (int i = 0; i < count; i++)
            {
                Result.Text += graph.Clique[i];
                if (i < count - 1)
                {
                    Result.Text += ", ";
                    if ((i + 1) % 10 == 0)
                        Result.Text += "\n";
                }
            }

            if (graph.ShowComplexity)
            {
                ComplexityPanel.Text = $"Кількість ітерацій: {graph.NumberOfIterations}\n" +
                                 $"Кількість елементарних операцій: {graph.NumberOfElementaryOperations}";
                ComplexityPanel.Visibility = Visibility.Visible;
            }

            if (graph.AdjacencyMatrix.GetLength(0) < 11)
                ShowCliqueButton.Visibility = Visibility.Visible;
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowClique(object sender, EventArgs e)
        {
            GraphDrawing graphDrawing = new GraphDrawing(this, graph);
            graphDrawing.ShowDialog();
        }
    }
}