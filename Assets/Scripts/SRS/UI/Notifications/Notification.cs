using System.Collections;
using UnityEngine;

namespace SRS.UI.Notifications
{
	public class Notification : MonoBehaviour
	{
		[SerializeField] private PanelTransition entryTransition;
		[SerializeField] private PanelTransition exitTransition;

		[SerializeField, Min(0)] private float displayTime;

		private void Awake()
		{
			entryTransition.OnTransitionEnd += StartDelay;
		}

		public void Show()
		{
			StartCoroutine(entryTransition.Animate(gameObject));	
		}

		public void Hide()
		{
			StartCoroutine(exitTransition.Animate(gameObject));
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