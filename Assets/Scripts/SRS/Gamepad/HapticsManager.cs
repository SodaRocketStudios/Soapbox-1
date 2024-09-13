using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

namespace SRS.Haptics
{
	public static class HapticsManager
	{
		public static void SetHaptics(float lowFrequencySpeed, float highFrequencySpeed, float leftTriggerSpeed, float rightTriggerSpeed)
		{
			if(Gamepad.current is IXboxOneRumble)
			{
				(Gamepad.current as IXboxOneRumble).SetMotorSpeeds(lowFrequencySpeed, highFrequencySpeed, leftTriggerSpeed, rightTriggerSpeed);
				return;
			}

			Gamepad.current.SetMotorSpeeds(lowFrequencySpeed, highFrequencySpeed);
		}

		public static GamepadType GetGamepadType(this InputDevice device)
		{
			if(device == null)
			{
				return GamepadType.None;
			}

			if(device is XInputController)
			{
				return GamepadType.XBox;
			}

			if(device is DualShockGamepad)
			{
				return GamepadType.DualShock;
			}

			if(device is SwitchProControllerHID)
			{
				return GamepadType.Switch;
			}

			return GamepadType.Other;
		}
	}
}