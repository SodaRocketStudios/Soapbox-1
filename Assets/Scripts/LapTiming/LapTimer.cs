using System;
using UnityEngine;
using SRS.Utils.Timing;

namespace Soap.LapTiming
{
	public class LapTimer : MonoBehaviour
	{
		public Action<float> OnTimeChanged;

		public TimedSegment Lap {get; private set;}
		public TimedSegment[] Sectors {get; private set;} = new TimedSegment[3];

		private Timer timer;

		private float sectorStartTime = 0;

		private void Awake()
		{
			timer = new();
		}

		private void Start()
		{
			timer.Time.OnChange = OnTimeChanged;
			StartTimer();
		}

		public void StartTimer()
		{
			timer.Reset();
			timer.Start();
			sectorStartTime = 0;
		}

		public void LogLapTime()
		{
			timer.Pause();
			Lap.LogTime(timer.Time);
		}

		public void LogSectorTime(int sectorIndex)
		{
			Sectors[sectorIndex - 1].LogTime(timer.Time - sectorStartTime);
			sectorStartTime = Sectors[sectorIndex - 1].LastTime;
		}

		public void UpdateDelta()
		{
			
		}
	}
}