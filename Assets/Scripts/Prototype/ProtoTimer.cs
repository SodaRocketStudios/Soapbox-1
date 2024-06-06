using UnityEngine.Events;

namespace Soap.Prototype
{
	public class ProtoTimer
	{
		public UnityEvent OnStart;
		public UnityEvent<float> OnPause;
		public UnityEvent<float> OnStop;

		private float time;
		public float Time
		{
			get => time;
		}

		private bool isRunning = false;

		public void Update(float deltaTime)
		{
			if(isRunning)
			{
				time += deltaTime;
			}
		}

		public void Start()
		{
			isRunning = true;
			OnStart.Invoke();
		}

		public void Pause()
		{
			isRunning = false;
			OnPause.Invoke(time);
		}

		public void Stop()
		{
			isRunning = false;
			time = 0;
			OnStop.Invoke(time);
		}
	}
}
