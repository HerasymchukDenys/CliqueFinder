namespace clique.Algorithms;

public class GreedyMethod
{
    private int[,] adjacencyMatrix;
    private List<int> clique;

    public GreedyMethod(int[,] matrix)
    {
        adjacencyMatrix = matrix;
        clique = new List<int>();
    }

    public int[] FindMaximumClique()
    {
        List<int> candidates = new List<int>();
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            candidates.Add(i);
        
        foreach (int v in candidates)
            if (IsClique(v))
                clique.Add(v);
        
        return clique.ToArray();
    }

    private bool IsClique(int v)
    {
        foreach (int u in clique)
            if (adjacencyMatrix[v, u] == 0)
                return false;
        
        return true;
    }
}