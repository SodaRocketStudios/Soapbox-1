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
			IsPhysicsEnabled = false;
		}

		public void DisablePhysics()
		{
			IsPhysicsEnabled = false;
		}

		private void FixedUpdate()
		{
			if(GameState.Instance.State == State.PreStart)
			{
				carRigidBody.Sleep();
				return;
			}

			if (IsPhysicsEnabled == false)
			{
				carRigidBody.Sleep();
				return;
			}
		}
	}
}