using System;
using UnityEngine;
using SRS.Utils.Timing;

namespace Soap.LapTiming
{
	public class LapTimer : MonoBehaviour
	{
		public Action<float> OnTimeChanged;
		public Action<float> OnSectorLogged;
		public Action<TimedSegment> onLapLogged;
		public Action<float> OnDeltaUpdate;

		private Timer timer;

		private float sectorStartTime = 0;

		private void Awake()
		{
			timer = new();
		}

		private void Start()
		{
			timer.Time.OnChange = OnTimeChanged;

			foreach(DeltaCheckpoint checkpoint in FindObjectsByType<DeltaCheckpoint>(FindObjectsSortMode.None))
			{
				checkpoint.OnDeltaLogged += UpdateDelta;
			}
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

		public void LogLapTime(TimingLine line)
		{
			timer.Pause();
			line.Time.LogTime(timer.CurrentTime);
			onLapLogged?.Invoke(line.Time);
		}

		public void LogSectorTime(TimingLine line)
		{
			line.Time.LogTime(timer.CurrentTime);
			OnSectorLogged?.Invoke(timer.CurrentTime - sectorStartTime);
			sectorStartTime = timer.CurrentTime;
		}

		public void UpdateDelta(TimingLine checkpoint)
		{
			checkpoint.Time.LogTime(timer.CurrentTime);

			if(checkpoint.Time.BestTime < 0)
			{
				return;
			}

			OnDeltaUpdate?.Invoke(checkpoint.Time.LastTime - checkpoint.Time.BestTime);
		}
	}
}