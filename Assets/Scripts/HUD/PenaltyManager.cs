using System.Collections.Generic;
using UnityEngine;

namespace Soap.HUD
{
	public class PenaltyManager : MonoBehaviour
	{
		public static PenaltyManager Instance;

		[SerializeField] private Penalty penaltyDisplay;

		private Queue<float> penaltyQueue = new();

		private bool isShowing = false;

		public void ShowPenalty(float penaltyTime)
		{
			if(isShowing)
			{
				penaltyQueue.Enqueue(penaltyTime);
				return;
			}

			penaltyDisplay.Show(penaltyTime);

			isShowing = true;
		}

		public void ShowNext()
		{
			if(penaltyQueue.Count > 0)
			{
				isShowing = false;
				ShowPenalty(penaltyQueue.Dequeue());
				return;
			}

			isShowing = false;
		}

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}