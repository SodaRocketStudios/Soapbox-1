using UnityEngine;
using SRS.Utils.Timing;

namespace Soap.LapTiming
{
	public class LapTime : MonoBehaviour
	{
		private Timer timer;

		private float bestLap;
		private float[] bestSectors = new float[3];

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
			float sectorTime = timer.CurrentTime - previousSectorTime;

			bestSectors[sectorIndex] = Mathf.Min(bestSectors[sectorIndex], sectorTime);

			previousSectorTime = sectorTime;

			Debug.Log($"Sector {sectorIndex + 1}: {sectorTime}");

			sectorIndex++;
		}

		public void LogLapTime()
		{
			bestLap = Mathf.Min(bestLap, timer.CurrentTime);
			Debug.Log(timer.CurrentTime);
		}
	}
}