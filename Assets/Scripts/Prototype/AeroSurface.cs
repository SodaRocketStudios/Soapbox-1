using UnityEngine;

public class AeroSurface : MonoBehaviour
{
	private const float AIR_DENSITY = 1.225f;

	[SerializeField] private float frontalArea;

	[SerializeField] private float dragCoefficient;

	[SerializeField] private float liftCoefficient;

	private Rigidbody carRigidBody;

	private void Start()
	{
		carRigidBody = transform.root.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		float longitudinalVelocitySquared = Mathf.Pow(Vector3.Dot(carRigidBody.velocity, transform.forward), 2);

		float dragForce = longitudinalVelocitySquared*frontalArea*dragCoefficient*AIR_DENSITY/2;

		float liftForce = longitudinalVelocitySquared*frontalArea*liftCoefficient*AIR_DENSITY/2;

		carRigidBody.AddForceAtPosition(-dragForce*transform.forward, transform.position);
		carRigidBody.AddForceAtPosition(-liftForce*transform.up, transform.position);
	}
}