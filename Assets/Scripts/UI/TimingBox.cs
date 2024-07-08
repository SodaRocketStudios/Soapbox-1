using UnityEngine;
using TMPro;
using Soap.LapTiming;

namespace Soap.UI
{
	public class TimingBox : MonoBehaviour
	{
		[SerializeField] private LapTime lapTimer;

		[SerializeField] private TMP_Text deltaTextBox;
		[SerializeField] private TMP_Text bestTextBox;
		[SerializeField] private TMP_Text currentTextBox;
		[SerializeField] private TMP_Text[] sectorTextBoxes = new TMP_Text[3];

		private void Start()
		{
			lapTimer.OnSectorLogged += UpdateSectorTime;
			lapTimer.OnTimeChange += UpdateLapTime;
		}

		private void UpdateSectorTime(int sectorIndex)
		{
			sectorTextBoxes[sectorIndex].text = $"{lapTimer.SectorTimes[sectorIndex]:F3}";
		}

		private void UpdateLapTime(float time)
		{
			int minutes = (int)time/60;
			float seconds = time - minutes*60;
			currentTextBox.text = $"{minutes}:{seconds:F3}";
		}
	}
}
