using UnityEngine;
using UnityEngine.Events;

namespace Soap.GameManagement
{
	public class PauseWrapper : MonoBehaviour
	{
		[SerializeField] private UnityEvent OnPause;
		[SerializeField] private UnityEvent OnResume;

		private void OnEnable()
		{
			PauseManager.Instance.OnPause += InvokePauseEvent;
			PauseManager.Instance.OnResume += InvokeResumeEvent;
		}

		private void OnDisable()
		{
			PauseManager.Instance.OnPause -= InvokePauseEvent;
			PauseManager.Instance.OnResume -= InvokeResumeEvent;
		}

		public void Pause()
		{
			PauseManager.Instance.Pause();
		}

		public void Unpause()
		{
			PauseManager.Instance.Unpause();
		}

		public void TogglePause()
		{
			PauseManager.Instance.TogglePause();
		}

		private void InvokePauseEvent()
		{
			OnPause?.Invoke();
		}

		private void InvokeResumeEvent()
		{
			OnResume?.Invoke();
		}
	}
}