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

		[SerializeField, Min(0)] private float minLength;

		[SerializeField] private TireProfile tireProfile;
		private float[] a;
		private float[] b;


		private Rigidbody carRigidBody;

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
				float verticalVelocity = Vector3.Dot(velocity, transform.up);
				float longitudinalVelocity = Vector3.Dot(velocity, transform.forward);
				float lateralVelocity = Vector3.Dot(velocity, transform.right);
				Vector3 planarVelocity = transform.InverseTransformVector(velocity).XZPlane();

				Vector3 planarHeading = transform.forward.XZPlane()*Mathf.Sign(planarVelocity.z);

				float slipAngle = Vector3.SignedAngle(planarHeading, planarVelocity, transform.up);

				float slipRatio = 0;

				float camber = 0;

				float suspensionForce = springStrength*(restLength - length) - damperStrength*verticalVelocity;
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

				Debug.DrawRay(transform.position, lateralForce, Color.red);

				// if target length is less than min length
				if(length < minLength)
				{
					// the load from the body of the car will instantly be transfered to the wheel
					// load on wheel = load from car instead of suspension force.
					// length = min length
					// verticalvelocity = 0;
				}

				carRigidBody.AddForceAtPosition(suspensionForce*transform.up, transform.position);
				carRigidBody.AddForceAtPosition(lateralForce, transform.position);
			}

			// if the suspension cannot reach the ground, the load on the tire is zero.
			// suspension force is applied to wheel instead of car body.
		}

		// private float Get
	}
}