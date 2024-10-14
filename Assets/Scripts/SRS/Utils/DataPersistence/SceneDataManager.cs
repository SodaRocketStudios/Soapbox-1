using UnityEngine;

namespace SRS.Utils.DataPersistence
{
	public class SceneDataManager : MonoBehaviour
	{
		[SerializeField] private string relativeSavePath;
		
		private void OnEnable()
		{
			Load();
		}

		public void Load()
		{
			PersistenceManager.Instance.Load(relativeSavePath);
		}

		public void Save()
		{
			PersistenceManager.Instance.Save(relativeSavePath);
		}
	}
}