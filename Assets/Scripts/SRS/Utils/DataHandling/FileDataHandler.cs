using System.IO;
using UnityEngine;

namespace SRS.Utils.DataHandling
{
    public class FileDataHandler : IDataHandler
    {
        public string Read(string relativePath)
        {
            string combinedPath = Path.Combine(Application.persistentDataPath, relativePath);

            using(StreamReader reader = File.OpenText(combinedPath))
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