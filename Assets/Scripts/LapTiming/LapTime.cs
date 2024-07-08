using UnityEngine;
using SRS.Utils.Timing;
using System;

namespace Soap.LapTiming
{
	public class LapTime : MonoBehaviour
	{
		public Action<int> OnSectorLogged;
		private Timer timer;

		public float Time
		{
			get
			{
				return timer.CurrentTime;
			}
		}

		public float BestLap {get; private set;}

		public float[] SectorTimes{get; private set;} = new float[3];
		public float[] BestSectors{get; private set;} = new float[3];

		private int sectorIndex;
		private float previousSectorTime;

		private void Awake()
		{
			timer = new Timer();
		}

		private void Start()
		{
			StartLapTimer();
		}

		public void StartLapTimer()
		{
			timer.Reset();
			timer.Start();
			previousSectorTime = 0;
			sectorIndex = 0;
		}

		public void LogSectorTime()
		{
			SectorTimes[sectorIndex] = timer.CurrentTime - previousSectorTime;

			BestSectors[sectorIndex] = Mathf.Min(BestSectors[sectorIndex], SectorTimes[sectorIndex]);

			previousSectorTime = SectorTimes[sectorIndex];

			OnSectorLogged?.Invoke(sectorIndex);

			Debug.Log($"Sector {sectorIndex + 1}: {SectorTimes[sectorIndex]}");

			sectorIndex++;
		}

		public void LogLapTime()
		{
			BestLap = Mathf.Min(BestLap, timer.CurrentTime);
			Debug.Log(timer.CurrentTime);
		}
	}
}