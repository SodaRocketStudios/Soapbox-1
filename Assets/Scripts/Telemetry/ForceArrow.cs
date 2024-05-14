using UnityEngine;

namespace Soap.Telemetry
{
	[RequireComponent(typeof(MeshRenderer))]
	public class ForceArrow : MonoBehaviour
	{
		private MeshRenderer meshRenderer;

		public void ToggleVisibility()
		{
			meshRenderer.enabled = !meshRenderer.enabled;
		}

		public void Draw(Vector3 position, Vector3 force)
		{
			transform.position = position;
			transform.LookAt(position + force);
		}
	}
}
