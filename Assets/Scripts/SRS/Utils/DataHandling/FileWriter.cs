using System.IO;
using UnityEngine;

namespace SRS.Utils.DataHandling
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
            string combinedPath = Path.Combine(Application.persistentDataPath, relativePath);
            using(StreamWriter writer = new StreamWriter(combinedPath, false))
			{
				writer.Write(data);
			}
        }
    }
}