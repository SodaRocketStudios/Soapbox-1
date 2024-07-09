using UnityEngine;
using TMPro;
using Soap.LapTiming;

namespace Soap.UI
{
	public class TimingBox : MonoBehaviour
	{
		[SerializeField] private LapTimer lapTimer;
		[SerializeField] private TMP_Text lapTimeTextBox;
		[SerializeField] private TMP_Text BestLapTextBox;
		[SerializeField] private TMP_Text DeltaTextBox;
		[SerializeField] private TMP_Text Sector1TextBox;
		[SerializeField] private TMP_Text Sector2TextBox;
		[SerializeField] private TMP_Text Sector3TextBox;

		private void Start()
		{
			lapTimer.OnTimeChanged += UpdateCurrentTime;
		}

		private void UpdateCurrentTime(float time)
		{
			int minutes = (int)time/60;
			float seconds = time - minutes*60;
			lapTimeTextBox.text = $"{minutes}:{seconds:00.000}";
		}
	}
}
