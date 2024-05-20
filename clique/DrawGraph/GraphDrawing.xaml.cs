using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using clique.Calculate;

namespace clique.DrawGraph
{
    public partial class GraphDrawing
    {
        public Graph graph { get; private set; }
        
        public GraphDrawing(Window MainWindow, Graph graph)
        {
            this.graph = graph;
            Owner = MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            Exit.Click += Close;
            graphCanvas.SizeChanged += GraphCanvas_SizeChanged;
        }

        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraph();
        }

        private void DrawGraph()
        {
            graphCanvas.Children.Clear();
            
            double canvasWidth = graphCanvas.ActualWidth;
            double canvasHeight = graphCanvas.ActualHeight;
            double centerX = canvasWidth / 2;
            double centerY = canvasHeight / 2;
            double radius = Math.Min(canvasWidth, canvasHeight) / 2 - 50;

            int size = graph.AdjacencyMatrix.GetLength(0);
            Ellipse[] nodes = new Ellipse[size];
            TextBlock[] labels = new TextBlock[size];

            for (int i = 0; i < size; i++)
            {
                double angle = 2 * Math.PI * i / size;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);

                nodes[i] = CreateNode(x, y, 20);
                labels[i] = CreateLabel(x, y, i.ToString());

                graphCanvas.Children.Add(nodes[i]);
                graphCanvas.Children.Add(labels[i]);
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (graph.AdjacencyMatrix[i, j] == 1)
                    {
                        var edge = CreateEdge(nodes[i], nodes[j], 20);
                        
                        if (graph.Clique.Contains(i) && graph.Clique.Contains(j))
                            edge.Stroke = Brushes.Green;
                        
                        graphCanvas.Children.Add(edge);
                    }
                }
            }

            foreach (int index in graph.Clique)
            {
                nodes[index].Fill = Brushes.Green;
            }
        }

        private Ellipse CreateNode(double x, double y, double radius)
        {
            var ellipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = Brushes.White
            };

            Canvas.SetLeft(ellipse, x - radius);
            Canvas.SetTop(ellipse, y - radius);

            return ellipse;
        }

        private TextBlock CreateLabel(double x, double y, string text)
        {
            var label = new TextBlock
            {
                Text = text,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold,
                FontSize = 16
            };
            
            Canvas.SetLeft(label, x - label.ActualWidth / 2);
            Canvas.SetTop(label, y - label.ActualHeight / 2);
            
            label.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            label.Arrange(new Rect(label.DesiredSize));
            Canvas.SetLeft(label, x - label.ActualWidth / 2);
            Canvas.SetTop(label, y - label.ActualHeight / 2);

            return label;
        }

        private Line CreateEdge(Ellipse node1, Ellipse node2, double radius)
        {
            double x1 = Canvas.GetLeft(node1) + radius;
            double y1 = Canvas.GetTop(node1) + radius;
            double x2 = Canvas.GetLeft(node2) + radius;
            double y2 = Canvas.GetTop(node2) + radius;
            
            double dx = x2 - x1;
            double dy = y2 - y1;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            double offsetX = radius * dx / distance;
            double offsetY = radius * dy / distance;

            var line = new Line
            {
                X1 = x1 + offsetX,
                Y1 = y1 + offsetY,
                X2 = x2 - offsetX,
                Y2 = y2 - offsetY,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            return line;
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
 


