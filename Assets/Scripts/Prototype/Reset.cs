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

		private CarController controller;

		[SerializeField] private LapTimer timer;

		private CarInitializer initializer;

		private void Start()
		{
			carRigidbody = GetComponent<Rigidbody>();

			mguk = GetComponent<MGUK>();

			controller = GetComponent<CarController>();

			initializer = GetComponent<CarInitializer>();
		}

		public void ResetToStart(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				controller.PhysicsEnabled(false);
				
				transform.position = initializer.initializedPosition;
				transform.rotation = initializer.initializedRotation;
				carRigidbody.velocity = Vector3.zero;
				carRigidbody.angularVelocity = Vector3.zero;

				mguk.Reset();

				timer.Reset();
			}
		}
	}
}