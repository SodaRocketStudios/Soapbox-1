using UnityEngine;

namespace Soap.GameManagement
{
    public class StateManager : MonoBehaviour
	{
		public static StateManager Instance;

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(this);
			}
		}

		public RaceState State{get; set;}
	}
}