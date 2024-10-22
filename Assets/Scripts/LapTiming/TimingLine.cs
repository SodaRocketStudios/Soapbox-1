using System;
using SRS.Utils.DataPersistence;
using Newtonsoft.Json.Linq;
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
			return new TimingData(TimedSegment);
        }

        public void RestoreState(object data)
        {
			float bestTime = (float)(data as JObject)["BestTime"];

			if(bestTime < 0)
			{
				return;
			}

			TimedSegment.SetBestTime(bestTime);
        }
	}
}