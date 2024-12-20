using UnityEngine;

namespace Soap.Physics
{
	public class Suspension : MonoBehaviour
	{
		[SerializeField, Min(0)] private float springStrength;

		[SerializeField, Min(0)] private float damperStrength; 

		[SerializeField, Min(0)] private float restLength;
		public float GroundedLength
		{
			get
			{
				if(wheel == null)
				{
					wheel = GetComponentInChildren<Wheel>();
				}
				
				return restLength + wheel.Radius;
			}
		}

		[SerializeField, Min(0)] private float maxLength;

		private Rigidbody carRigidBody;

		private Wheel wheel;

		private Vector3 wheelOffset;

		private void Awake()
		{
			carRigidBody = transform.root.GetComponent<Rigidbody>();
			wheel = GetComponentInChildren<Wheel>();
			wheelOffset = wheel.transform.localPosition;
			wheelOffset.y = 0;
		}

		private void FixedUpdate()
		{
			if(carRigidBody.IsSleeping())
			{
				return;
			}
			
			RaycastHit hit;

			if(UnityEngine.Physics.Raycast(transform.position, -transform.up, out hit, maxLength + wheel.Radius))
			{
				if(hit.collider.transform == transform.parent)
				{
					return;
				}

				Vector3 velocity = carRigidBody.GetPointVelocity(transform.position);
				float verticalVelocity = Vector3.Dot(velocity, transform.up);
				float length = hit.distance - wheel.Radius;

				float suspensionForce = springStrength*(restLength - length) - damperStrength*verticalVelocity;

				// if suspension is fully compressed
				if(length < 0)
				{
					// TODO -- Handle bottomed out suspension
					// the load from the body of the car will instantly be transfered to the wheel
					// load on wheel = load from car instead of suspension force.
					// length = min length
					// suspensionForce = verticalVelocity*carRigidBody.mass;
					// verticalVelocity = 0;
					length = 0;
				}

				Debug.DrawLine(transform.position, hit.point, Color.green);

				wheel.SetGrounded(true);
				wheel.SetLoad(suspensionForce);
				wheel.SetLocalPosition(wheelOffset - transform.up*length);
				if(carRigidBody.useGravity)
				{
					carRigidBody.AddForceAtPosition(suspensionForce*transform.up, transform.position);
				}

				return;
			}

			wheel.SetGrounded(false);

			// if the suspension cannot reach the ground, the load on the tire is zero.
			// suspension force is applied to wheel instead of car body.
		}
	}
}