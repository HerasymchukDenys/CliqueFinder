namespace clique.Algorithms;

public class GreedyMethod
{
    private int[,] adjacencyMatrix;
    private int[] clique;

    public GreedyMethod(int[,] matrix)
    {
        adjacencyMatrix = matrix;
        clique = new int[matrix.GetLength(0)];
    }

    public int[] FindMaximumClique()
    {
        List<int> candidates = new List<int>();
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            candidates.Add(i);
        
        List<int> clique = new List<int>();

        foreach (int v in candidates)
            if (IsClique(v, clique))
                clique.Add(v);
        
        return clique.ToArray();
    }

    private bool IsClique(int v, List<int> clique)
    {
        foreach (int u in clique)
            if (adjacencyMatrix[v, u] == 0)
                return false;
        
        return true;
    }
}