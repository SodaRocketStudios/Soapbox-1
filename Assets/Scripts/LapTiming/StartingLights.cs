using System;
using UnityEngine;

namespace Soap.LapTiming
{
	public class StartingLights : MonoBehaviour
	{
		[SerializeField, Min(0.2f)] private float minDelay = 0.2f;
		[SerializeField, Min(0.2f)] private float maxDelay = 3;

		private Animator animator;

		private System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
		private float randomDelay;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void StartCountdown()
		{
			animator.SetTrigger("StartCountdown");
			randomDelay = random.Next();
		}

		public void Enable()
		{
			animator.SetTrigger("Show");
		}

		public void Disable()
		{
			animator.SetTrigger("Disable");
		}

		public void LightsOff()
		{
			animator.SetTrigger("Off");
		}

		private void TriggerLightsOut()
		{
			Invoke("LightsOff", randomDelay);
		}
	}
}