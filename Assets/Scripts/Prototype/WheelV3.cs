using UnityEngine;
using Soap.Physics;
using SRS.Extensions.Vector;

namespace Soap.Prototype
{
	public class WheelV3 : MonoBehaviour
	{
		// Wheel Data
		[SerializeField, Min(0)] private float wheelRadius = 0.457f;

		[SerializeField, Min(0)] private float springStrength;

		[SerializeField, Min(0)] private float damperStrength; 

		[SerializeField, Min(0)] private float restLength;

		[SerializeField, Min(0)] private float maxLength;

		[SerializeField] private CurveTireProfile tireProfile;


		// Steering variables
		[SerializeField, Range(0, 30)] private float steeringAngle = 0;
		[SerializeField, Min(0)] private float steeringSpeed = 1;
		private float previousSteerAngle;
		private float steerInput;
		private float steerTime;

		[SerializeField] private float overrideSpeed = 0;
		private float overrideSpeedSquared;

		private float brakeInput;

		private Rigidbody carRigidBody;

		private float wheelSpeed = 0;

		private float suspensionLength;

		private void Start()
		{
			carRigidBody = transform.root.GetComponent<Rigidbody>();
			overrideSpeedSquared = overrideSpeed*overrideSpeed;
		}

		private void FixedUpdate()
		{
			RaycastHit hit;

			// If suspension can reach the ground.
			if(UnityEngine.Physics.Raycast(transform.position, -transform.up, out hit, maxLength + wheelRadius))
			{
				// suspension length
				float length = hit.distance - wheelRadius;

				Vector3 velocity = carRigidBody.GetPointVelocity(transform.position);
				Vector3 planarVelocity = velocity.XZPlane();
				float verticalVelocity = Vector3.Dot(velocity, transform.up);
				float longitudinalVelocity = Vector3.Dot(velocity, transform.forward);
				float lateralVelocity = Vector3.Dot(velocity, transform.right);

				Debug.DrawRay(transform.position, longitudinalVelocity*transform.forward);

				Vector3 planarHeading = transform.forward.XZPlane();

				float slipAngle = -Vector3.SignedAngle(planarHeading, planarVelocity, transform.up);

				if(Mathf.Abs(slipAngle) > 90)
				{
					slipAngle -= 180*Mathf.Sign(slipAngle);
				}

				Debug.Log(slipAngle);

				if(planarVelocity.sqrMagnitude == 0)
				{
					slipAngle = 0;
				}

				float wheelAcceleration = longitudinalVelocity - wheelSpeed; // try to match wheel speed to velocity.

				float brakeAcceleration = -Time.deltaTime*brakeInput*wheelSpeed;

				if(Mathf.Abs(brakeAcceleration) > Mathf.Abs(wheelSpeed))
				{
					brakeAcceleration = -wheelSpeed;
				}

				wheelAcceleration += brakeAcceleration; // apply braking force.
				
				wheelSpeed += wheelAcceleration;

				if(longitudinalVelocity < 0.25f && brakeInput > 0)
				{
					carRigidBody.useGravity = false;
					carRigidBody.velocity = Vector3.zero;
					carRigidBody.angularVelocity = Vector3.zero;
				}
				else
				{
					carRigidBody.useGravity = true;
				}

				float slipRatio;

				if(longitudinalVelocity == 0)
				{
					if(wheelSpeed == 0)
					{
						slipRatio = 0;
					}
					else
					{
						slipRatio = 0.01f*Mathf.Sign(wheelSpeed);
					}
				}
				else
				{
					slipRatio = (wheelSpeed - longitudinalVelocity)/Mathf.Abs(longitudinalVelocity);
				}

				float suspensionForce = springStrength*(restLength - length) - damperStrength*verticalVelocity;

				// if suspension is fully compressed
				if(length <= 0)
				{
					// the load from the body of the car will instantly be transfered to the wheel
					// load on wheel = load from car instead of suspension force.
					// length = min length
					// verticalvelocity = 0;
					// suspensionForce = verticalVelocity*carRigidBody.mass;
				}

				suspensionLength = length;

				float load = Mathf.Abs(suspensionForce);  // TODO -- Double check this logic.

				// Lateral
				Vector3 lateralForce = tireProfile.EvaluateLateral(slipAngle)*load*transform.right;

				if(velocity.sqrMagnitude <= overrideSpeedSquared)
				{
					lateralForce = -lateralVelocity*load*Vector3.right;
				}

				Debug.DrawRay(transform.position, lateralForce.normalized, Color.red);

				// Longitudinal
				Vector3 longitudinalForce = tireProfile.EvaluateLongitudinal(slipRatio)*load*transform.forward;
				
				Debug.DrawRay(transform.position, longitudinalForce.normalized, Color.blue);

				HandleSteering();

				if(carRigidBody.useGravity)
				{
					carRigidBody.AddForceAtPosition(suspensionForce*transform.up, transform.position);
					carRigidBody.AddForceAtPosition(lateralForce, transform.position);
					carRigidBody.AddForceAtPosition(longitudinalForce, transform.position);
				}
			}

			// if the suspension cannot reach the ground, the load on the tire is zero.
			// suspension force is applied to wheel instead of car body.
		}

		public void Steer(float inputValue)
		{
			previousSteerAngle = transform.localEulerAngles.y;
			if(previousSteerAngle > 180)
			{
				previousSteerAngle -= 360;
			}

			steerInput = inputValue;
			steerTime = 0;
		}

		private void HandleSteering()
		{
			transform.localEulerAngles = Mathf.Lerp(previousSteerAngle, steerInput*steeringAngle, Mathf.Clamp01(steerTime*steeringSpeed))*Vector3.up;

			steerTime += Time.fixedDeltaTime;
		}

		public void Brake(float inputValue)
		{
			brakeInput = inputValue;
		}

		private void HandleBraking()
		{

		}

		private Vector3 MagicFormula()
		{
			return new Vector3();
		}

		private float SimplifiedMagicFormula(float alpha)
		{
			float A = 25;
			float B = 85;
			float P = 1.5f;

			return B*alpha/(1 + Mathf.Pow(Mathf.Abs(A*alpha), P));
		}

		private void ShowTelemetry()
		{

		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawRay(transform.position, -transform.up*restLength);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, -transform.up*suspensionLength);
			Gizmos.DrawWireSphere(transform.position -suspensionLength*transform.up, wheelRadius);
		}
	}
}
