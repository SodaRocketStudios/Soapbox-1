using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SRS.SceneManagement
{
	public class SceneController : MonoBehaviour
	{
		public SceneController Instance;

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

		private Dictionary<string, Scene> loadedScenes = new();

		public void LoadScene(Scene scene)
		{
			// Load scene additive
			// Add scene to loaded scenes
			// Disable scene

			SceneManager.LoadSceneAsync(scene.name);
			loadedScenes.Add(scene.name, scene);
		}

		public void ChangeScene(Scene scene)
		{
			// Disable current scene
			// Enable new scene
		}

		public void OpenScene(Scene scene)
		{

		}

		public void CloseScene(Scene scene)
		{

		}

		public void UnloadScene(Scene scene)
		{
			// Remove scene from loaded scenes
			// Unload scene
			// SceneManager.UnloadSceneAsync(scene.name);
		}
	}
}