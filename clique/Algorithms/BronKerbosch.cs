namespace clique.Algorithms;

public class BronKerbosch
{
    private int[,] adjacencyMatrix;
    private int[] clique;
    private int maxCliqueSize;
    public int NumberOfIterations { get; private set; }
    public int NumberOfElementaryOperations { get; private set; }

    public BronKerbosch(int[,] matrix)
    {
        adjacencyMatrix = matrix;
        clique = new int[matrix.GetLength(0)];
        maxCliqueSize = 0;
    }
    
    private void MaximumCliqueFinder(List<int> R, List<int> P, List<int> X)
    {
        NumberOfElementaryOperations++; ////
        
        if (P.Count == 0 && X.Count == 0)
        {
            NumberOfElementaryOperations++; ////
            if (R.Count > maxCliqueSize)
            {
                maxCliqueSize = R.Count;
                R.CopyTo(clique, 0);
                NumberOfElementaryOperations += 2; ////
            }

            NumberOfElementaryOperations++; ////
            return;
        }

        foreach (int v in P.ToArray())
        {
            NumberOfIterations++; ////
            List<int> nextR = new List<int>(R);
            nextR.Add(v);
            List<int> nextP = new List<int>(P.FindAll(x => adjacencyMatrix[v, x] == 1));
            List<int> nextX = new List<int>(X.FindAll(x => adjacencyMatrix[v, x] == 1));
            NumberOfElementaryOperations += 3; ////
            
            MaximumCliqueFinder(nextR, nextP, nextX);
            NumberOfElementaryOperations++; ////
            
            P.Remove(v);
            X.Add(v);
            NumberOfElementaryOperations += 2; ////
        }
    }
    
    public int[] FindMaximumClique()
    {
        List<int> vertices = new List<int>();
        NumberOfElementaryOperations++; ////

        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
        {
            NumberOfIterations++; ////
            vertices.Add(i);
            NumberOfElementaryOperations++; ////
        }

        NumberOfElementaryOperations++; ////
        MaximumCliqueFinder(new List<int>(), vertices, new List<int>());

        int[] result = new int[maxCliqueSize];
        Array.Copy(clique, result, maxCliqueSize);

        NumberOfElementaryOperations += 3; ////
        return result;
    }
}