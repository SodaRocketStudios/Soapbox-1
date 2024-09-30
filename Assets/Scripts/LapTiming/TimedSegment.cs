using System;
using SRS.Utils.DataPersistence;

namespace Soap.LapTiming
{
	[Serializable]
	public class TimedSegment : IPersist
	{
		public Action<float> OnNewBest;

		public float BestTime {get; private set;} = -1;
		public float LastTime {get; private set;} = -1;

		private int id;

        public int ID => id;

        public void LogTime(float time)
		{
			LastTime = time;

			if(time < BestTime || BestTime <= 0)
			{
				SetBestTime(time);
				return;
			}
		}


        private void SetBestTime(float time)
		{
			BestTime = time;
			OnNewBest?.Invoke(BestTime);
		}

        public void Load(object data)
        {
			SetBestTime((data as TimedSegmentData).BestTime);
        }

        public object Save()
        {
            return new TimedSegmentData(this);
        }

	}
	
	[System.Serializable]
	public class TimedSegmentData
	{
		public int ID;
		public float BestTime;

		public TimedSegmentData(TimedSegment segment)
		{
			this.ID = segment.ID;
			this.BestTime = segment.BestTime;
		}
	}
}