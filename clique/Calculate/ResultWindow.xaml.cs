using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using clique.DrawGraph;

namespace clique.Calculate
{
    public partial class ResultWindow : Window
    {
        public Graph graph { get; private set; }
        public bool ShowTime { get; private set; }

        public ResultWindow(Window MainWindow, Graph graph, bool showTime)
        {
            this.graph = graph;
            ShowTime = showTime;
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
            int count = 0;
            foreach (int num in graph.Clique)
            {
                if (count > 0 && count % 10 == 0)
                    StackPanel.Children.Add(new TextBlock { Text = "\n" });

                TextBlock textBlock = new TextBlock();
                textBlock.Text = num.ToString();
                if (count > 0)
                    textBlock.Text = ", " + textBlock.Text;

                StackPanel.Children.Add(textBlock);
                count++;
            }

            if (ShowTime)
            {
                TimePanel.Text = $"Час: {graph.Time:N15}  сек.";
                TimePanel.Visibility = Visibility.Visible;
            }

            if (graph.AdjacencyMatrix.GetLength(0) <= 10)
                ShowCliqueButton.Visibility = Visibility.Visible;
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowClique(object sender, EventArgs e)
        {
            GraphDrawing graphDrawing = new GraphDrawing(this, graph);
            graphDrawing.Show();
        }
    }
}