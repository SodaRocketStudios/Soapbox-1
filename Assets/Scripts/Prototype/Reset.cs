using Soap.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using Soap.LapTiming;
using Soap.GameManagement;

namespace Soap.Prototype
{
	public class Reset : MonoBehaviour
	{
		private Rigidbody carRigidbody;

		private MGUK mguk;

		private CarController controller;

		[SerializeField] private LapTimer timer;

		[SerializeField] private StartingLights startLights;

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
				GameState.Instance.State = State.PreStart;

				controller.PhysicsEnabled(false);
				
				transform.position = initializer.initializedPosition;
				transform.rotation = initializer.initializedRotation;
				carRigidbody.velocity = Vector3.zero;
				carRigidbody.angularVelocity = Vector3.zero;

				mguk.Reset();

				timer.Reset();

				startLights.Disable();
			}
		}
	}
}