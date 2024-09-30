using UnityEngine;
using UnityEngine.Events;
using SRS.Utils.Timing;

namespace Soap.LapTiming
{
	public class LapTimer : MonoBehaviour
	{
		public UnityEvent<float> OnLapLogged;

		public UnityEvent<float> OnTimeChanged;

		private Timer timer;

		public TimedSegment lapTime;

		private float penaltyTime = 0;

		private void Awake()
		{
			timer = new();
		}

		private void OnEnable()
		{
			timer.Time.OnChange += UpdateTime;
		}

		private void OnDisable()
		{
			timer.Time.OnChange -= UpdateTime;
		}

		private void Start()
		{
			foreach(DeltaCheckpoint checkpoint in FindObjectsByType<DeltaCheckpoint>(FindObjectsSortMode.None))
			{
				checkpoint.OnDeltaLogged += UpdateDelta;
			}
		}

		public void StartTimer()
		{
			timer.Start();
		}

		public void Reset()
		{
			timer.Reset();
			penaltyTime = 0;
		}

		public void LogLapTime(TimingLine line)
		{
			timer.Pause();
			timer.Time.Value += penaltyTime;
			lapTime.LogSectorTime(timer.CurrentTime);
			OnLapLogged?.Invoke(timer.CurrentTime);
		}

		public void StartSegment(TimingLine line)
		{
			line.TimedSegment.StartTiming(timer.CurrentTime);
		}

		public void LogSegment(TimingLine line)
		{
			line.TimedSegment.LogSectorTime(timer.CurrentTime);
		}

		public void UpdateDelta(TimingLine checkpoint)
		{
			bool firstTime = checkpoint.TimedSegment.BestTime < 0 ? true:false;

			float delta = timer.CurrentTime - checkpoint.TimedSegment.BestTime;

			checkpoint.TimedSegment.LogSectorTime(timer.CurrentTime);

			if(firstTime)
			{
				return;
			}
		}

		public void AddPenalty(float penaltyTime)
		{
			this.penaltyTime += penaltyTime;
		}

		private void UpdateTime(float time)
		{
			OnTimeChanged?.Invoke(time);
		}
	}
}