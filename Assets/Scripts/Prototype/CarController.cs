using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;
using System.Linq;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		[SerializeField, Range(0.5f, 1f)] private float brakeBias;

		[SerializeField] private float preloadTorque;
		[SerializeField, Range(1, 5)] float torqueBiasRatio;

		private Rigidbody carRigidBody;

		private DifferentialV1 diff;

		private Wheel[] wheels;
		private Wheel[] driveWheels = new Wheel[2];

		private AeroSurface[] aeroSurfaces;

		private MGUK mguk;

		private bool isPhysicsEnabled;

		private float accelerationInput;

		private void Awake()
		{
			carRigidBody = GetComponent<Rigidbody>();
			wheels = GetComponentsInChildren<Wheel>();
			driveWheels = wheels.Where(wheel => wheel.IsDriveWheel).ToArray();
			aeroSurfaces = GetComponentsInChildren<AeroSurface>();
			mguk = GetComponent<MGUK>();
			diff = new(driveWheels, preloadTorque, torqueBiasRatio);

			PhysicsEnabled(false);
		}

		private void FixedUpdate()
		{
			if (isPhysicsEnabled == false)
			{
				if(accelerationInput > 0)
				{
					PhysicsEnabled(true);
					return;
				}
				carRigidBody.Sleep();
				return;
			}

			float torque;
			
			if(accelerationInput > 0)
			{
				torque = mguk.Deploy(accelerationInput);
			}
			else
			{
				torque = mguk.Recharge();
			}

			diff.Accelerate(torque);
		}

		public void PhysicsEnabled(bool enabled)
		{
			isPhysicsEnabled = enabled;
			carRigidBody.useGravity = enabled;
		}

		public void Steer(InputAction.CallbackContext context)
		{
			float steerInput = context.ReadValue<float>();
			
			foreach (Wheel wheel in wheels)
			{
				wheel.Steer(steerInput);
			}
		}

		public void Brake(InputAction.CallbackContext context)
		{
			float brakeInput = context.ReadValue<float>();

			if(brakeInput > 0)
			{
				foreach(AeroSurface surface in aeroSurfaces)
				{
					surface.ToggleDRS();
				}
			}

			foreach(Wheel wheel in wheels)
			{
				wheel.Brake(brakeInput, brakeBias);
			}
		}

		public void DRS(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				foreach(AeroSurface surface in aeroSurfaces)
				{
					surface.ToggleDRS();
				}
			}
		}

		public void Accelerate(InputAction.CallbackContext context)
		{
			accelerationInput = context.ReadValue<float>();
		}
	}
}