using UnityEngine;
using UnityEngine.InputSystem;
using Soap.Physics;

namespace Soap.Haptics
{
	public class HapticsManager : MonoBehaviour
	{
		[SerializeField, Min(0)] private float EngineNoiseScale = 0.1f;
		[SerializeField, Min(0)] private float SlipFeedbackScale = 0.1f;
		private Rigidbody carRigibody;
		private CarController carController;

		private void Awake()
		{
			carRigibody = GetComponent<Rigidbody>();
			carController = GetComponent<CarController>();
		}

		private void Update()
		{
			float slip = 0;

			foreach(Wheel wheel in carController.Wheels)
			{
				if(Mathf.Abs(wheel.CombinedSlip) > slip)
				{
					if(wheel.IsGrounded)
					{
						slip = wheel.CombinedSlip;
					}
				}
			}

			SetHaptics(carController.AccelerationInput*EngineNoiseScale, slip*SlipFeedbackScale, 0, 0);
		}

		private void SetHaptics(float lowFrequencySpeed, float highFrequencySpeed, float leftTriggerSpeed, float rightTriggerSpeed)
		{
			if(Gamepad.current == null)
			{
				return;
			}

			// This seems to cause feedback lag.
			// if(Gamepad.current is IXboxOneRumble)
			// {
			// 	(Gamepad.current as IXboxOneRumble).SetMotorSpeeds(lowFrequencySpeed, highFrequencySpeed, leftTriggerSpeed, rightTriggerSpeed);
			// 	return;
			// }

			Gamepad.current.SetMotorSpeeds(lowFrequencySpeed, highFrequencySpeed);
		}

		public void StopHaptics()
		{
			Gamepad.current.ResetHaptics();
		}
	}
}