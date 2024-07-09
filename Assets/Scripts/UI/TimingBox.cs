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
		[SerializeField] private TMP_Text[] SectorTimeTextBoxes = new TMP_Text[3];

		private void Start()
		{
			lapTimer.OnTimeChanged += UpdateCurrentTime;
			lapTimer.OnSectorLogged += UpdateSectorTime;
			lapTimer.Lap.OnNewBest += UpdateBestLap;
		}

		private void UpdateCurrentTime(float time)
		{
			int minutes = (int)time/60;
			float seconds = time - minutes*60;
			lapTimeTextBox.text = $"{minutes}:{seconds:00.000}";
		}

		private void UpdateSectorTime(int sectorIndex)
		{
			SectorTimeTextBoxes[sectorIndex].text = $"{lapTimer.Sectors[sectorIndex].LastTime:00.000}";
		}

		private void UpdateBestLap(float time)
		{
			int minutes = (int)time/60;
			float seconds = time - minutes*60;
			BestLapTextBox.text = $"{minutes}:{seconds:00.000}";
		}
	}
}