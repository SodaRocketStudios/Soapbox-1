using UnityEngine;
using Soap.Input;

namespace Soap.GameManagement
{
	public class PauseManager : MonoBehaviour
	{
		public static PauseManager Instance;

		private bool IsPaused = false;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
			}
		}

		private void OnEnable()
		{
			InputHandler.OnPauseInput += TogglePause;
		}
		
		private void OnDisable()
		{
			InputHandler.OnPauseInput -= TogglePause;
		}

		public void Pause()
		{
			InputHandler.Instance.SetUIInput();
			Time.timeScale = 0;
			Debug.Log("Pause");
		}

		public void Unpause()
		{
			InputHandler.Instance.SetGameplayInput();
			Time.timeScale = 1;
			Debug.Log("Unpause");
		}

		public void TogglePause()
		{
			if(IsPaused)
			{
				Unpause();
			}
			else
			{
				Pause();
			}

			IsPaused = !IsPaused;
		}
	}
}