using UnityEngine;

namespace Soap.GameManagement
{
	public class GameState : MonoBehaviour
	{
		public static GameState Instance;

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

		public State State{get; set;}
	}

	public enum State
	{
		PreStart,
		Countdown,
		Running
	}
}