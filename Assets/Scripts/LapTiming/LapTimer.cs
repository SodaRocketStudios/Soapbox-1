using System;
using UnityEngine;
using SRS.Utils.Timing;

namespace Soap.LapTiming
{
	public class LapTimer : MonoBehaviour
	{
		public Action<float> OnTimeChanged;
		public Action<int> OnSectorLogged;

		public TimedSegment Lap {get; private set;}
		public TimedSegment[] Sectors {get; private set;} = new TimedSegment[3];

		private Timer timer;

		private float sectorStartTime = 0;

		private void Awake()
		{
			timer = new();

			for(int i = 0; i < Sectors.Length; i++)
			{
				Sectors[i] = new();
			}

			Lap = new();
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

		public void LogSectorTime(int sector)
		{
			int sectorIndex = sector - 1;
			Sectors[sectorIndex].LogTime(timer.Time.Value - sectorStartTime);
			sectorStartTime += Sectors[sectorIndex].LastTime;
			OnSectorLogged?.Invoke(sectorIndex);
		}

		public void UpdateDelta()
		{
			
		}
	}
}