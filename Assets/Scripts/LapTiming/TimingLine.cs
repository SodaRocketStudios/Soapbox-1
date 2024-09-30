using UnityEngine;
using UnityEngine.Events;

namespace Soap.LapTiming
{
	public class TimingLine : MonoBehaviour
	{
		public UnityEvent<TimingLine> OnTrigger;

		public TimedSegment TimedSegment = new();

		private void OnTriggerEnter(Collider other)
		{
			OnTrigger?.Invoke(this);
		}
	}
}