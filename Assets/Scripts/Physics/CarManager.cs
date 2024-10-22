using UnityEngine;
using Soap.GameManagement;

namespace Soap.Physics
{
	public class CarManager : MonoBehaviour
	{
		public bool IsPhysicsEnabled {get; private set;}

		private Rigidbody carRigidBody;

		private void Awake()
		{
			carRigidBody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			DisablePhysics();
		}

		public void EnablePhysics()
		{
			IsPhysicsEnabled = true;
			carRigidBody.useGravity = true;
		}

		public void DisablePhysics()
		{
			IsPhysicsEnabled = false;
			carRigidBody.useGravity = false;
		}

		public void Reset()
		{
			DisablePhysics();

			foreach(Wheel wheel in GetComponentsInChildren<Wheel>())
			{
				wheel.Reset();
			}
		}

		private void FixedUpdate()
		{
			if(StateManager.Instance.State == RaceState.PreStart || IsPhysicsEnabled == false)
			{
				carRigidBody.Sleep();
				return;
			}
		}
	}
}