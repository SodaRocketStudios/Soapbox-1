using UnityEngine;
using TMPro;

namespace SRS.UI
{
	public class NumberDisplay : MonoBehaviour
	{
		[SerializeField] private string preText;
		[SerializeField] private string postText;
		[SerializeField] private int decimalPlaces;

		private TMP_Text textBox;

		private void Awake()
		{
			textBox = GetComponent<TMP_Text>();
		}

		public void SetValue(float value)
		{
			string valueString = value.ToString($"F{decimalPlaces}");
			textBox.text = $"{preText} {valueString} {postText}";
		}
	}
}
