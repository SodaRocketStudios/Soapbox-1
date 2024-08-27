using UnityEngine;

namespace Soap.Physics
{
	public class Differential
	{
		private float preloadTorque;

		private float biasRatio;

		private Wheel[] wheels;

		private bool isLocked = true;

		private float velocityDeltaSign;

		private float torqueDelta;

		public Differential(Wheel[] wheels, float preloadTorque, float biasRatio)
		{
			this.wheels = wheels;
			this.preloadTorque = preloadTorque;
			this.biasRatio = biasRatio;
		}

		public void Accelerate(float engineTorque)
		{
			torqueDelta = Mathf.Abs(wheels[0].Torque - wheels[1].Torque);
			float ratio = wheels[0].Torque > wheels[1].Torque ? wheels[0].Torque / wheels[1].Torque : wheels[1].Torque / wheels[0].Torque;
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

			ApplyTorque(engineTorque);
		}

		private void ApplyTorque(float engineTorque)
		{
			if(isLocked)
			{
				wheels[0].Accelerate(engineTorque/2);
				wheels[1].Accelerate(engineTorque/2);
				return;
			}

			if(wheels[0].WheelSpeed > wheels[1].WheelSpeed)
			{
				wheels[0].Accelerate(engineTorque*(1/(1 + biasRatio)));
				wheels[1].Accelerate(engineTorque*(biasRatio/(1 + biasRatio)));
			}
			else
			{
				wheels[0].Accelerate(engineTorque*(biasRatio/(1 + biasRatio)));
				wheels[1].Accelerate(engineTorque*(1/(1 + biasRatio)));
			}
		}
	}
}