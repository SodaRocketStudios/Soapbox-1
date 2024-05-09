using TMPro;
using UnityEngine;

	public class SpeedReader : MonoBehaviour
	{
		[SerializeField] private Rigidbody carRigidbody;

		[SerializeField] private TMP_Text speedTextBox;

		private void Update()
		{
			speedTextBox.text = (Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.forward)*3.6).ToString("F2") + "kph";
		}
	}
