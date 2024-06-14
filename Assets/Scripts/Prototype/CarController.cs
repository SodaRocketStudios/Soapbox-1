using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;
using System.Linq;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		[SerializeField, Range(0.5f, 1f)] private float brakeBias;
		
		[SerializeField, Range(0.5f, 1f)] private float differential;

		private Wheel[] wheels;
		private Wheel[] driveWheels = new Wheel[2];

		private AeroSurface[] aeroSurfaces;

		private MGUK mguk;

		private float accelerationInput;

		private void Start()
		{
			wheels = GetComponentsInChildren<Wheel>();
			driveWheels = wheels.Where(wheel => wheel.IsDriveWheel).ToArray();
			aeroSurfaces = GetComponentsInChildren<AeroSurface>();
			mguk = GetComponent<MGUK>();
		}

		private void FixedUpdate()
		{
			float torque;
			
			if(accelerationInput > 0)
			{
				torque = mguk.Deploy(accelerationInput);
			}
			else
			{
				torque = mguk.Recharge();
			}

			// TODO -- figure out how the diff clutch affects the acceleration torque.

			float clutchTorque = 0;

			foreach(Wheel wheel in driveWheels)
			{
				clutchTorque += wheel.Torque;
			}

			clutchTorque /= 2;
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