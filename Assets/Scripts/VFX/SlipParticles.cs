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
			if(wheel.SlipRatio > 0)
			Debug.Log(wheel.SlipRatio, wheel);
			if(Mathf.Abs(wheel.SlipRatio) >= 0.5)
			{
				particles.Emit(1);
			}
		}
	}
}