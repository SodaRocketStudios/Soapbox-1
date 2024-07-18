using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soap.LapTiming
{
	public class TimingLine : MonoBehaviour
	{
		public UnityEvent<TimingLine> OnTrigger;
		public Action<TimingLine> OnTriggerAction;

		public TimedSegment Time = new();

		private void OnTriggerEnter(Collider other)
		{
			OnTrigger?.Invoke(this);
			OnTriggerAction?.Invoke(this);
		}
	}
}