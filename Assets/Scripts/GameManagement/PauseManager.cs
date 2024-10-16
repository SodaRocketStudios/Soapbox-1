using UnityEngine;
using Soap.Input;
using System;

namespace Soap.GameManagement
{
	public class PauseManager : MonoBehaviour
	{
		public static PauseManager Instance;

		public Action OnPause;
		public Action OnResume;

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
			OnPause?.Invoke();
			IsPaused = true;
		}

		public void Unpause()
		{
			InputHandler.Instance.SetGameplayInput();
			Time.timeScale = 1;
			OnResume?.Invoke();
			IsPaused = false;
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
		}
	}
}