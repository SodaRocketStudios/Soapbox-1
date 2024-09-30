using UnityEngine;
using TMPro;
using System.Text;

namespace SRS.UI
{
	public class TimeDisplay : MonoBehaviour
	{
		const float HOURS_TO_SECONDS = 3600;
		const float SECONDS_TO_HOURS = 1/HOURS_TO_SECONDS;
		const float MINUTES_TO_SECONDS = 60;
		const float SECONDS_TO_MINUTES = 1/MINUTES_TO_SECONDS;

		[SerializeField, Min(0)] private int integerPlaces;
		[SerializeField, Min(0)] private int decimalPlaces;

		private string formatSpecifier;

		[SerializeField] private bool alwaysShowMinutes;
		[SerializeField] private bool alwaysShowHours;

		private TMP_Text textBox;

		private void Awake()
		{
			textBox = GetComponent<TMP_Text>();

			StringBuilder stringBuilder = new();

			for (int i = 0; i < integerPlaces; i++)
			{
				stringBuilder.Append("0");
			}

			if(decimalPlaces > 0)
			{
				stringBuilder.Append(".");
			}

			for (int i = 0; i < decimalPlaces; i++)
			{
				stringBuilder.Append("0");
			}

			formatSpecifier = stringBuilder.ToString();
		}

		public void SetValue(float time)
		{
			int hours = (int)(time*SECONDS_TO_HOURS);
			time -= hours*HOURS_TO_SECONDS;
			int minutes = (int)(time*SECONDS_TO_MINUTES);
			time -= minutes*MINUTES_TO_SECONDS;

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

			stringBuilder.Append(time.ToString(formatSpecifier));
			textBox.text = stringBuilder.ToString();
		}
	}
}