using System;
using UnityEngine.Events;
using SRS.Utils.DataPersistence;

namespace Soap.LapTiming
{
	[Serializable]
	public class TimedSegment : IPersist
	{
		public UnityEvent<float> OnSectorTimeLogged;
		public UnityEvent<float> OnNewBestSector;

		public float BestTime {get; private set;} = -1;
		public float LastTime {get; private set;} = -1;

		public float TotalTime {get => LastTime + startTime;}

		private float startTime = 0;

		private int id;

        public int ID => id;

		public void StartTiming(float time)
		{
			startTime = time;
		}

        public void LogSectorTime(float time)
		{
			time -= startTime;

			LastTime = time;

			OnSectorTimeLogged?.Invoke(time);

			if(time < BestTime || BestTime <= 0)
			{
				SetBestTime(time);
				return;
			}
		}

        private void SetBestTime(float time)
		{
			BestTime = time;
			OnNewBestSector?.Invoke(BestTime);
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
	
	[Serializable]
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