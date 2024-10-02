using System;

namespace Soap.LapTiming
{
    [Serializable]
	public class TimingData
	{
		public float BestTime;

		public TimingData(TimedSegment segment)
		{
			BestTime = segment.BestTime;
		}
	}
}