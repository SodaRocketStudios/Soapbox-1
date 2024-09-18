using System;
using System.Collections;
using UnityEngine;

namespace SRS.UI.Notifications
{
	public class Notification : MonoBehaviour
	{
		[SerializeField] private PanelTransition entryTransition;
		[SerializeField] private PanelTransition exitTransition;

		[SerializeField, Min(0)] private float displayTime;

		public Action OnBeforeNotify;
		public Action OnAfterNotify;

		private void OnEnable()
		{
			entryTransition.OnTransitionEnd += StartDelay;
			exitTransition.OnTransitionEnd += InvokeAfterNotify;
		}

		private void OnDisable()
		{
			entryTransition.OnTransitionEnd -= StartDelay;
			exitTransition.OnTransitionEnd -= InvokeAfterNotify;
		}

		public void Show()
		{
			OnBeforeNotify?.Invoke();
			StartCoroutine(entryTransition.Animate(gameObject));
		}

		public void Hide()
		{
			StartCoroutine(exitTransition.Animate(gameObject));
		}

		private void InvokeAfterNotify()
		{
			OnAfterNotify?.Invoke();
		}

		private void StartDelay()
		{
			StartCoroutine(Delay());
		}

		private IEnumerator Delay()
		{
			yield return new WaitForSeconds(displayTime);

			Hide();
		}
	}
}