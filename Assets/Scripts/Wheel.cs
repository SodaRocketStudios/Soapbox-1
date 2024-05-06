using UnityEngine;

public class Wheel : MonoBehaviour
{

	[SerializeField] private float restLength;

	[SerializeField] private float maxLength;

	[SerializeField] private float strength;

	[SerializeField] private float damper;

	[SerializeField] private float radius;

	private float length;

	private Rigidbody carRigidBody;

	private void Start()
	{
		carRigidBody = transform.root.GetComponent<Rigidbody>();

		length = restLength;
	}

	private void FixedUpdate()
	{
		RaycastHit hit;

		if(Physics.Raycast(transform.position, -transform.up, out hit, maxLength + radius))
		{
			length = hit.distance - radius;

			length = Mathf.Min(length, maxLength);

			float springForce = strength*(restLength - length);
			float velocity = Vector3.Dot(carRigidBody.GetPointVelocity(transform.position), transform.up);
			float damperForce = -damper*velocity;

			Vector3 force = (springForce + damperForce) * transform.up;

			carRigidBody.AddForceAtPosition(force, transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position, -transform.up*restLength);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -transform.up*length);
		Gizmos.DrawWireSphere(transform.position -length*transform.up, radius);
	}
}