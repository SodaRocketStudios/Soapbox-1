using UnityEngine;
using SRS.Utils.Timing;
using TMPro;

namespace Soap.Prototype
{
	public class ProtoTimeDisplay : MonoBehaviour
	{
		private Timer timer;

		private TMP_Text textBox;

		private void Awake()
		{
			textBox = GetComponent<TMP_Text>();
			timer = new Timer();
		}

		private void OnEnable()
		{
			timer.Time.OnChange += UpdateDisplay;
		}

		private void OnDisable()
		{
			timer.Time.OnChange -= UpdateDisplay;
		}

		private void Start()
		{
			timer.Start();
		}

		private void UpdateDisplay(float time)
		{
			int minutes = (int)time/60;
			float seconds = time - minutes*60;
			if(minutes <= 0)
			{
				textBox.text = $"{seconds:F3}";
				return;
			}
			textBox.text = $"{minutes}:{seconds:F3}";
		}
	}
}