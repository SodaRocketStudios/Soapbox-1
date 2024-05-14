using UnityEngine;

namespace Soap.Telemetry
{
	public class TelemetryManager : MonoBehaviour
	{
		[SerializeField] private GameObject forceArrowPrefab;

		private ForceArrow arrow;

		private void Start()
		{
			arrow = Instantiate(forceArrowPrefab).GetComponent<ForceArrow>();
		}

		public void DrawForce(Vector3 position, Vector3 force)
		{
			float magniutude = force.magnitude;
			arrow.Draw(position, force);
		}
	}
}
