using UnityEngine;
using Soap.Physics;

namespace Soap.Prototype
{
	[System.Serializable]
	public class DifferentialV1
	{
		[SerializeField] private float preloadTorque;

		private Wheel[] wheels;

		private bool isLocked = true;

		private float velocityDeltaSign;

		public DifferentialV1(Wheel[] wheels, float preloadTorque)
		{
			this.wheels = wheels;
			this.preloadTorque = preloadTorque;
		}

		public void Accelerate(float torque)
		{
			float torqueDelta = wheels[0].Torque - wheels[1].Torque;
			float velocityDelta = wheels[0].WheelSpeed - wheels[1].WheelSpeed;

			if(isLocked == false)
			{
				// Velocity delta sign change
				if(velocityDelta * velocityDeltaSign < 0)
				{
					isLocked = true;
				}
			}
			else
			{
				if(Mathf.Abs(torqueDelta) > preloadTorque)
				{
					velocityDeltaSign = Mathf.Sign(velocityDelta);
					isLocked = false;
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
				Debug.Log("Locked");
				return;
			}

			if(wheels[0].Torque > wheels[1].Torque)
			{
				wheels[0].Accelerate(torque/2 + preloadTorque);
				wheels[1].Accelerate(torque/2 - preloadTorque);
			}
			else
			{
				wheels[0].Accelerate(torque/2 - preloadTorque);
				wheels[1].Accelerate(torque/2 + preloadTorque);
			}
		}
	}
}
