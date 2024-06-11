using System;
using SRS.Utils.Observables;

namespace SRS.Utils.Timing
{
	public class Timer
	{
		public Action OnStart;
		public Action<float> OnPause;
		public Action<float> OnStop;

		public ObservableValue<float> Time{get; private set;}

		public bool IsRunning{get; private set;}

		public Timer()
		{
			IsRunning = false;
			Time = new();
			Reset();
			Updater.AddUpdateCallback(Update);
		}

		public void Start()
		{
			IsRunning = true;
			OnStart?.Invoke();
		}

		public void Pause()
		{
			IsRunning = false;
			OnPause?.Invoke(Time);
		}

		public void Stop()
		{
			IsRunning = false;
			Reset();
			OnStop?.Invoke(Time);
		}

		public void Reset()
		{
			Time.SetValue(0);
		}

		private void Update(float deltaTime)
		{
			if(IsRunning)
			{
				Time += deltaTime;
			}
		}
	}
}