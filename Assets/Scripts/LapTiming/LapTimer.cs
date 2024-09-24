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
		public Action OnReset;

		private Timer timer;

		private float sectorStartTime = 0;

		private float penaltyTime = 0;

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
			timer.Start();
			sectorStartTime = 0;
		}

		public void Reset()
		{
			timer.Reset();
			OnReset?.Invoke();
			penaltyTime = 0;
		}

		public void LogLapTime(TimingLine line)
		{
			timer.Pause();
			line.Time.LogTime(timer.CurrentTime + penaltyTime);
			onLapLogged?.Invoke(line.Time);
			LogSectorTime(line);
			timer.Time.Value += penaltyTime;
		}

		public void LogSectorTime(TimingLine line)
		{
			line.Time.LogTime(timer.CurrentTime);
			OnSectorLogged?.Invoke(timer.CurrentTime - sectorStartTime);
			sectorStartTime = timer.CurrentTime;
		}

		public void UpdateDelta(TimingLine checkpoint)
		{
			bool firstTime = checkpoint.Time.BestTime < 0 ? true:false;

			float delta = timer.CurrentTime - checkpoint.Time.BestTime;

			checkpoint.Time.LogTime(timer.CurrentTime);

			if(firstTime)
			{
				return;
			}

			OnDeltaUpdate?.Invoke(delta);
		}

		public void AddPenalty(float penaltyTime)
		{
			this.penaltyTime += penaltyTime;
		}
	}
}