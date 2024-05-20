using System.Windows;
using System.Windows.Controls;
using clique.Algorithms;
using System.Diagnostics;
using clique.Validate;

namespace clique.Calculate;

public class Graph
{
    private TextBox[,] textBoxes;
    private String chosenMethod;
    
    public Window MainWindow { get; private set; }
    public int[,] AdjacencyMatrix { get; private set; }
    public int[] Clique { get; private set; }
    public double Time { get; private set; }
    public bool CorrectMatrix { get; private set; }

    public Graph(Window MainWindow, TextBox[,] textBoxes, String chosenMethod)
    {
        this.MainWindow = MainWindow;
        this.textBoxes = textBoxes;
        this.chosenMethod = chosenMethod;
        CorrectMatrix = true;
    }
    
    public void Calulate()
    {
        AdjacencyMatrix = ReadMatrix();
        if (CorrectMatrix)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            switch (chosenMethod)
            {
                case "Жадібний метод":
                    Clique = new GreedyMethod(AdjacencyMatrix).FindMaximumClique();
                    break;
                case "Алгоритм Брона-Кербоша":
                    Clique = new BronKerbosch(AdjacencyMatrix).FindMaximumClique();
                    break;
            }
            stopwatch.Stop();
            Time = stopwatch.ElapsedMilliseconds * 0.001;
        }
    }
    
    private int[,] ReadMatrix()
    {
        int matrixSize = textBoxes.GetLength(0);

        int[,] matrixValues = new int[matrixSize, matrixSize];

        for (int i = 0; i < matrixSize && CorrectMatrix; i++)
            for (int j = 0; j < matrixSize && CorrectMatrix; j++)
                if (int.TryParse(textBoxes[i, j].Text, out int value) && (value == 0 || value == 1))
                    matrixValues[i, j] = value;
                else
                {
                    new ValidateAdjacencyMatrix(MainWindow).MatrixError();
                    CorrectMatrix = false;
                    return new int[0, 0];
                }

        return matrixValues;
    }
}