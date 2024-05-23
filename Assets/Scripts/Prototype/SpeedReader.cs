using TMPro;
using UnityEngine;

namespace Soap.Prototype
{
	public class SpeedReader : MonoBehaviour
	{
		[SerializeField] private Rigidbody carRigidbody;

		[SerializeField] private TMP_Text speedTextBox;

		private void Update()
		{
			float longitudinalVelocity = Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.forward);
			speedTextBox.text = $"{longitudinalVelocity*3.6f:F2} kph";
		}
	}
}