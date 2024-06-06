using UnityEngine;
using UnityEngine.Events;

namespace Soap.Prototype
{
	[RequireComponent(typeof(BoxCollider))]
	public class ProtoTimingLine : MonoBehaviour
	{
		public UnityEvent<float> OnEnter;

		private float time;
		private float recordTime = -1;

		private void OnTriggerEnter(Collider other)
		{
			// TODO -- Get time from timer
			time = 1;
			
			if(recordTime < 0)
			{
				recordTime = time;
			}

			else if(time < recordTime)
			{
				recordTime = time;
			}

			Debug.Log("Timing Line");

			OnEnter.Invoke(time);
		}
	}
}
