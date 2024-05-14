using UnityEngine;
using SRS.Extensions.Vector;
using Soap.Physics;

namespace Soap.Prototype
{
	public class WheelV2 : MonoBehaviour
	{
		[SerializeField, Min(0)] private float wheelRadius = 0.457f;

		[SerializeField, Min(0)] private float springStrength;

		[SerializeField, Min(0)] private float damperStrength; 

		[SerializeField, Min(0)] private float restLength;

		[SerializeField, Min(0)] private float maxLength;

		[SerializeField] private TireProfile tireProfile;
		private float[] a;
		private float[] b;


		// Steering variables
		[SerializeField, Range(0, 30)] private float steeringAngle = 0;
		[SerializeField, Min(0)] private float steeringSpeed = 1;
		private float previousSteerAngle;
		private float steerInput;
		private float steerTime;

		// Braking Inputs
		private float brakeInput;

		private Rigidbody carRigidBody;

		private float wheelSpeed = 0;

		private float suspensionLength;

		private void Start()
		{
			carRigidBody = transform.root.GetComponent<Rigidbody>();

			a = tireProfile.LateralParameters;
			b = tireProfile.LongitudinalParameters;
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
				Vector3 planarVelocity = transform.InverseTransformVector(velocity).XZPlane();
				float verticalVelocity = Vector3.Dot(velocity, transform.up);
				float longitudinalVelocity = Vector3.Dot(velocity, transform.forward);

				Vector3 planarHeading = transform.forward.XZPlane()*Mathf.Sign(planarVelocity.z);

				float slipAngle = Vector3.SignedAngle(planarHeading, planarVelocity, transform.up);

				float wheelAcceleration = longitudinalVelocity - wheelSpeed;

				wheelAcceleration += -brakeInput*0.2f;

				if(Mathf.Abs(wheelAcceleration) > Mathf.Abs(longitudinalVelocity))
				{
					wheelAcceleration = -longitudinalVelocity;
				}

				wheelSpeed += wheelAcceleration;

				Debug.DrawRay(transform.position, wheelSpeed*transform.forward, Color.blue);

				float slipRatio = 0;

				if(longitudinalVelocity != 0)
				{
					slipRatio = (wheelSpeed/longitudinalVelocity) - 1;
				}

				slipRatio = Mathf.Min(slipRatio, 1);
				slipRatio = Mathf.Max(slipRatio, -1);

				Debug.Log(slipRatio);

				float camber = 0;

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

				float suspensionForceSquared = suspensionForce*suspensionForce;

				// Pacejka Magic Formula
				// Lateral
				float Clat = a[0];
				float Dlat = (a[1]*suspensionForceSquared + a[2]*suspensionForce)*(1 - a[15]*camber*camber);
				float Blat = a[3]*Mathf.Sin(Mathf.Atan(suspensionForce/a[4])*2)*(1-a[5]*Mathf.Abs(camber))/(Clat*Dlat);
				float Hlat = a[8]*suspensionForce + a[9] + a[10]*camber;
				float Vlat = a[11]*suspensionForce + a[12] + (a[13]*suspensionForceSquared + a[14]*suspensionForce)*camber;
				float Elat = (a[6]*suspensionForce + a[7])*(1 - (a[16]*camber + a[17])*Mathf.Sign(slipAngle + Hlat));
				float Bxlat = Blat*(slipAngle + Hlat);
				Vector3 lateralForce = -(Dlat*Mathf.Sin(Clat*Mathf.Atan(Bxlat - Elat*(Bxlat - Mathf.Atan(Bxlat)))) + Vlat)*transform.right;

				// Longitudinal
				float Clong = b[0];
				float Dlong = b[1]*suspensionForceSquared + b[2]*suspensionForce;
				float Blong = (b[3]*suspensionForceSquared + b[4]*suspensionForce)*Mathf.Exp(-b[5]*suspensionForce)/(Clong*Dlong);
				float Hlong = b[9]*suspensionForce + b[10];
				float Vlong = b[11]*suspensionForce + b[12];
				float Elong = (b[6]*suspensionForceSquared + b[7]*suspensionForce + b[8])*(1 - b[13]*Mathf.Sign(slipRatio + Hlong));
				float Bxlong = Blong*(slipRatio + Hlong);
				Vector3 longitudinalForce = (Dlong*Mathf.Sin(Clong*Mathf.Atan(Bxlong - Elong*(Bxlong - Mathf.Atan(Bxlong)))) + Vlong)*transform.forward;

				Debug.DrawRay(transform.position, lateralForce.normalized, Color.red);
				Debug.DrawRay(transform.position, longitudinalForce.normalized, Color.blue);

				HandleSteering();

				carRigidBody.AddForceAtPosition(suspensionForce*transform.up, transform.position);
				carRigidBody.AddForceAtPosition(lateralForce, transform.position);
				carRigidBody.AddForceAtPosition(longitudinalForce, transform.position);
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