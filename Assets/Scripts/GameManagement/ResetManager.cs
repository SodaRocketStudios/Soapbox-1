using System;
using UnityEngine;

namespace Soap.GameManagement
{
	public class ResetManager : MonoBehaviour
	{
		public static Action OnReset;

		public static void Reset()
		{
			StateManager.Instance.State = RaceState.PreStart;
			OnReset?.Invoke();
		}
	}
}