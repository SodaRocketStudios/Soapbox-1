using UnityEngine;
using UnityEngine.Events;

namespace Soap.LapTiming
{
	public class TimingLine : MonoBehaviour
	{
		public UnityEvent OnEnter;

		private void OnTriggerEnter(Collider other)
		{
			OnEnter.Invoke();
		}
	}
}