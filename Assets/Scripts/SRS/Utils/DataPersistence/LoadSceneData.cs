using UnityEngine;

namespace SRS.Utils.DataPersistence
{
	public class LoadSceneData : MonoBehaviour
	{
		[SerializeField] private string relativeSavePath;
		
		private void OnEnable()
		{
			PersistenceManager.Instance.Load(relativeSavePath);
		}
	}
}