using System.IO;
using System.Windows;
using clique.Calculate;
using clique.Validate;

namespace clique.File
{
    public class WriteToFile
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public Graph graph { get; set; }

        public WriteToFile(string path, string fileName, Graph graph)
        {
            Path = path;
            FileName = fileName + ".txt";
            this.graph = graph;
        }

        public void Run(Window MainWindow)
        {
            if (!TrySetDirectory())
            {
                new ErrorWindow(MainWindow,
                    $"Не знайдено директорію з таким іменем \"{Path}\"").ShowDialog();
                return;
            }

            if (!TryWrite())
            {
                new ErrorWindow(MainWindow,
                    $"Не вдалося записати до файлу \"{FileName}\"").ShowDialog();
            }
        }

        private bool TrySetDirectory()
        {
            try
            {
                Directory.SetCurrentDirectory(Path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TryWrite()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    writer.WriteLine("Граф:");
                    for (int i = 0; i < graph.AdjacencyMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < graph.AdjacencyMatrix.GetLength(1); j++)
                            writer.Write(graph.AdjacencyMatrix[i, j] + ", ");

                        writer.WriteLine();
                    }
                    writer.WriteLine();
                    
                    writer.WriteLine("Кліка:");
                    for (int i = 0; i < graph.Clique.Length; i++)
                    {
                        writer.Write(graph.Clique[i]);
                        if (i != graph.Clique.Length - 1) 
                            writer.Write(", ");
                    }
                    writer.WriteLine();
                    writer.WriteLine();
                    
                    writer.WriteLine($"Вибраний метод: {graph.ChosenMethod}");
                    writer.WriteLine();
                    
                    if (graph.ShowComplexity)
                    {
                        writer.WriteLine($"Кількість ітерацій: {graph.NumberOfIterations}");
                        writer.WriteLine($"Кількість елементарних операцій: {graph.NumberOfElementaryOperations}");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
