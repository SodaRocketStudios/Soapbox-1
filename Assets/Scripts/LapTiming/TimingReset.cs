using UnityEngine;
using Soap.GameManagement;

namespace Soap.LapTiming
{
	public class TimingReset : MonoBehaviour
	{
		[SerializeField] private LapTimer timer;

		[SerializeField] private StartingLights startLights;

		private void OnEnable()
		{
			ResetManager.OnReset += Reset;
		}

		private void OnDisable()
		{
			ResetManager.OnReset -= Reset;
		}

		public void Reset()
		{
			timer.Reset();

			startLights.Disable();
		}
	}
}