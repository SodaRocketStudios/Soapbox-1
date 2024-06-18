using UnityEngine;
using Soap.Physics;

namespace Soap.Prototype
{
	public class DifferentialV1
	{
		private float preloadTorque;

		private float biasRatio;

		private Wheel[] wheels;

		private bool isLocked = true;

		private float velocityDeltaSign;

		private float torqueDelta;

		public DifferentialV1(Wheel[] wheels, float preloadTorque, float biasRatio)
		{
			this.wheels = wheels;
			this.preloadTorque = preloadTorque;
			this.biasRatio = biasRatio;
		}

		public void Accelerate(float torque)
		{
			torqueDelta = Mathf.Abs(wheels[0].Torque - wheels[1].Torque);
			float ratio = Mathf.Max(wheels[0].Torque / wheels[1].Torque, wheels[1].Torque / wheels[0].Torque);
			float velocityDelta = wheels[0].WheelSpeed - wheels[1].WheelSpeed;

			if(isLocked)
			{
				if(torqueDelta > preloadTorque && ratio > biasRatio)
				{
					velocityDeltaSign = Mathf.Sign(velocityDelta);
					isLocked = false;
				}
			}
			else
			{
				// Velocity delta sign change
				if(velocityDelta * velocityDeltaSign < 0)
				{
					isLocked = true;
				}
			}

			ApplyTorque(torque);
		}

		private void ApplyTorque(float torque)
		{
			if(isLocked)
			{
				wheels[0].Accelerate(torque/2);
				wheels[1].Accelerate(torque/2);
				return;
			}

			float correctingTorque = torqueDelta - preloadTorque;

			if(wheels[0].WheelSpeed > wheels[1].WheelSpeed)
			{
				wheels[0].Accelerate(torque/2 - correctingTorque);
				wheels[1].Accelerate(torque/2 + correctingTorque);
			}
			else
			{
				wheels[0].Accelerate(torque/2 + correctingTorque);
				wheels[1].Accelerate(torque/2 - correctingTorque);
			}
		}
	}
}
