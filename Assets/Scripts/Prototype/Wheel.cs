using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wheel : MonoBehaviour
{

	[SerializeField] private float restLength;

	[SerializeField] private float maxLength;

	[SerializeField] private float strength;

	[SerializeField] private float damper;

	[SerializeField] private float radius;

	[SerializeField, Range(0, 1)] private float grip;

	[SerializeField, Min(0)] private float steerAngle;

	[SerializeField, Min(0)] private float steerSpeed;

	[SerializeField, Min(0)] private float maxBrakeForce;

	private float length;

	private Rigidbody carRigidBody;

	private float brakeInput = 0;

	private float steerInput;

	private float previousSteerAngle;

	private float steerTime;

	private float mass;

	private void Start()
	{
		carRigidBody = transform.root.GetComponent<Rigidbody>();

		length = restLength;

		mass = carRigidBody.mass;
	}

	private void FixedUpdate()
	{
		RaycastHit hit;

		if(Physics.Raycast(transform.position, -transform.up, out hit, maxLength + radius))
		{
			length = hit.distance - radius;

			length = Mathf.Min(length, maxLength);

			float springForce = strength*(restLength - length);
			Vector3 velocity = carRigidBody.GetPointVelocity(transform.position);
			float damperForce = -damper*Vector3.Dot(velocity, transform.up);

			Vector3 suspensionForce = (springForce + damperForce) * transform.up;

			if(length >= maxLength)
			{
				suspensionForce = Vector3.zero;
			}

			carRigidBody.AddForceAtPosition(suspensionForce, transform.position);

			Vector3 lateralForce = -Vector3.Dot(velocity, transform.right)*transform.right*grip*mass;

			carRigidBody.AddForceAtPosition(lateralForce, transform.position + transform.up*radius);

			float longitudinalVelocity = Vector3.Dot(carRigidBody.GetPointVelocity(transform.position), transform.forward);

			float brakeForce = -brakeInput*grip*maxBrakeForce*Mathf.Sign(longitudinalVelocity);

			brakeForce = Mathf.Min(brakeForce, longitudinalVelocity)*mass;

			carRigidBody.AddForceAtPosition(brakeForce*transform.forward, transform.position);
		}

		transform.localEulerAngles = Mathf.Lerp(previousSteerAngle, steerInput*steerAngle, Mathf.Clamp01(steerTime*steerSpeed))*Vector3.up;

		steerTime += Time.fixedDeltaTime;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position, -transform.up*restLength);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -transform.up*length);
		Gizmos.DrawWireSphere(transform.position -length*transform.up, radius);

		Gizmos.color = Color.blue;

		if(carRigidBody != null)
		{

			Gizmos.DrawRay(transform.position, transform.right*Vector3.Dot(carRigidBody.GetPointVelocity(transform.position), transform.right));
		}
	}

	public void Steer(InputAction.CallbackContext context)
	{
		previousSteerAngle = transform.localEulerAngles.y;
		if(previousSteerAngle > 180)
		{
			previousSteerAngle -= 360;
		}

		steerInput = context.ReadValue<float>();
		steerTime = 0;
	}

	public void Brake(InputAction.CallbackContext context)
	{
		brakeInput = context.ReadValue<float>();
	}
}