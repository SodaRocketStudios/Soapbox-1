using UnityEngine;
using UnityEngine.InputSystem;

namespace Soap.Prototype
{
	public class Reset : MonoBehaviour
	{
		[SerializeField] private Transform carTransform;
		private Rigidbody carRigidbody;

		private Vector3 startLocation;
		private Quaternion startRotation;

		private void Start()
		{
			startLocation = carTransform.position;
			startRotation = carTransform.rotation;
			carRigidbody = carTransform.GetComponent<Rigidbody>();
		}

		public void ResetPosition(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				carTransform.position = startLocation;
				carTransform.rotation = startRotation;
				carRigidbody.velocity = Vector3.zero;
				carRigidbody.angularVelocity = Vector3.zero;
			}
		}

		public void ResetTimeScale(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				Time.timeScale = 1;
			}
		}
	}
}