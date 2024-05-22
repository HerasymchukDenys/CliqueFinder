using System.Windows;
using System.Windows.Controls;
using clique.Algorithms;
using System.Diagnostics;
using clique.Validate;

namespace clique.Calculate;

public class Graph
{
    private TextBox[,] textBoxes;

    public Window MainWindow { get; private set; }
    public int[,] AdjacencyMatrix { get; private set; }
    public bool CorrectMatrix { get; private set; }
    public String ChosenMethod { get; private set; }
    public int[] Clique { get; private set; }
    public bool ShowComplexity { get; private set; }
    public int NumberOfIterations { get; private set; }
    public int NumberOfElementaryOperations { get; private set; }

    public Graph(Window MainWindow, TextBox[,] textBoxes, String ChosenMethod, bool ShowComplexity)
    {
        this.MainWindow = MainWindow;
        this.textBoxes = textBoxes;
        this.ChosenMethod = ChosenMethod;
        this.ShowComplexity = ShowComplexity;
        CorrectMatrix = true;
    }
    
    public void Calulate()
    {
        AdjacencyMatrix = ReadMatrix();
        if (CorrectMatrix)
        {
            switch (ChosenMethod)
            {
                case "Жадібний метод":
                    GreedyMethod greedyMethod = new GreedyMethod(AdjacencyMatrix);
                    Clique = greedyMethod.FindMaximumClique();
                    NumberOfIterations = greedyMethod.NumberOfIterations;
                    NumberOfElementaryOperations = greedyMethod.NumberOfElementaryOperations;
                    break;
                case "Алгоритм Брона-Кербоша":
                    BronKerbosch bronKerbosch = new BronKerbosch(AdjacencyMatrix);
                    Clique = bronKerbosch.FindMaximumClique();
                    NumberOfIterations = bronKerbosch.NumberOfIterations;
                    NumberOfElementaryOperations = bronKerbosch.NumberOfElementaryOperations;
                    break;
            }
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