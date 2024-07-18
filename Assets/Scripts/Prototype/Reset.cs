using Soap.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using Soap.LapTiming;

namespace Soap.Prototype
{
	public class Reset : MonoBehaviour
	{
		private Rigidbody carRigidbody;

		private MGUK mguk;

		[SerializeField] private LapTimer timer;

		private Vector3 startLocation;
		private Quaternion startRotation;

		private void Start()
		{
			startLocation = transform.position;
			startRotation = transform.rotation;

			carRigidbody = GetComponent<Rigidbody>();

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

				timer.Reset();

				GetComponent<CarInitializer>().GroundCar();
			}
		}
	}
}