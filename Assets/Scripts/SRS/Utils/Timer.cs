using UnityEngine.Events;

namespace SRS.Utils
{
	public class Timer
	{
		public UnityEvent OnStart;
		public UnityEvent<float> OnPause;
		public UnityEvent<float> OnStop;

		public float Time{get; private set;}

		public bool IsRunning{get; private set;}

		public Timer()
		{
			IsRunning = false;
			Time = 0;
			Updater.AddUpdateCallback(Update);
		}

		public void Update(float deltaTime)
		{
			if(IsRunning)
			{
				Time += deltaTime;
			}
		}

		public void Start()
		{
			IsRunning = true;
			OnStart.Invoke();
		}

		public void Pause()
		{
			IsRunning = false;
			OnPause.Invoke(Time);
		}

		public void Stop()
		{
			IsRunning = false;
			Time = 0;
			OnStop.Invoke(Time);
		}
	}
}
