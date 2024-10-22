using Soap.GameManagement;
using UnityEngine;

namespace Soap.Physics
{
	public class PhysicsReset : MonoBehaviour
	{
		private Rigidbody carRigidbody;

		private MGUK mguk;

		private CarManager manager;

		private CarInitializer initializer;

		private void Start()
		{
			carRigidbody = GetComponent<Rigidbody>();

			mguk = GetComponent<MGUK>();

			manager = GetComponent<CarManager>();

			initializer = GetComponent<CarInitializer>();
		}

		private void OnEnable()
		{
			ResetManager.OnReset += ResetToStart;
		}

		private void OnDisable()
		{
			ResetManager.OnReset -= ResetToStart;
		}

		public void ResetToStart()
		{
			manager.Reset();
			
			transform.position = initializer.initializedPosition;
			transform.rotation = initializer.initializedRotation;
			carRigidbody.velocity = Vector3.zero;
			carRigidbody.angularVelocity = Vector3.zero;

			mguk.Reset();
		}
	}
}