using UnityEngine;
using Soap.Physics;
using Soap.GameManagement;

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

		private void OnEnable()
		{
			ResetManager.OnReset += Reset;
		}

		private void OnDisable()
		{
			ResetManager.OnReset -= Reset;
		}

		private void Update()
		{
			if(Mathf.Abs(wheel.SlipRatio) >= 0.5)
			{
				particles.Emit(1);
			}
		}

		public void Reset()
		{
			StopParticles();
		}

		private void StopParticles()
		{
			particles.Stop();
			particles.Clear();
		}
	}
}