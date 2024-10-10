using UnityEngine;

namespace Soap.GameManagement
{
	public class RaceStateWrapper : MonoBehaviour
	{
		public void ChangeState(State state)
		{
			RaceState.Instance.State = state;
		}
	}
}