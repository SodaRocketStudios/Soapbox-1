using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SRS.SceneManagement
{
	public class SceneController : MonoBehaviour
	{
		public static SceneController Instance;

		[SerializeField] private List<string> initialScenes;

		List<string> loadedScenes = new();

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

		private void Start()
		{
			foreach(string scene in initialScenes)
			{
				LoadScene(scene);
			}
		}

		public void LoadScene(string scene)
		{
			if(loadedScenes.Contains(scene))
			{
				Debug.Log("'Scene already loaded");
				return;
			}

			SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			loadedScenes.Add(scene);
		}

		public void UnloadScene(string scene)
		{
			if(loadedScenes.Contains(scene) == false)
			{
				Debug.Log("'Scene not loaded");
				return;
			}

			SceneManager.UnloadSceneAsync(scene);
			loadedScenes.Remove(scene);
		}
	}
}