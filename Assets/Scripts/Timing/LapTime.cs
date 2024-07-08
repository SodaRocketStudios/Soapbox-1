using UnityEngine;
using SRS.Utils.Timing;
using SRS.Utils.Observables;

namespace Soap.LapTiming
{
	public class LapTime : MonoBehaviour
	{
		private Timer timer;

		private float bestLap;
		private float[] bestSectors = new float[3];

		private int sectorIndex;
		private float lastSectorTime;

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
			timer.Start();
			sectorIndex = 0;
		}

		public void LogSectorTime()
		{
			float sectorTime = timer.Time.Value - lastSectorTime;

			bestSectors[sectorIndex] = Mathf.Min(bestSectors[sectorIndex], sectorTime);

			lastSectorTime = sectorTime;

			Debug.Log($"Sector {sectorIndex + 1}: {sectorTime}");

			sectorIndex++;
		}

		public void LogLapTime()
		{
			bestLap = Mathf.Min(bestLap, timer.Time.Value);
			Debug.Log(timer.Time.Value);
		}
	}
}