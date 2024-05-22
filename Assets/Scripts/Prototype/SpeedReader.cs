using TMPro;
using UnityEngine;

namespace Soap.Prototype
{
	public class SpeedReader : MonoBehaviour
	{
		[SerializeField] private Rigidbody carRigidbody;

		[SerializeField] private TMP_Text longitudinalTextBox;
		[SerializeField] private TMP_Text lateralTextBox;

		private void Update()
		{
			float longitudinalVelocity = Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.forward);
			float lateralVelocity = Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.right);
			longitudinalTextBox.text = $"Long: {longitudinalVelocity:F2} m/s";
			lateralTextBox.text = $"Lat: {lateralVelocity:F2} m/s";
		}
	}
}