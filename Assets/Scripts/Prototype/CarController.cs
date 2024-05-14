using UnityEngine;
using UnityEngine.InputSystem;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private WheelV2[] wheels;

		private void Start()
		{
			wheels = GetComponentsInChildren<WheelV2>();
		}

		public void Steer(InputAction.CallbackContext context)
		{
			foreach (WheelV2 wheel in wheels)
			{
				wheel.Steer(context.ReadValue<float>());
			}
		}

		public void Brake(InputAction.CallbackContext context)
		{
			foreach(WheelV2 wheel in wheels)
			{
				wheel.Brake(context.ReadValue<float>());
			}
		}
	}
}
