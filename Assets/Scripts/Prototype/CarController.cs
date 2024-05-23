using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;

namespace Soap.Prototype
{
	public class CarController : MonoBehaviour
	{
		private Wheel[] wheels;

		private void Start()
		{
			wheels = GetComponentsInChildren<Wheel>();
		}

		public void Steer(InputAction.CallbackContext context)
		{
			foreach (Wheel wheel in wheels)
			{
				wheel.Steer(context.ReadValue<float>());
			}
		}

		public void Brake(InputAction.CallbackContext context)
		{
			foreach(Wheel wheel in wheels)
			{
				wheel.Brake(context.ReadValue<float>());
			}
		}
	}
}
