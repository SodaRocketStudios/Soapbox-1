using UnityEngine;

namespace Soap.Physics
{
	public class DRSManager : MonoBehaviour
	{
		public DRSState state {get; private set;}

		public void OnZoneEnter()
		{
			state = DRSState.Available;
		}

		public void OnZoneExit()
		{
			state = DRSState.Disabled;
		}
	}

	public enum DRSState
	{
		Disabled,
		Available,
		Enabled
	}
}
