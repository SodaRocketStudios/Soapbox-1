using System;
using SRS.Utils.DataPersistence;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Soap.LapTiming
{
	[Serializable]
	public class TimingLine : MonoBehaviour, IPersist
	{
		public UnityEvent<TimingLine> OnTrigger;

		public TimedSegment TimedSegment = new();

        private void OnTriggerEnter(Collider other)
		{
			OnTrigger?.Invoke(this);
		}

        public object CaptureState()
        {
			return null;
        }

        public int RestoreState(object data)
        {
          return -1;
        }

	}
}