using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Input;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;

namespace Soap.Prototype
{
	public class RumbleTest : MonoBehaviour
	{
		private float throttle;

		private float brake;

		private void Start()
		{
			InputHandler.OnAccelerateInput += OnAccelerate;
			InputHandler.OnBrakeInput += OnBrake;

			InputHandler.Instance.SetGameplayInput();
		}

		private bool IsPlayStationController(InputDevice device)
		{
			return device is DualShockGamepad;
		}

		private bool IsXBoxController(InputDevice device)
		{
			return device is XInputController;
		}

		public bool HasTriggerRumble()
		{
			return Gamepad.current is IXboxOneRumble;
		}

        private void SetRumble(float left, float right)
		{
			if(HasTriggerRumble())
			{
				Debug.Log("Xbox 1");
				(Gamepad.current as IXboxOneRumble).SetMotorSpeeds(0, 0, left, right);
				return;
			}
			// Gamepad.current.SetMotorSpeeds(left, right);
		}

		public void OnAccelerate(float throttle)
		{
			this.throttle = throttle;

			SetRumble(brake, throttle);
		}

		public void OnBrake(float brake)
		{
			this.brake = brake;

			SetRumble(brake, throttle);
		}
	}
}