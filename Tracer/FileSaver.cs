using System.IO;

namespace Tracer
{
    public class FileSaver : IPrinter
    {
        public void Print(string serializedResult)
        {
            File.WriteAllText(PathToSave, serializedResult);
        }

        private string PathToSave { get; set; }

        public FileSaver(string path)
        {
            PathToSave = path;
        }
    }
}