namespace clique.Algorithms;

public class GreedyMethod
{
    private int[,] adjacencyMatrix;
    private List<int> clique;
    public int NumberOfIterations { get; private set; }
    public int NumberOfElementaryOperations { get; private set; }

    public GreedyMethod(int[,] matrix)
    {
        adjacencyMatrix = matrix;
        clique = new List<int>();
    }

    public int[] FindMaximumClique()
    {
        List<int> candidates = new List<int>();
        NumberOfElementaryOperations++; ////
        
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
        {
            NumberOfIterations++; ////
            candidates.Add(i);
            NumberOfElementaryOperations++; ////
        }

        foreach (int v in candidates)
        {
            NumberOfIterations++; ////ПОч
            NumberOfElementaryOperations++; ////
            if (IsClique(v))
            {
                clique.Add(v);
                NumberOfElementaryOperations++; ////
            }
        }

        NumberOfElementaryOperations++; ////
        return clique.ToArray();
    }

    private bool IsClique(int v)
    {
        NumberOfElementaryOperations++;
        foreach (int u in clique)
        {
            NumberOfIterations++; ////
            NumberOfElementaryOperations++; ////
            if (adjacencyMatrix[v, u] == 0)
                return false;
        }
        
        return true;
    }
}