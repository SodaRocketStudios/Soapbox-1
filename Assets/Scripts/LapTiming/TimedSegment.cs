using System;
using UnityEngine;

namespace Soap.LapTiming
{
	public class TimedSegment : MonoBehaviour
	{
		public Action<float> OnNewBest;
		public Action<int> OnTimeLogged; // Passes 1 if a new best was set and -1 otherwise.

		public float BestTime {get; private set;} = -1;
		public float LastTime {get; private set;} = -1;

		public void LogTime(float time)
		{
			LastTime = time;

			if(time < BestTime || BestTime <= 0)
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