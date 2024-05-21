using UnityEngine;
using UnityEngine.InputSystem;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private WheelV3[] wheels;

		private void Start()
		{
			wheels = GetComponentsInChildren<WheelV3>();
		}

		public void Steer(InputAction.CallbackContext context)
		{
			foreach (WheelV3 wheel in wheels)
			{
				wheel.Steer(context.ReadValue<float>());
			}
		}

		public void Brake(InputAction.CallbackContext context)
		{
			foreach(WheelV3 wheel in wheels)
			{
				wheel.Brake(context.ReadValue<float>());
			}
		}
	}
}
