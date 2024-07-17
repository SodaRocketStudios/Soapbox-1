using System;
using UnityEngine;
using SRS.Utils.Timing;

namespace Soap.LapTiming
{
	public class LapTimer : MonoBehaviour
	{
		public Action<float> OnTimeChanged;
		public Action<int> OnSectorLogged;
		public Action<TimedSegment> onLapLogged;
		public Action<float> OnDeltaUpdate;

		private Timer timer;

		private float sectorStartTime = 0;

		private void Awake()
		{
		}

		private void Start()
		{
			timer.Time.OnChange = OnTimeChanged;
		}

		public void StartTimer()
		{
			timer.Reset();
			timer.Start();
			sectorStartTime = 0;
		}

		public void ResetTimer()
		{
			timer.Reset();
		}

		private void LogLapTime(TimingLine line)
		{
			timer.Pause();
			line.Time.LogTime(timer.CurrentTime);
			onLapLogged?.Invoke(line.Time);
		}

		private void LogSectorTime(TimingLine line)
		{
			line.Time.LogTime(timer.CurrentTime);
			// OnSectorLogged?.Invoke(sectorIndex);
		}

		private void UpdateDelta(TimingLine line)
		{
			OnDeltaUpdate?.Invoke(line.Time.LastTime - line.Time.BestTime);
		}
	}
}