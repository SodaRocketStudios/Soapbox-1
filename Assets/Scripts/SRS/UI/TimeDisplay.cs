using UnityEngine;
using TMPro;
using System.Text;

namespace SRS.UI
{
	public class TimeDisplay : MonoBehaviour
	{
		[SerializeField] private string preText;
		[SerializeField] private string postText;
		[SerializeField] private int decimalPlaces;

		[SerializeField] private bool alwaysShowMinutes;
		[SerializeField] private bool alwaysShowHours;

		private TMP_Text textBox;

		private void Awake()
		{
			textBox = GetComponent<TMP_Text>();
		}

		public void SetValue(float time)
		{
			int hours = (int)time/60;
			time -= hours*60;
			int minutes = (int)time/60;
			time -= minutes*60;

			StringBuilder stringBuilder = new StringBuilder();

			bool showHours = hours > 0 || alwaysShowHours;

			if(showHours )
			{
				stringBuilder.Append(hours.ToString());
				stringBuilder.Append(":");
			}

			bool showMinutes = hours > 0 || minutes > 0 || alwaysShowMinutes || alwaysShowHours;

			if(showMinutes)
			{
				stringBuilder.Append(minutes.ToString());
				stringBuilder.Append(":");
			}

			stringBuilder.Append(time.ToString($"F{decimalPlaces}"));
			textBox.text = stringBuilder.ToString();
		}
	}
}