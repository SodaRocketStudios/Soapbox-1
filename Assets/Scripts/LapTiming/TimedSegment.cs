using System;
using UnityEngine.Events;

namespace Soap.LapTiming
{
	[Serializable]
	public class TimedSegment
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

        public void SetBestTime(float time)
		{
			BestTime = time;
			OnNewBestSector?.Invoke(BestTime);
		}
	}
}