namespace Soap.LapTiming
{
	public class TimedSegment
	{
		public float BestTime {get; private set;} = -1;
		public float LastTime {get; private set;} = -1;

		public void LogTime(float time)
		{
			LastTime = time;

			if(time < BestTime || BestTime <= 0)
			{
				BestTime = time;
				return;
			}
		}

		public void SetBestTime(float time)
		{
			BestTime = time;
		}
	}
}