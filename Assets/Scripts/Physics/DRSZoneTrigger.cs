using UnityEngine;

namespace Soap.Physics
{
	public class DRSZoneTrigger : MonoBehaviour
	{
		[SerializeField] private bool inZone;
		
		private void OnTriggerEnter(Collider other)
		{
			foreach (AeroSurface aeroSurface in other.GetComponentsInChildren<AeroSurface>())
			{
				aeroSurface.SetInDRSZone(inZone);
			}
		}
	}
}
