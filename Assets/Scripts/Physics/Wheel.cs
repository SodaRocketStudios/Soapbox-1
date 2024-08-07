using UnityEngine;
using SRS.Extensions.Vector;

namespace Soap.Physics
{
	public class Wheel : MonoBehaviour
	{
		[Header("Tire Profile")]
		[SerializeField] private CurveTireProfile tireProfile;

		// Steering variables
		[Header("Steering Parameters")]
		[SerializeField, Range(0, 30)] private float steeringAngle = 0;
		[SerializeField, Min(0)] private float steeringSpeed = 1;
		private float previousSteerAngle;
		private float steerInput;
		private float steerTime;

		[Header("Other Paramters")]
		[SerializeField] private float brakeTorque;
		[SerializeField] private float overrideSpeed = 0;
		private float overrideSpeedSquared;

		[SerializeField] private bool isDriveWheel;
		public bool IsDriveWheel
		{
			get => isDriveWheel;
		}
		private float driveTorque;

		public float Torque {get; private set;}

		public float Radius
		{
			get => tireProfile.Radius;
		}

		private float brakeInput;

		public float WheelSpeed {get; private set;} = 0;

		private float load;

		private bool isGrounded;

		private Rigidbody carRigidBody;

		private Transform wheelMesh; // TODO -- This should probably be in an animator script rather than with physics.

		private void Awake()
		{
			wheelMesh = transform.GetChild(0);
		}

		private void Start()
		{
			carRigidBody = transform.root.GetComponent<Rigidbody>();
			overrideSpeedSquared = overrideSpeed * overrideSpeed;
		}

		private void Update()
		{
			wheelMesh.transform.Rotate(Vector3.right, WheelSpeed/Radius);
		}

		private void FixedUpdate()
        {
			if(carRigidBody.IsSleeping())
			{
				return;
			}
			
			if(isGrounded)
			{
				Vector3 velocity = carRigidBody.GetPointVelocity(transform.position);
				Vector3 planarVelocity = velocity.XZPlane();
				Vector3 planarHeading = transform.forward.XZPlane();
				float longitudinalVelocity = Vector3.Dot(velocity, transform.forward);

				WheelSpeed = longitudinalVelocity;

				HandleBraking(longitudinalVelocity);

				HandleAcceleration();

				HandleSteering();

				float normalizedSlipAngle = CalculateSlipAngle(planarVelocity, planarHeading) / tireProfile.PeakSlipAngle;

				float normalizedSlipRatio = CalculateSlipRatio(longitudinalVelocity) / tireProfile.PeakSlipRatio;

				// Locked in braking
				if(normalizedSlipRatio <= -1)
				{
					// Debug.DrawRay(transform.position, Vector3.up*5, Color.red);
				}

				// Combined slip ------------------------------------------------------------------

				float combinedSlip = Mathf.Sqrt(normalizedSlipAngle * normalizedSlipAngle + normalizedSlipRatio * normalizedSlipRatio);

				float lateralFactor;
				float longitudinalFactor;

				if (combinedSlip == 0)
				{
					lateralFactor = 0;
					longitudinalFactor = 0;
				}
				else
				{
					lateralFactor = normalizedSlipAngle / combinedSlip;
					longitudinalFactor = normalizedSlipRatio / combinedSlip;
				}

				// Lateral ------------------------------------------------------------------------

				Vector3 lateralForce = lateralFactor * tireProfile.EvaluateLateral(combinedSlip * tireProfile.PeakSlipAngle) * load * transform.right;

				if (planarVelocity.sqrMagnitude < overrideSpeedSquared)
				{
					lateralForce *= Mathf.Lerp(0.1f, 1, planarVelocity.sqrMagnitude/overrideSpeedSquared);
					// lateralForce = -lateralVelocity * load * Vector3.right;
				}

				Debug.DrawRay(transform.position, lateralForce.normalized, Color.red);

				// Longitudinal -------------------------------------------------------------------

				Vector3 longitudinalForce = longitudinalFactor * tireProfile.EvaluateLongitudinal(combinedSlip * tireProfile.PeakSlipRatio) * load * transform.forward;

				Torque = longitudinalForce.magnitude * tireProfile.Radius;

				Debug.DrawRay(transform.position, longitudinalForce.normalized, Color.blue);

				if (carRigidBody.useGravity)
				{
					carRigidBody.AddForceAtPosition(lateralForce, transform.position);
					carRigidBody.AddForceAtPosition(longitudinalForce, transform.position);
				}
			}
        }

        public void SetLoad(float load)
		{
			this.load = Mathf.Max(load + tireProfile.Mass*UnityEngine.Physics.gravity.magnitude, 0);
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}

		public void SetLocalPosition(Vector3 position)
		{
			transform.localPosition = position;
		}

		public void SetGrounded(bool grounded)
		{
			isGrounded = grounded;
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
			steerTime += Time.fixedDeltaTime;
			transform.localEulerAngles = Mathf.Lerp(previousSteerAngle, steerInput*steeringAngle, Mathf.Clamp01(steerTime*steeringSpeed))*Vector3.up;
		}

		public void Brake(float inputValue, float brakeBias)
		{
			if(isDriveWheel)
			{
				brakeInput = inputValue * (1 - brakeBias);
			}
			else
			{
				brakeInput = inputValue * brakeBias;
			}
		}

		private void HandleBraking(float longitudinalVelocity)
		{
			float brakeAcceleration = -Time.deltaTime*brakeInput*WheelSpeed*brakeTorque;

			if(Mathf.Abs(brakeAcceleration) > Mathf.Abs(WheelSpeed))
			{
				brakeAcceleration = -WheelSpeed;
			}

			WheelSpeed += brakeAcceleration; // apply braking force.

			if(longitudinalVelocity < 0.1f && brakeInput > 0)
			{
				carRigidBody.useGravity = false;
				carRigidBody.velocity = Vector3.zero;
				carRigidBody.angularVelocity = Vector3.zero;
			}
			else
			{
				carRigidBody.useGravity = true;
			}	
		}

		public void Accelerate(float torque)
		{
			if(isDriveWheel)
			{
				driveTorque = torque;
			}
		}

		private void HandleAcceleration()
		{
			float wheelAcceleration = driveTorque/(Radius*tireProfile.Mass);

			WheelSpeed += wheelAcceleration;
		}

		private float CalculateSlipAngle(Vector3 planarVelocity, Vector3 planarHeading)
        {
            // Avoid errors when getting the angle to a zero vector.
            if (planarVelocity.sqrMagnitude == 0)
            {
                return 0;
            }
			else
			{
				return -Vector3.SignedAngle(planarHeading, planarVelocity, transform.up);
			}
        }

        private float CalculateSlipRatio(float longitudinalVelocity)
        {
            if (longitudinalVelocity == 0)
            {
                if (WheelSpeed == 0)
                {
                    return 0;
                }
                else
                {
                    return Mathf.Clamp(WheelSpeed, -1, 1);
                }
            }
            else
            {
                return (WheelSpeed - longitudinalVelocity) / Mathf.Abs(longitudinalVelocity);
            }
        }
	}
}