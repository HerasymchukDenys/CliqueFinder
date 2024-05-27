using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using clique.Calculate;

namespace clique.DrawGraph;

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
        GraphCanvas.SizeChanged += GraphCanvasSizeChanged;
    }

    private void GraphCanvasSizeChanged(object sender, SizeChangedEventArgs e)
    {
        GraphCanvas.Children.Clear();
        DrawGraph();
    }

    private void DrawGraph()
    {
        Point center = new Point(GraphCanvas.ActualWidth / 2, GraphCanvas.ActualHeight / 2);
        double radius = Math.Min(GraphCanvas.ActualWidth, GraphCanvas.ActualHeight) / 2 - 50;

        int size = graph.AdjacencyMatrix.GetLength(0);
        Ellipse[] nodes = new Ellipse[size];
        TextBlock[] labels = new TextBlock[size];

        for (int i = 0; i < size; i++)
        {
            double angle = 2 * Math.PI * i / size;
            Point point = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
            nodes[i] = CreateNode(point.X, point.Y, 20);
            labels[i] = CreateLabel(point.X, point.Y, i.ToString());

            GraphCanvas.Children.Add(nodes[i]);
            GraphCanvas.Children.Add(labels[i]);
        }
            
        DrowCliqueNodes(nodes);
            
        for (int i = 0; i < size; i++)
            for (int j = i + 1; j < size; j++)
                if (graph.AdjacencyMatrix[i, j] == 1)
                {
                    Line edge = CreateEdge(nodes[i], nodes[j], 20);
                        
                    if (graph.Clique.Contains(i) && graph.Clique.Contains(j))
                        edge.Stroke = Brushes.Green;
                        
                    GraphCanvas.Children.Add(edge);
                }
    }

    private Ellipse CreateNode(double x, double y, double radius)
    {
        Ellipse ellipse = new Ellipse
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
        TextBlock label = new TextBlock
        {
            Text = text,
            Foreground = Brushes.Black,
            FontWeight = FontWeights.Bold,
            FontSize = 16
        };
        
        label.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
        label.Arrange(new Rect(label.DesiredSize));
        Canvas.SetLeft(label, x - label.ActualWidth / 2);
        Canvas.SetTop(label, y - label.ActualHeight / 2);

        return label;
    }

    private Line CreateEdge(Ellipse firstNode, Ellipse secondNode, double radius)
    {
        Point FirstPoint = new Point(Canvas.GetLeft(firstNode) + radius, Canvas.GetTop(firstNode) + radius);
        Point SecondPoint = new Point(Canvas.GetLeft(secondNode) + radius, Canvas.GetTop(secondNode) + radius);
        Point Line = new Point(SecondPoint.X - FirstPoint.X, SecondPoint.Y - FirstPoint.Y);
        
        double distance = Math.Sqrt(Line.X * Line.X + Line.Y * Line.Y);
        double offsetX = radius * Line.X / distance;
        double offsetY = radius * Line.Y / distance;

        Line line = new Line
        {
            X1 = FirstPoint.X + offsetX,
            Y1 = FirstPoint.Y + offsetY,
            X2 = SecondPoint.X - offsetX,
            Y2 = SecondPoint.Y - offsetY,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        return line;
    }

    private void DrowCliqueNodes(Ellipse[] nodes)
    {
        foreach (int index in graph.Clique)
            nodes[index].Fill = Brushes.Green;
    }

    private void Close(object sender, EventArgs e)
    {
        this.Close();
    }
}

 


