using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private Wheel[] wheels;
		private AeroSurface[] aeroSurfaces;

		private MGUK mguk;

		private float accelerationInput;

		private void Start()
		{
			wheels = GetComponentsInChildren<Wheel>();
			aeroSurfaces = GetComponentsInChildren<AeroSurface>();
			mguk = GetComponent<MGUK>();
		}

		private void Update()
		{
			float torque = 0;
			if(accelerationInput > 0)
			{
				torque = mguk.Deploy(accelerationInput);
			}
			else
			{
				torque = mguk.Recharge();
			}

			foreach(Wheel wheel in wheels)
			{
				wheel.Accelerate(torque);
			}
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
				wheel.Brake(brakeInput);
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