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
        public Graph Graph { get; set; }

        public WriteToFile(string path, string fileName, Graph graph)
        {
            Path = path;
            FileName = fileName + ".txt";
            this.Graph = graph;
        }

        public void Run(Window mainWindow)
        {
            if (!TrySetDirectory())
            {
                new ErrorWindow(mainWindow,
                    $"Не знайдено директорію з таким іменем \"{Path}\"").ShowDialog();
                return;
            }

            if (!Write())
            {
                new ErrorWindow(mainWindow,
                    $"Не вдалося записати до файлу \"{FileName}\"").ShowDialog();
                return;
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

        private bool Write()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    writer.WriteLine("Граф:");
                    for (int i = 0; i < Graph.AdjacencyMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < Graph.AdjacencyMatrix.GetLength(1); j++)
                            writer.Write(Graph.AdjacencyMatrix[i, j] + ", ");

                        writer.WriteLine();
                    }
                    writer.WriteLine();
                    
                    writer.WriteLine("Кліка:");
                    for (int i = 0; i < Graph.Clique.Length; i++)
                    {
                        writer.Write(Graph.Clique[i]);
                        if (i != Graph.Clique.Length - 1) 
                            writer.Write(", ");
                    }

                    writer.WriteLine();
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
