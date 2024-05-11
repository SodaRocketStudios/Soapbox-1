using UnityEngine;
public class WheelV2 : MonoBehaviour
{
	[SerializeField, Min(0)] private float wheelRadius = 0.457f;

	[SerializeField, Min(0)] private float springStrength;

	[SerializeField, Min(0)] private float damperStrength; 

	[SerializeField, Min(0)] private float restLength;

	[SerializeField, Min(0)] private float maxLength;

	[SerializeField, Min(0)] private float minLength;

	private Rigidbody carRigidBody;

	// Pacejka constants
	private float D = 1.25f;
	private float C = 1.9f;
	private float B = 0.1f;
	private float E = 0.97f;

	private void Start()
	{
		carRigidBody = transform.root.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		RaycastHit hit;

		// If suspension can reach the ground.
		if(Physics.Raycast(transform.position, -transform.up, out hit, maxLength + wheelRadius))
		{
			// suspension length
			float length = hit.distance - wheelRadius;

			Vector3 Velocity = carRigidBody.GetPointVelocity(transform.position);
			float verticalVelocity = Vector3.Dot(Velocity, transform.up);
			float longitudinalVelocity = Vector3.Dot(Velocity, transform.forward);
			float lateralVelocity = Vector3.Dot(Velocity, transform.right);
			Vector3 planarVelocity = new Vector3(Vector3.Dot(Velocity, carRigidBody.transform.right), 0, Vector3.Dot(Velocity, carRigidBody.transform.forward));

			float suspensionForce = springStrength*(restLength - length) - damperStrength*verticalVelocity;

			float slipAngle = Vector3.SignedAngle(transform.forward, planarVelocity, transform.up);

			float lateralForce = -D * Mathf.Sin(C * Mathf.Atan((B*slipAngle) - E * ((B*slipAngle) - Mathf.Atan(B * slipAngle)))) * suspensionForce;

			Debug.Log(suspensionForce);

			// if target length is less than min length
			if(length < minLength)
			{
				// the load from the body of the car will instantly be transfered to the wheel
				// load on wheel = load from car instead of suspension force.
				// length = min length
				// verticalvelocity = 0;
			}

			carRigidBody.AddForceAtPosition(suspensionForce*transform.up, transform.position);
			carRigidBody.AddForceAtPosition(lateralForce*transform.right, transform.position);
		}

		// if the suspension cannot reach the ground, the load on the tire is zero.
		// suspension force is applied to wheel instead of car body.
	}

	// private float Get
}