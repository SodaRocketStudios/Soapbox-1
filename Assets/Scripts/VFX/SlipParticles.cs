using UnityEngine;
using Soap.Physics;

namespace Soap.VFX
{
	public class SlipParticles : MonoBehaviour
	{
		private Wheel wheel;
		private ParticleSystem particles;

		private void Start()
		{
			wheel = GetComponent<Wheel>();
			particles = GetComponent<ParticleSystem>();
		}

		private void Update()
		{
			if(Mathf.Abs(wheel.SlipRatio) > 1f)
			{
				particles.Emit(1);
			}

			// TODO -- look more into slip angle and make sure the numbers make sense.

			if(wheel.SlipAngle > 15)
			{
				particles.Emit(1);
			}
		}
	}
}