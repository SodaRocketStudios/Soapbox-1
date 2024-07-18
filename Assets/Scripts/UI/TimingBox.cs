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

		private int sectorIndex;

		private void Start()
		{
			lapTimer.OnTimeChanged += UpdateCurrentTime;
			lapTimer.OnSectorLogged += UpdateSectorTime;
			lapTimer.onLapLogged += UpdateBestLap;
			lapTimer.OnDeltaUpdate += UpdateDelta;
			lapTimer.OnReset += Initialize;

			Initialize();
		}

		private void Initialize()
		{
			foreach(TMP_Text textBox in SectorTimeTextBoxes)
			{
				textBox.text = "-";
			}
			sectorIndex = 0;
		}

		private void UpdateCurrentTime(float time)
		{
			int minutes = (int)time/60;
			float seconds = time - minutes*60;
			lapTimeTextBox.text = $"{minutes}:{seconds:00.000}";
		}

		private void UpdateSectorTime(float time)
		{
			SectorTimeTextBoxes[sectorIndex].text = $"{time:00.000}";
			sectorIndex++;
		}

		private void UpdateBestLap(TimedSegment time)
		{
			int minutes = (int)time.BestTime/60;
			float seconds = time.BestTime - minutes*60;
			BestLapTextBox.text = $"{minutes}:{seconds:00.000}";
		}

		private void UpdateDelta(float delta)
		{
			DeltaTextBox.text = $"{delta:F3}";
		}
	}
}