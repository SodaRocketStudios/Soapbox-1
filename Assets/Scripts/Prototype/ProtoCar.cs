using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoCar : MonoBehaviour
{
	private Rigidbody carRigidBody;

	private float b = 2.6f;
	private float c = 2.4f;

	private float maxSteeringAngle = 15;

	private float corneringStiffness = 0.5f;
	
	private float brakingConstant = 1;

	private float rollingResistance = 0.1f;

	private float dragCoefficient = 0.7f;

	private float steeringAngle;

	private float brakingInput;

	private float steerInput;

	private void Start()
	{
		carRigidBody = GetComponent<Rigidbody>();	
	}

	private void FixedUpdate()
	{
		float longitudinalVelocity = Vector3.Dot(carRigidBody.velocity, transform.forward);
		float lateralVelocity = Vector3.Dot(carRigidBody.velocity, transform.right);

		float angularVelocity = Vector3.Dot(carRigidBody.angularVelocity, transform.up);

		float frontSlipAngle = Mathf.Atan((lateralVelocity + angularVelocity*b)/longitudinalVelocity) - steeringAngle*Mathf.Sign(longitudinalVelocity);
		float rearSlipAngle = Mathf.Atan((lateralVelocity - angularVelocity*c)/longitudinalVelocity);

		Debug.Log($"{frontSlipAngle}, {rearSlipAngle}");

		float lateralForceFront = Mathf.Min(corneringStiffness*frontSlipAngle, 1);
		float lateralForceRear = Mathf.Min(corneringStiffness*rearSlipAngle, 1);

		lateralForceFront *= carRigidBody.mass/2;
		lateralForceRear *= carRigidBody.mass/2;

		float brakingForce = brakingInput*brakingConstant;

		float lateralResistance = rollingResistance*lateralVelocity + dragCoefficient*lateralVelocity;
		float longitudinalResistance = rollingResistance*longitudinalVelocity + dragCoefficient*longitudinalVelocity;

		float lateralforce = lateralForceRear + lateralForceFront*Mathf.Cos(steeringAngle)*lateralResistance;
		float longitudinalForce = brakingForce + lateralForceFront*Mathf.Sin(steeringAngle)*longitudinalResistance;

		float bodyTorque = Mathf.Cos(steeringAngle)*lateralForceFront*b - lateralForceRear*c;

		carRigidBody.AddForce(longitudinalForce*transform.forward, ForceMode.Force);
		carRigidBody.AddForce(lateralforce*transform.right, ForceMode.Force);

		carRigidBody.AddTorque(0, bodyTorque, 0);
	}

	public void Brake(InputAction.CallbackContext context)
	{
		brakingInput = context.ReadValue<float>();
	}

	public void Steer(InputAction.CallbackContext context)
	{
		steerInput = context.ReadValue<float>();

		steeringAngle = steerInput*maxSteeringAngle;
	}
}