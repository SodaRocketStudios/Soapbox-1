using UnityEngine;

namespace Soap.Prototype
{
	public class ProtoAeroSurface : MonoBehaviour
	{
		private const float AIR_DENSITY = 1.225f;

		[SerializeField] private float frontalArea;

		[SerializeField] private float dragCoefficient;

		[SerializeField] private float liftToDragRatio;

		private Rigidbody carRigidBody;

		private void Start()
		{
			carRigidBody = transform.root.GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			float longitudinalVelocity = Vector3.Dot(carRigidBody.velocity, transform.forward);
			float longitudinalVelocitySquared = longitudinalVelocity * longitudinalVelocity;

			float dragForce = longitudinalVelocitySquared*frontalArea*dragCoefficient*AIR_DENSITY/2;
			Debug.DrawRay(transform.position, transform.forward*dragForce, Color.blue);

			float liftForce = longitudinalVelocitySquared*frontalArea*dragCoefficient*liftToDragRatio*AIR_DENSITY/2;
			Debug.DrawRay(transform.position, transform.up*liftForce, Color.magenta);

			carRigidBody.AddForceAtPosition(-dragForce*transform.forward, transform.position);
			carRigidBody.AddForceAtPosition(-liftForce*transform.up, transform.position);
		}
	}
}