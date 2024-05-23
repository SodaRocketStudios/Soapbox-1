using UnityEngine;

namespace Soap.Physics
{
	public class AeroSurface : MonoBehaviour
	{
		private const float AIR_DENSITY = 1.225f;

		[SerializeField] private float frontalArea;

		[SerializeField] private float dragCoefficient;

		[SerializeField] private float liftToDragRatio;

		[SerializeField, Range(0, 1)] private float DragReductionRatio = 1;

		private bool DRSEnabled = false;

		private Rigidbody carRigidBody;

		private void Start()
		{
			carRigidBody = transform.root.GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			float longitudinalVelocity = Vector3.Dot(carRigidBody.velocity, transform.forward);
			float longitudinalVelocitySquared = longitudinalVelocity * longitudinalVelocity;

			float dynamicFrontalArea = frontalArea;

			if(DRSEnabled)
			{
				dynamicFrontalArea *= DragReductionRatio;
			}

			float dragForce = longitudinalVelocitySquared*dynamicFrontalArea*dragCoefficient*AIR_DENSITY/2;

			float liftForce = longitudinalVelocitySquared*dynamicFrontalArea*dragCoefficient*liftToDragRatio*AIR_DENSITY/2;

			carRigidBody.AddForceAtPosition(-dragForce*transform.forward, transform.position);
			carRigidBody.AddForceAtPosition(-liftForce*transform.up, transform.position);
		}

		public void ToggleDRS()
		{
			DRSEnabled = !DRSEnabled;
		}
	}
}
