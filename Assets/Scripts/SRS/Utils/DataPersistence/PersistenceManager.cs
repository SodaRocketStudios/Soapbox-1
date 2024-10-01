using System.Linq;
using System.Text;
using UnityEngine;
using SRS.Utils.DataHandling;

namespace SRS.Utils.DataPersistence
{
	public class PersistenceManager : MonoBehaviour
	{
		[SerializeField] private bool encryptData;

		private ISerializer serializer = new JsonSerializer();

		private IDataWriter dataWriter = new FileWriter();

		[ContextMenu("Save")]
		public void Save()
		{
			var persistentObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPersist>();

			StringBuilder dataStringBuilder = new();

			string dataString;

			foreach (MonoBehaviour persistentObject in persistentObjects)
			{
				dataStringBuilder.Append(serializer.Serialize((persistentObject as IPersist).Save()));
			}

			dataString = dataStringBuilder.ToString();

			if(encryptData)
			{
				dataString = Encryptor.Encrypt(dataString);
			}

			dataWriter.Write("test.save", dataString);
		}
	}
}