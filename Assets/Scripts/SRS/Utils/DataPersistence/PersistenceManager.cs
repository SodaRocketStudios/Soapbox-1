using UnityEngine;
using SRS.Utils.DataHandling;
using System.Collections.Generic;

namespace SRS.Utils.DataPersistence
{
	public class PersistenceManager : MonoBehaviour
	{
		public static PersistenceManager Instance;

		[SerializeField] private bool encryptData;

		private ISerializer serializer = new JsonSerializer();

		private IDataHandler dataHandler = new FileDataHandler();

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
			}
		}

		public void Save(string relativePath)
		{
			Dictionary<string, object> state = new();
			CaptureState(state);
			
			string data = serializer.Serialize(state);

			if(encryptData)
			{
				data = Encryptor.Encrypt(data);
			}

			dataHandler.Write(relativePath, data);
		}

		public void Load(string relativePath)
		{
			string data = dataHandler.Read(relativePath);

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