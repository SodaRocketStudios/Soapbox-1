using UnityEngine;

namespace Soap.GameManagement
{
	public class RaceStateWrapper : MonoBehaviour
	{
		public void SetPreStartState()
		{
			StateManager.Instance.State = RaceState.PreStart;
		}

		public void SetCountdownState()
		{
			StateManager.Instance.State = RaceState.Countdown;
		}

		public void SetRunningState()
		{
			StateManager.Instance.State = RaceState.Running;
		}
	}
}