using System;
using UnityEngine;

namespace Soap.LapTiming
{
	public class TimedSegment : MonoBehaviour
	{
		public Action<float> OnNewBest; //TODO -- Do I need to notify every time a time is recorded to set colors on UI?
		public Action<int> OnTimeLogged; // Passes 1 if a new best was set and -1 otherwise.

		public float BestTime {get; private set;} = -1;
		public float LastTime {get; private set;} = -1;

		public void LogTime(float time)
		{
			LastTime = time;

			if(BestTime <= 0 || time < BestTime)
			{
				BestTime = time;
				OnNewBest?.Invoke(BestTime);
				OnTimeLogged?.Invoke(1);
				return;
			}

			OnTimeLogged?.Invoke(-1);
		}
	}
}
