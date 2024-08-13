using UnityEngine;
using UnityEngine.UI;

namespace SRS.UI
{
	public class ProgressBar : MonoBehaviour
	{
		private Slider slider;

		private void Awake()
		{
			slider = GetComponent<Slider>();
		}

		public void SetPercentage(float percentage)
		{
			slider.value = percentage;
		}
	}
}
