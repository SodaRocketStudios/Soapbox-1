using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private Wheel[] wheels;
		private AeroSurface[] aeroSurfaces;

		private ProtoERS ers;

		private float accelerationInput;

		private void Start()
		{
			wheels = GetComponentsInChildren<Wheel>();
			aeroSurfaces = GetComponentsInChildren<AeroSurface>();
			ers = GetComponent<ProtoERS>();
		}

		private void Update()
		{
			float torque = ers.UseERS(accelerationInput);
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