using Soap.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Soap.Prototype
{
	public class Reset : MonoBehaviour
	{
		[SerializeField] private ProtoTimeDisplay timeDisplay;

		private Rigidbody carRigidbody;

		private MGUK mguk;

		private Vector3 startLocation;
		private Quaternion startRotation;

		private void Start()
		{
			startLocation = transform.position;
			startRotation = transform.rotation;

			carRigidbody = transform.GetComponent<Rigidbody>();

			mguk = GetComponent<MGUK>();
		}

		public void ResetToStart(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				transform.position = startLocation;
				transform.rotation = startRotation;
				carRigidbody.velocity = Vector3.zero;
				carRigidbody.angularVelocity = Vector3.zero;

				mguk.Reset();
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