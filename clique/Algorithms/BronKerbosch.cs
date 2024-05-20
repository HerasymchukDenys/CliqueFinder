namespace clique.Algorithms;

public class BronKerbosch
{
    private int[,] adjacencyMatrix;
    private int[] clique;
    private int maxCliqueSize;

    public BronKerbosch(int[,] matrix)
    {
        adjacencyMatrix = matrix;
        clique = new int[matrix.GetLength(0)];
        maxCliqueSize = 0;
    }
    
    private void MaximumCliqueFinder(List<int> R, List<int> P, List<int> X)
    {
        if (P.Count == 0 && X.Count == 0)
        {
            if (R.Count > maxCliqueSize)
            {
                maxCliqueSize = R.Count;
                R.CopyTo(clique, 0);
            }
            return;
        }

        foreach (int v in P.ToArray())
        {
            List<int> nextR = new List<int>(R);
            nextR.Add(v);
            List<int> nextP = new List<int>(P.FindAll(x => adjacencyMatrix[v, x] == 1));
            List<int> nextX = new List<int>(X.FindAll(x => adjacencyMatrix[v, x] == 1));

            MaximumCliqueFinder(nextR, nextP, nextX);

            P.Remove(v);
            X.Add(v);
        }
    }
    
    public int[] FindMaximumClique()
    {
        List<int> vertices = new List<int>();
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            vertices.Add(i);
        
        MaximumCliqueFinder(new List<int>(), vertices, new List<int>());

        int[] result = new int[maxCliqueSize];
        Array.Copy(clique, result, maxCliqueSize);

        return result;
    }
}