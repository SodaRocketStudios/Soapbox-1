using System.IO;

namespace SRS.DataPersistence
{
    public class FileWriter : IDataWriter
    {
        public string Read(string relativePath)
        {
            using(StreamReader reader = File.OpenText(relativePath))
			{
				return reader.ReadToEnd();
			}
        }

        public void Write(string relativePath, string data)
        {
            using(StreamWriter writer = new StreamWriter(relativePath, false))
			{
				writer.Write(data);
			}
        }
    }
}