using UnityEngine;
using SRS.Utils.DataHandling;
using System.Collections.Generic;

namespace SRS.Utils.DataPersistence
{
	public class PersistenceManager : MonoBehaviour
	{
		[SerializeField] private bool encryptData;

		private ISerializer serializer = new JsonSerializer();

		private IDataHandler dataHandler = new FileDataHandler();

		[ContextMenu("Save")]
		public void Save()
		{
			Dictionary<string, object> state = new();
			CaptureState(state);
			
			string data = serializer.Serialize(state);

			if(encryptData)
			{
				data = Encryptor.Encrypt(data);
			}

			dataHandler.Write("test.sav", data);
		}

		[ContextMenu("Load")]
		public void Load()
		{
			string data = dataHandler.Read("test.sav");

			if(string.IsNullOrEmpty(data))
			{
				return;
			}

			if(encryptData)
			{
				data = Encryptor.Encrypt(data);
			}

			Dictionary<string, object> state = serializer.Deserialize(data).ToDictionary();

			RestoreState(state);
		}

		private void CaptureState(Dictionary<string, object> state)
		{
			foreach(PersistentEntity entity in FindObjectsByType<PersistentEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				state[entity.UniqueIdentifier] = entity.CaptureState();
			}
		}

		private void RestoreState(Dictionary<string, object> state)
		{			
			if(state == null)
			{
				return;
			}

			foreach(PersistentEntity entity in FindObjectsByType<PersistentEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if(state.ContainsKey(entity.UniqueIdentifier))
				{
					entity.RestoreState(state[entity.UniqueIdentifier]);
				}
			}
		}
	}
}