using System;
using UnityEngine;
using UnityEngine.Events;
using SRS.Extensions.Random;

namespace Soap.LapTiming
{
	public class StartingLights : MonoBehaviour
	{
		public UnityEvent LightsOut;

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
			randomDelay = random.NextFloat(minDelay, maxDelay);
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
			animator.SetTrigger("Disable");
			LightsOut?.Invoke();
		}

		public void TriggerLightsOut()
		{
			Invoke("LightsOff", randomDelay);
		}
	}
}