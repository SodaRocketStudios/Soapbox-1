using System;

namespace Soap.LapTiming
{
	public class TimedSegment
	{
		public Action<TimedSegment> OnTimeLogged;

		public float BestTime {get; private set;} = -1;
		public float LastTime {get; private set;} = -1;

		public void LogTime(float time)
		{
			LastTime = time;

			OnTimeLogged?.Invoke(this);

			if(time < BestTime || BestTime <= 0)
			{
				BestTime = time;
				return;
			}
		}
	}
}