using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private Wheel[] wheels;
		private AeroSurface[] aeroSurfaces;

		private void Start()
		{
			wheels = GetComponentsInChildren<Wheel>();
			aeroSurfaces = GetComponentsInChildren<AeroSurface>();
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
			foreach(Wheel wheel in wheels)
			{
				wheel.Brake(context.ReadValue<float>());
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
	}
}
