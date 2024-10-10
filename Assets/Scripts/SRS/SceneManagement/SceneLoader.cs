using UnityEngine;

namespace SRS.SceneManagement
{
	public class SceneLoader : MonoBehaviour
	{
		public void LoadScene(string sceneName)
		{
			SceneController.Instance.LoadScene(sceneName);
		}

		public void UnloadScene(string sceneName)
		{
			SceneController.Instance.UnloadScene(sceneName);
		}
	}
}